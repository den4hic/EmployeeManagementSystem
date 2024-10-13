import { Component, OnInit, OnDestroy } from '@angular/core';

import { Subscription } from 'rxjs';
import {SignalRService} from "../../services/signal-r.service";

@Component({
  selector: 'app-online-users',
  template: `
    <h2>Online Users</h2>
    <ul>
      <li *ngFor="let user of onlineUsers">{{ user }}</li>
    </ul>
  `
})
export class OnlineUsersComponent implements OnInit, OnDestroy {
  onlineUsers: string[] = [];
  private subscription: Subscription = new Subscription();

  constructor(private signalRService: SignalRService) {}

  ngOnInit() {
    this.subscription = this.signalRService.getOnlineUsers().subscribe(
      users => this.onlineUsers = users
    );
    window.addEventListener('beforeunload', this.handleUnload.bind(this));
  }

  ngOnDestroy() {
    window.removeEventListener('beforeunload', this.handleUnload.bind(this));
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  private handleUnload() {
    this.signalRService.disconnect();
  }
}
