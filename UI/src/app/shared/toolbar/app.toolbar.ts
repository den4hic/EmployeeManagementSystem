import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { SignalRService } from "../../services/signal-r.service";
import { NotificationService } from "../../services/notification.service";
import { UserService } from "../../services/user.service";
import { UserDto } from "../../services/dtos/user.dto";
import { NotificationDto } from "../../services/dtos/notification.dto";
import {NotificationType} from "../../services/enums/notification-type";

@Component({
  selector: 'app-toolbar',
  templateUrl: 'app.toolbar.html',
  styleUrls: ['app.toolbar.css']
})
export class ToolbarComponent implements OnInit {
  public user: UserDto | null = null;
  public notifications: NotificationDto[] = [];
  public isLoading = false;

  constructor(
    private authService: AuthService,
    private signalRService: SignalRService,
    private notificationService: NotificationService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit() {
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
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

    this.notificationService.getUserNotifications(this.user.id).subscribe({
      next: (notifications) => {
        this.notifications = notifications.slice(0, 4); // Get only the latest 4 notifications
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to get notifications', error);
        this.isLoading = false;
      }
    });
  }

  viewAllNotifications() {
    this.router.navigate(['/notifications']);
  }

  getNotificationText(notification: NotificationDto): string {
    const action = this.getActionText(notification.type);
    const target = this.getTargetText(notification.type);
    return `${action} ${target}`;
  }

  private getActionText(type: NotificationType): string {
    switch (type) {
      case NotificationType.AssignedToTask:
        return 'assigned to';
      case NotificationType.TaskStatusChanged:
        return 'changed status of';
      case NotificationType.TaskDeleted:
        return 'deleted';
      case NotificationType.TaskCreated:
        return 'created';
      case NotificationType.UnassignedFromTask:
        return 'unassigned from';
      case NotificationType.TaskDueDateChanged:
        return 'changed due date of';
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

  private getTargetText(type: NotificationType): string {
    return type.toString().includes('Task') ? 'task' : 'project';
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

}
