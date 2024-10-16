import {Component, OnInit} from '@angular/core';
import {NotificationService} from '../../services/notification.service';
import {NotificationDto} from '../../services/dtos/notification.dto';
import {NotificationType} from '../../services/enums/notification-type';
import {UserService} from "../../services/user.service";
import {UserDto} from "../../services/dtos/user.dto";
import {animate, style, transition, trigger} from '@angular/animations';
import {SignalRService} from "../../services/signal-r.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(10px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
    ]),
  ],
})
export class NotificationsComponent implements OnInit {
  notifications: NotificationDto[] = [];
  user: UserDto | null = null;
  private notificationSubscription: Subscription = new Subscription();

  constructor(
    private notificationService: NotificationService,
    private signalRService: SignalRService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.loadNotifications();
    this.subscribeToNotifications();
  }

  ngOnDestroy() {
    if (this.notificationSubscription) {
      this.notificationSubscription.unsubscribe();
    }
  }

  private subscribeToNotifications() {
    this.notificationSubscription = this.signalRService.notifications.subscribe(
      (notification) => {
        console.log('Received new notification:', notification);
        this.loadNotifications();
      }
    );
  }

  loadNotifications() {
    this.userService.getUserInfo().subscribe(
      (user) => {
        this.user = user;
        if (this.user && this.user.id) {
          this.notificationService.getUserNotifications(this.user.id).subscribe({
            next: (notifications) => {
              notifications = notifications.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
              this.notifications = notifications;
            },
            error: (error) => {
              console.error('Failed to load notifications', error);
            }
          });
        }
      }
    );
  }

  public getActionText(type: NotificationType): string {
    switch (type) {
      case NotificationType.AssignedToTask:
        return 'assigned you to a task';
      case NotificationType.TaskStatusChanged:
        return 'changed the status of a task';
      case NotificationType.TaskDeleted:
        return 'deleted a task';
      case NotificationType.TaskCreated:
        return 'created a new task';
      case NotificationType.UnassignedFromTask:
        return 'unassigned you from a task';
      case NotificationType.TaskDueDateChanged:
        return 'changed the due date of a task';
      case NotificationType.NewProject:
        return 'created a new project';
      case NotificationType.UnassignedFromProject:
        return 'unassigned you from a project';
      case NotificationType.ProjectDeleted:
        return 'deleted a project';
      default:
        return 'made an update';
    }
  }

  getIcon(type: NotificationType): string {
    if (type < 6) {
      return 'assignment';
    } else if (type >= 6 && type < 9) {
      return 'folder';
    } else {
      return 'notifications';
    }
  }

  getIconClass(type: NotificationType): string {
    if (type < 6) {
      return 'icon-task';
    } else if (type >= 6 && type < 9) {
      return 'icon-project';
    } else {
      return 'icon-warning';
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
}
