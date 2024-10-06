import { CanActivateFn, Router } from '@angular/router';
import { inject } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { JwtService } from "../services/jwt.service";
import { Observable, of, switchMap } from 'rxjs';
import {catchError} from "rxjs/operators";

export const authGuard: CanActivateFn = (route, state): Observable<boolean> => {
  const authService = inject(AuthService);
  const jwtService = inject(JwtService);
  const router = inject(Router);

  if (jwtService.getUserIsBlocked()) {
    router.navigate(['/blocked']);
    return of(false);
  }

  if (jwtService.isTokenExpired()) {
    return authService.refreshToken().pipe(
      switchMap(response => {
        console.log('Token refreshed:', response);
        return of(true);
      }),
      catchError(error => {
        console.error('Token refresh failed:', error);
        authService.logout();
        router.navigate(['/landing']);
        return of(false);
      })
    );
  }

  return of(true);
};
