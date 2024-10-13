import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { trigger, transition, style, animate } from '@angular/animations';
import {SignalRService} from "../../services/signal-r.service";
import {NotificationType} from "../../services/enums/notification-type";

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css',
  animations: [
    trigger('notificationAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-20px)' }),
        animate('300ms', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
      transition(':leave', [
        animate('300ms', style({ opacity: 0, transform: 'translateY(-20px)' })),
      ]),
    ]),
  ],
})
export class NotificationComponent implements OnInit, OnDestroy {
  private notificationSubscription: Subscription = new Subscription();
  notifications: any[] = [];

  constructor(private signalRService: SignalRService) {}

  ngOnInit() {
    this.notificationSubscription = this.signalRService.notifications.subscribe(
      (notificationType: NotificationType) => {
        this.addNotification(notificationType);
      }
    );
  }

  ngOnDestroy() {
    if (this.notificationSubscription) {
      this.notificationSubscription.unsubscribe();
    }
  }

  private addNotification(notificationType: NotificationType) {
    const notification = this.createNotification(notificationType);
    this.notifications.unshift(notification);
    setTimeout(() => this.removeNotification(notification), 5000);
  }

  private createNotification(notificationType: NotificationType) {
    let title = '';
    let message = '';
    let icon = '';
    let notificationClass = '';

    switch (notificationType) {
      case NotificationType.AssignedToTask:
        title = 'New Task Assignment';
        message = 'You have been assigned to a new task.';
        icon = 'fas fa-tasks';
        notificationClass = 'notification-info';
        break;
      case NotificationType.TaskStatusChanged:
        title = 'Task Status Updated';
        message = 'A task status has been updated.';
        icon = 'fas fa-sync';
        notificationClass = 'notification-warning';
        break;
      case NotificationType.TaskDeleted:
        title = 'Task Deleted';
        message = 'A task has been deleted.';
        icon = 'fas fa-trash-alt';
        notificationClass = 'notification-danger';
        break;
      case NotificationType.TaskCreated:
        title = 'New Task Created';
        message = 'A new task has been created.';
        icon = 'fas fa-plus-circle';
        notificationClass = 'notification-success';
        break;
      case NotificationType.TaskUpdated:
        title = 'Task Updated';
        message = 'A task has been updated.';
        icon = 'fas fa-edit';
        notificationClass = 'notification-info';
        break;
      case NotificationType.TaskDueDateChanged:
        title = 'Task Due Date Changed';
        message = 'A task due date has been changed.';
        icon = 'fas fa-calendar-alt';
        notificationClass = 'notification-warning';
        break;
      default:
        title = 'New Notification';
        message = 'You have a new notification.';
        icon = 'fas fa-bell';
        notificationClass = 'notification-info';
    }

    return { title, message, icon, class: notificationClass };
  }

  removeNotification(notification: any) {
    const index = this.notifications.indexOf(notification);
    if (index > -1) {
      this.notifications.splice(index, 1);
    }
  }
}
