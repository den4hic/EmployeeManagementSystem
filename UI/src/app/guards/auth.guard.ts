import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthService} from "../services/auth.service";
import {JwtService} from "../services/jwt.service";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const jwtService = inject(JwtService);
  const router = inject(Router);

  if (jwtService.isTokenExpired()) {
    router.navigate(['/landing']);
    return false;
  }
  console.log(jwtService.getUserIsBlocked() === false);
  if (jwtService.getUserIsBlocked()) {
    console.log(1);
    router.navigate(['/blocked']);
    return false;
  }

  return true;
};
