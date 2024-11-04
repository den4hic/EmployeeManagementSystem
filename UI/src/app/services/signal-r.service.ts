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
  private hubConnection: HubConnection | null = null;
  public onlineUsers = new BehaviorSubject<string[]>([]);
  public notifications = new Subject<NotificationModel>();

  constructor() {
    this.connect();
  }

  private createConnection() {
    if (!this.hubConnection) {
      return;
    }
    this.hubConnection.on('UpdateUserList', (users: string[]) => {
      this.onlineUsers.next(users);
      console.log('Online users:', users);
    });

    this.hubConnection.on('ReceiveNotification', (message: NotificationType, notTitle: string) => {
      const notification : NotificationModel = { NotificationType: message, NotificationTitle: notTitle };
      this.notifications.next(notification);
      console.log('Received notification:', notTitle);
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
    if (!this.hubConnection) {
      return;
    }
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
    if (!this.hubConnection) {
      return;
    }
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

  sendSingleNotification(userId: number, notificationType: NotificationType, notTitle: string) {
    this.hubConnection?.invoke('SendNotification', userId.toString(), notificationType, notTitle)
      .catch(err => console.error(err));
  }

  createProjectGroup(id: number) {
    this.hubConnection?.invoke('CreateProjectGroup', id.toString())
      .catch(err => console.error(err));
  }

  sendProjectNotification(project: ProjectDto, notificationType: NotificationType) {
    this.hubConnection?.invoke('SendProjectNotification', project, notificationType)
      .catch(err => console.error(err));
  }

  public connect() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://employee-system-api-fre4cmdvake3dea2.westeurope-01.azurewebsites.net/notificationHub', {
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

  addUserToGroup(userId: number, number: number) {
    this.hubConnection?.invoke('AddUserToGroup', userId, number.toString())
      .catch(err => console.error(err));
  }

  removeUserFromGroup(userId: number, number: number) {
    this.hubConnection?.invoke('RemoveUserFromGroup', userId, number.toString())
      .catch(err => console.error(err));
  }
}
