import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { trigger, transition, style, animate } from '@angular/animations';
import {SignalRService} from "../../services/signal-r.service";
import {NotificationType} from "../../services/enums/notification-type";
import {NotificationModel} from "../../services/dtos/notification";
import {TaskStatus} from "../../services/enums/task-status";

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
      (notificationModel: NotificationModel) => {
        this.addNotification(notificationModel);
      }
    );
  }

  ngOnDestroy() {
    if (this.notificationSubscription) {
      this.notificationSubscription.unsubscribe();
    }
  }

  private addNotification(notificationModel: NotificationModel) {
    const notification = this.createNotification(notificationModel);
    this.notifications.unshift(notification);
    setTimeout(() => this.removeNotification(notification), 5000);
  }

  private createNotification(notificationModel: NotificationModel) {
    let title = '';
    let message = '';
    let icon = '';
    let notificationClass = '';
    const notificationTitle = notificationModel.NotificationTitle;

    switch (notificationModel.NotificationType) {
      case NotificationType.AssignedToTask:
        title = 'New Task Assignment';
        message = `You have been assigned to a ${notificationTitle} task.`;
        icon = 'add_task';
        notificationClass = 'notification-info';
        break;
      case NotificationType.TaskStatusChanged:
        title = 'Task Status Updated';
        message = `A ${notificationTitle} task status has been updated.`;
        icon = 'task';
        notificationClass = 'notification-warning';
        break;
      case NotificationType.TaskDeleted:
        title = 'Task Deleted';
        message = `A ${notificationTitle} task has been deleted.`;
        icon = 'delete_sweep';
        notificationClass = 'notification-danger';
        break;
      case NotificationType.TaskCreated:
        title = 'New Task Created';
        message = `A new ${notificationTitle} task has been created.`;
        icon = 'add_task';
        notificationClass = 'notification-success';
        break;
      case NotificationType.UnassignedFromTask:
        title = 'Unassigned From Task';
        message = `You have been unassigned from a ${notificationTitle} task.`;
        icon = 'person_remove';
        notificationClass = 'notification-info';
        break;
      case NotificationType.TaskDueDateChanged:
        title = 'Task Due Date Changed';
        message = 'A task due date has been changed.';
        icon = 'schedule';
        notificationClass = 'notification-warning';
        break;
      case NotificationType.NewProject:
        title = 'New Project Created';
        message = `A new ${notificationTitle} project has been created.`;
        icon = 'group';
        notificationClass = 'notification-success';
        break;
      case NotificationType.UnassignedFromProject:
        title = 'Unassigned From Project';
        message = `You have been unassigned from a ${notificationTitle} project.`;
        icon = 'person_remove';
        notificationClass = 'notification-info';
        break;
      case NotificationType.ProjectDeleted:
        title = 'Project Deleted';
        message = `A ${notificationTitle} project has been deleted.`;
        icon = 'delete_sweep';
        notificationClass = 'notification-danger';
        break;
      default:
        title = 'New Notification';
        message = 'You have a new notification.';
        icon = 'task';
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
