import {CanActivateFn, Router} from '@angular/router';
import {AuthService} from "../services/auth.service";
import {inject} from "@angular/core";
import {MatSnackBar} from "@angular/material/snack-bar";
import {JwtService} from "../services/jwt.service";

export const roleGuard: CanActivateFn = (route, state) => {
  const roles = route.data['roles'] as string[];
  const authService = inject(AuthService);
  const jwtService = inject(JwtService);
  const matSnackBar = inject(MatSnackBar);
  const router = inject(Router);

  const role = jwtService.getUserRole();

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

  if (role != null && roles.includes(role)) {
    return true;
  }

  router.navigate(['/']);
  matSnackBar.open('You cannot access to this page', 'Close', {
    duration: 3000
  });

  return false;
};
