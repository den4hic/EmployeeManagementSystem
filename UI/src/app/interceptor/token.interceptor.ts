import { HttpInterceptorFn } from '@angular/common/http';
import {AuthService} from "../services/auth.service";
import {inject} from "@angular/core";
import {JwtService} from "../services/jwt.service";

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const jwtService = inject(JwtService);

  if (jwtService.getToken()){
    const cloned = req.clone({
      headers : req.headers.set("Authorization", "Bearer " + jwtService.getToken())
    })

    return next(cloned);
  }

  return next(req);
};
