import {NotificationType} from "../enums/notification-type";
import {GroupDto} from "./group.dto";
import {UserDto} from "./user.dto";

export interface NotificationDto {
  id: number;
  groupId: number;
  message: string;
  createdAt: Date;
  receiverId: number;
  senderId: number;
  group: GroupDto;
  receiver: UserDto;
  sender: UserDto;
  type: NotificationType;

}
