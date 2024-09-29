import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {AuthService} from "../services/auth.service";
import {inject} from "@angular/core";
import {JwtService} from "../services/jwt.service";
import {catchError, switchMap, tap} from "rxjs/operators";
import {error} from "@angular/compiler-cli/src/transformers/util";
import {throwError} from "rxjs";

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const jwtService = inject(JwtService);

  if (!jwtService.getToken()) return next(req);


  const cloned = req.clone({
    headers : req.headers.set("Authorization", "Bearer " + jwtService.getToken())
  })


  return next(cloned).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401) {
        const authService = inject(AuthService);
        const token = jwtService.getToken();
        const refreshToken = jwtService.getRefreshToken();

        console.log('Token interceptor', token, refreshToken);

        if (!token || !refreshToken) {
          authService.logout();
          return throwError(() => new Error('Authentication required'));
        }

        return authService.refreshToken({ accessToken: token, refreshToken: refreshToken }).pipe(
          switchMap(() => {
            const cloned = req.clone({
              headers: req.headers.set("Authorization", "Bearer " + jwtService.getToken())
            });
            return next(cloned);
          }),
          catchError((refreshError) => {
            authService.logout();
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => err);
    })
  );
};
