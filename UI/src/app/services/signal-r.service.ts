import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {NotificationType} from "./enums/notification-type";
import {TaskDto} from "./dtos/task.dto";
import {NotificationModel} from "./dtos/notification";
import {ProjectDto} from "./dtos/project.dto";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;
  public onlineUsers = new BehaviorSubject<string[]>([]);
  public notifications = new Subject<NotificationModel>();

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7110/notificationHub', {
        accessTokenFactory: () => {
          const token = localStorage.getItem('accessToken');
          if (!token) {
            throw new Error('No token found');
          }
          return token;
        }
      })
      .withAutomaticReconnect()
      .build();
    this.createConnection();
    this.startConnection();
  }

  private createConnection() {
    this.hubConnection.on('UpdateUserList', (users: string[]) => {
      this.onlineUsers.next(users);
      console.log('Online users:', users);
    });

    this.hubConnection.on('ReceiveNotification', (message: NotificationType, taskTitle: string) => {
      const notification : NotificationModel = { NotificationType: message, NotificationTitle: taskTitle };
      this.notifications.next(notification);
      console.log('Received notification:', taskTitle);
    });

    this.hubConnection.onreconnecting((error) => {
      console.log('Attempting to reconnect:', error);
    });

    this.hubConnection.onreconnected((connectionId) => {
      console.log('Reconnected with ID:', connectionId);
    });

    this.hubConnection.onclose((error) => {
      console.log('Connection closed:', error);
      this.startConnection();
    });
  }

  private startConnection() {
    if (this.hubConnection.state === HubConnectionState.Disconnected) {
      this.hubConnection.start()
        .then(() => console.log('Connection started'))
        .catch(err => {
          console.log('Error while starting connection: ' + err);
          setTimeout(() => this.startConnection(), 5000);
        });
    }
  }

  public async disconnect() {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      try {
        await this.hubConnection.stop();
        console.log('Disconnected');
      } catch (err) {
        console.log('Error while disconnecting: ' + err);
      }
    }
  }

  public getOnlineUsers(): Observable<string[]> {
    return this.onlineUsers.asObservable();
  }

  sendTaskUpdate(userId: number, notificationType: NotificationType, taskDto: TaskDto) {
    this.hubConnection.invoke('SendNotification', userId.toString(), notificationType, taskDto)
      .catch(err => console.error(err));
  }

  createProjectGroup(id: number) {
    this.hubConnection.invoke('CreateProjectGroup', id.toString())
      .catch(err => console.error(err));
  }

  sendProjectNotification(project: ProjectDto, message: string, notificationType: NotificationType) {
    this.hubConnection.invoke('SendProjectNotification', project, message, notificationType)
      .catch(err => console.error(err));
  }
}
