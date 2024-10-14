import {NotificationType} from "../enums/notification-type";
import {TaskDto} from "./task.dto";

export interface NotificationModel {
  NotificationType: NotificationType;
  NotificationTitle: string;
}
