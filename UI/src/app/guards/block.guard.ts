import {CanActivateFn, Router} from '@angular/router';
import {JwtService} from "../services/jwt.service";
import {inject} from "@angular/core";

export const blockGuard: CanActivateFn = (route, state) => {
  const jwtService = inject(JwtService);
  const router = inject(Router);

  if (jwtService.getUserIsBlocked()) {
    return true;
  }

  router.navigate(['/']);
  return false;
};
