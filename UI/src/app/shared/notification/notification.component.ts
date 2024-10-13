import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import {SignalRService} from "../../services/signal-r.service";
import {NotificationType} from "../../services/enums/notification-type";

@Component({
  selector: 'app-notification',
  template: '',
})
export class NotificationComponent implements OnInit, OnDestroy {
  private notificationSubscription: Subscription = new Subscription();

  constructor(
    private signalRService: SignalRService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.notificationSubscription = this.signalRService.notifications.subscribe(
      (notificationType: NotificationType) => {
        this.showNotification(notificationType);
      }
    );
  }

  ngOnDestroy() {
    if (this.notificationSubscription) {
      this.notificationSubscription.unsubscribe();
    }
  }

  private showNotification(notificationType: NotificationType) {
    let message = '';
    let action = 'Close';
    let duration = 5000;

    switch (notificationType) {
      case NotificationType.AssignedToTask:
        message = 'You have been assigned to a new task.';
        break;
      case NotificationType.TaskStatusChanged:
        message = 'A task status has been updated.';
        break;
      case NotificationType.TaskDeleted:
        message = 'A task you are assigned to has been deleted.';
        break;
      case NotificationType.TaskCreated:
        message = 'A new task has been created.';
        break;
      case NotificationType.TaskUpdated:
        message = 'A task has been updated.';
        break;
      case NotificationType.TaskDueDateChanged:
        message = 'A task due date has been changed.';
        break;
      default:
        message = 'You have a new notification.';
    }

    this.snackBar.open(message, action, {
      duration: duration,
      horizontalPosition: 'end',
      verticalPosition: 'top',
    });
  }
}
