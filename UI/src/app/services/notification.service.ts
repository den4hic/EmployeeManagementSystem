import { Injectable } from '@angular/core';
import {ApiPaths} from "./enums/api-paths";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {NotificationDto} from "./dtos/notification.dto";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiPath: string = ApiPaths.Notification;

  constructor(private http: HttpClient) { }

  getUserNotifications(userId: number): Observable<NotificationDto[]> {
    const url = `${this.apiPath}/get-notifications/${userId}`;
    return this.http.get<NotificationDto[]>(url);
  }

  getUnreadNotifications(userId: number): Observable<NotificationDto[]> {
    const url = `${this.apiPath}/get-unread-notifications/${userId}`;
    return this.http.get<NotificationDto[]>(url);
  }
}
