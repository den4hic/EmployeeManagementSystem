import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthService} from "../services/auth.service";
import {JwtService} from "../services/jwt.service";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const jwtService = inject(JwtService);
  const router = inject(Router);

  if (jwtService.isTokenExpired()) {
    authService.logout();
    router.navigate(['/landing']);
    return false;
  }

  return true;
};
