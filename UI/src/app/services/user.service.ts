import { inject, Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import { ApiPaths} from "./enums/api-paths";
import { JwtService } from "./jwt.service";
import { Observable, of, throwError } from "rxjs";
import { UserDto } from "./dtos/user.dto";
import { tap, catchError } from "rxjs/operators";
import {UserStatistics} from "./dtos/user-statistic.dto";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiPath : string = ApiPaths.User;
  private jwtService = inject(JwtService);
  private http = inject(HttpClient);

  private currentUser: UserDto | null = null;

  constructor() {
    this.loadUserFromStorage();
  }

  getUserInfo(isUpdate: boolean = false): Observable<UserDto> {
    if (this.currentUser && !isUpdate) {
      return of(this.currentUser);
    }

    const username = this.jwtService.getUsername();
    if (!username) {
      return throwError(() => new Error('User is not authenticated'));
    }

    return this.http.get<UserDto>(`${this.apiPath}/current/${username}`).pipe(
      tap(user => {
        user.username = username;
        const role = this.jwtService.getUserRole();
        if (role) {
          user.role = role;
        }
        this.currentUser = user;
        this.saveUserToStorage(user);
      }),
      catchError(error => {
        console.error('Failed to get user info', error);
        return throwError(() => error);
      })
    );
  }

  private saveUserToStorage(user: UserDto): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
  }

  private loadUserFromStorage(): void {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUser = JSON.parse(storedUser);
    }
  }

  clearUserInfo(): void {
    this.currentUser = null;
    localStorage.removeItem('currentUser');
  }

  getUsers(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(`${this.apiPath}`);
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.apiPath}/${userId}`);
  }

  getUsersWithDetails(page: number, pageSize: number, sortField: string, sortDirection: string, filter: string, selectedRole: string): Observable<{items: UserDto[], totalItems: number}> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('sortField', sortField)
      .set('sortDirection', sortDirection)
      .set('filter', filter)
      .set('role', selectedRole);

    return this.http.get<{items: UserDto[], totalItems: number}>(`${this.apiPath}/details/filtered`, { params });
  }

  getUserStatistics(): Observable<UserStatistics> {
    return this.http.get<UserStatistics>(`${this.apiPath}/statistics`);
  }

  blockUser(userId: number, isBlocked: boolean) {
    return this.http.put(`${this.apiPath}/block/${userId}`, isBlocked);
  }

  updateUserInfo(updatedUser: UserDto) {
    updatedUser.employee = null;
    updatedUser.manager = null;
    return this.http.put<void>(`${this.apiPath}`, updatedUser);
  }
}


