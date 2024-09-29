import { Injectable } from '@angular/core';
import {jwtDecode} from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  public getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  public getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  public getUserRole(): string | null {
    const token = this.getToken();
    if (token) {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.role || null;
    }
    return null;
  }

  public getUsername(): string | null {
    const token = this.getToken();
    if (token) {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.username || null;
    }
    return null;
  }

  public isInRole(role: string): boolean {
    const userRole = this.getUserRole();
    return userRole === role;
  }

  public isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;
    const decodedToken: any = jwtDecode(token);
    const expirationDate = new Date(0);
    expirationDate.setUTCSeconds(decodedToken.exp);
    return expirationDate.valueOf() <= new Date().valueOf();
  }
}
