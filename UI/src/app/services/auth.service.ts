import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import {RegisterDto} from "./dtos/register.dto";
import {LoginDto} from "./dtos/login.dto";
import {TokenDto} from "./dtos/token.dto";
import {UserService} from "./user.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7110/api/account';

  constructor(private http: HttpClient, private userService: UserService) {}

  register(model: RegisterDto): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/register`, model);
  }

  login(model: LoginDto): Observable<TokenDto> {
    return this.http.post<TokenDto>(`${this.apiUrl}/login`, model).pipe(
      tap(response => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
      })
    );
  }

  logout(): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/logout`, {}).pipe(
      tap(() => {
        this.userService.clearUserInfo();
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
      })
    );
  }

  refreshToken(tokenDto: TokenDto): Observable<TokenDto> {
    return this.http.post<TokenDto>(`${this.apiUrl}/refresh-token`, tokenDto).pipe(
      tap(response => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
      })
    );
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('accessToken');
  }
}
