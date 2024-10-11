import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import {JwtService} from "./jwt.service";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;
  private onlineUsers = new BehaviorSubject<string[]>([]);

  constructor(private jwtService: JwtService) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7110/onlineUsersHub', {
        accessTokenFactory: () => {
          const token = this.jwtService.getToken();
          if (!token) {
            throw new Error('No token found');
          }
          return token;
        }
      })
      .build();

    this.hubConnection.on('UserConnected', (userId: string) => {
      console.log(`User connected: ${userId}`);
      this.updateOnlineUsers(userId, true);
    });

    this.hubConnection.on('UserDisconnected', (userId: string) => {
      console.log(`User disconnected: ${userId}`);
      this.updateOnlineUsers(userId, false);
    });

    this.hubConnection.on('OnlineUsers', (users: string[]) => {
      console.log('Online users:', users);
      this.onlineUsers.next(users);
    });

    this.startConnection();
  }

  private startConnection(): void {
    this.hubConnection.start()
      .then(() => {
        console.log('Connected to SignalR Hub');
        this.hubConnection.invoke('GetOnlineUsers');
      })
      .catch(err => console.error('Error while starting connection: ' + err));
  }

  private updateOnlineUsers(userId: string, isOnline: boolean): void {
    const currentUsers = this.onlineUsers.value;
    if (isOnline && !currentUsers.includes(userId)) {
      this.onlineUsers.next([...currentUsers, userId]);
    } else if (!isOnline) {
      this.onlineUsers.next(currentUsers.filter(id => id !== userId));
    }
  }

  public getOnlineUsers(): Observable<string[]> {
    return this.onlineUsers.asObservable();
  }
}
