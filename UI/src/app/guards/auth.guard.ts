import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthService} from "../services/auth.service";
import {JwtService} from "../services/jwt.service";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const jwtService = inject(JwtService);
  const router = inject(Router);

  if (jwtService.isTokenExpired()) {
    authService.refreshToken().subscribe(
      response => {
        console.log('Token refreshed:', response);
        return true;
      },
      error => {
        console.error('Token refresh failed:', error);
        authService.logout();
        router.navigate(['/login']);
        return false;
      }
    );
  }

  if (jwtService.getUserIsBlocked()) {
    router.navigate(['/blocked']);
    return false;
  }

  return true;
};
