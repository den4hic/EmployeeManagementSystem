import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {SignalRService} from "../../services/signal-r.service";
import {NotificationService} from "../../services/notification.service";
import {UserService} from "../../services/user.service";
import {UserDto} from "../../services/dtos/user.dto";
import {NotificationDto} from "../../services/dtos/notification.dto";
import {NotificationType} from "../../services/enums/notification-type";
import {Subscription} from "rxjs";
import {JwtService} from "../../services/jwt.service";

@Component({
  selector: 'app-toolbar',
  templateUrl: 'app.toolbar.html',
  styleUrls: ['app.toolbar.css']
})
export class ToolbarComponent implements OnInit {
  public user: UserDto | null = null;
  public notifications: NotificationDto[] = [];
  public isLoading = false;
  private notificationSubscription: Subscription = new Subscription();
  public shownNotifications: NotificationDto[] = [];
  private readonly maxNotifications = 4;
  private userDto: UserDto | null = null;

  constructor(
    private authService: AuthService,
    private jwtService: JwtService,
    private signalRService: SignalRService,
    private notificationService: NotificationService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadUserDataIfNeeded();
    this.subscribeToNotifications();
  }

  ngOnDestroy() {
    if (this.notificationSubscription) {
      this.notificationSubscription.unsubscribe();
    }
  }

  isAuthenticated(): boolean {
    return !this.jwtService.isTokenExpired();
  }

  goToProfile(): void {
    this.router.navigate(['/profile']);
  }

  logout(): void {
    this.authService.logout().subscribe(() => {
      this.router.navigate(['/login']);
      this.signalRService.disconnect();
    });
  }

  loadUserDataIfNeeded() {
    if (this.isAuthenticated() && !this.user && !this.isLoading) {
      this.isLoading = true;
      this.loadUserInfo();
    }
  }

  loadUserInfo() {
    this.userService.getUserInfo().subscribe(
      (user) => {
        this.user = user;
        this.loadNotifications();
      },
      (error) => {
        console.error('Failed to get user info', error);
        this.isLoading = false;
      }
    );
  }

  loadNotifications() {
    if (!this.user) return;

    this.notificationService.getUnreadNotifications(this.user.id).subscribe({
      next: (notifications) => {
        notifications = notifications.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        this.notifications = notifications;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to get notifications', error);
        this.isLoading = false;
      }
    });
  }

  viewAllNotifications() {
    for (const notification of this.notifications) {
      if (!this.user)
        return;
      this.notificationService.markNotificationAsRead(notification.id, this.user.id).subscribe();
    }

    this.router.navigate(['/notifications']);
  }

  getNotificationText(notification: NotificationDto): string {
    return this.getActionText(notification.type);
  }

  private getActionText(type: NotificationType): string {
    switch (type) {
      case NotificationType.AssignedToTask:
        return 'assigned to a task';
      case NotificationType.TaskStatusChanged:
        return 'changed status of a task';
      case NotificationType.TaskDeleted:
        return 'deleted task';
      case NotificationType.TaskCreated:
        return 'created task';
      case NotificationType.UnassignedFromTask:
        return 'unassigned from a task';
      case NotificationType.TaskDueDateChanged:
        return 'changed due date of a task';
      case NotificationType.NewProject:
        return 'created new project';
      case NotificationType.UnassignedFromProject:
        return 'unassigned from project';
      case NotificationType.ProjectDeleted:
        return 'deleted project';
      default:
        return 'updated';
    }
  }

  getTimeAgo(date: Date): string {
    const now = new Date();
      const diff = now.getTime() - new Date(date).getTime();
      const minutes = Math.floor(diff / 60000);
      if (minutes < 60) {
        return `${minutes} min ago`;
      }
      const hours = Math.floor(minutes / 60);
      if (hours < 24) {
        return `${hours} hours ago`;
      }
      const days = Math.floor(hours / 24);
      return `${days} days ago`;
    }

  private subscribeToNotifications() {
    this.notificationSubscription = this.signalRService.notifications.subscribe(
      (notification) => {
        console.log('Received new notification:', notification);
        this.loadNotifications();
      }
    );
  }

  onNotificationMenuClick() {
    this.shownNotifications = this.notifications.slice(0, this.maxNotifications);
    for (const shownNotification of this.shownNotifications) {
      if (!this.user)
          return
      this.notificationService.markNotificationAsRead(shownNotification.id, this.user.id).subscribe();
    }

    this.notifications = this.notifications.filter((notification) => !this.shownNotifications.includes(notification));
  }
}
