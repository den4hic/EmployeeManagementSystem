import {NotificationType} from "../enums/notification-type";
import {GroupDto} from "./group.dto";

export interface NotificationDto {
  id: number;
  groupId: number;
  message: string;
  createdAt: Date;
  receiverId: number;
  group: GroupDto;
  type: NotificationType;

}
