import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AuthService} from "./auth.service";
import {JwtService} from "./jwt.service";
import {Observable} from "rxjs";
import {TokenDto} from "./dtos/token.dto";
import {UserDto} from "./dtos/user.dto";
import {tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7110/api/user';
  private authService = inject(AuthService);
  private jwtService = inject(JwtService);
  private http = inject(HttpClient);

  constructor() { }

  getUserInfo() : Observable<UserDto> | null {
    const username = this.jwtService.getUsername();
    if (!username) return null;
    return this.http.get<UserDto>(`${this.apiUrl}/current/${username}`).pipe(
      tap(user => {
        user.username = username;
      })
    );
  }
}
