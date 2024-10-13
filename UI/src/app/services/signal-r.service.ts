import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;
  public onlineUsers = new BehaviorSubject<string[]>([]);


  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7110/onlineUsersHub', {
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
}
