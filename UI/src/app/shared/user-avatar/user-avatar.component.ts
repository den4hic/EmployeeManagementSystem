import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { SignalRService } from '../../services/signal-r.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrl: './user-avatar.component.css',
})
export class UserAvatarComponent implements OnInit, OnDestroy {
  @Input() imageSrc: string = 'assets/default-avatar.png';
  @Input() altText: string = 'User avatar';
  @Input() userId: string = '0';

  isOnline: boolean = false;
  private onlineUsersSubscription: Subscription = new Subscription();

  constructor(private signalRService: SignalRService) {}

  ngOnInit() {
    this.onlineUsersSubscription = this.signalRService.getOnlineUsers().subscribe(
      onlineUsers => {
        this.isOnline = onlineUsers.includes(this.userId);
      }
    );
  }

  ngOnDestroy() {
    if (this.onlineUsersSubscription) {
      this.onlineUsersSubscription.unsubscribe();
    }
  }
}
