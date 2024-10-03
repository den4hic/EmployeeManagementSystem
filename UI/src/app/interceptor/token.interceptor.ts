import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from "@angular/core";
import { JwtService } from "../services/jwt.service";
import { catchError, switchMap } from "rxjs/operators";
import { AuthService } from "../services/auth.service";
import { throwError, Observable } from "rxjs";
import { Router } from "@angular/router";

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const jwtService = inject(JwtService);
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!jwtService.getToken()) {
    return next(req);
  }

  const cloned = addToken(req, jwtService.getToken()!);

  return next(cloned).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return handleUnauthorizedError(authService, router, req, next);
      }
      return throwError(() => error);
    })
  );
};

function addToken(req: any, token: string) {
  return req.clone({
    headers: req.headers.set("Authorization", `Bearer ${token}`)
  });
}

function handleUnauthorizedError(
  authService: AuthService,
  router: Router,
  req: any,
  next: any
): Observable<any> {
  console.log('Unauthorized request, refreshing token...');
  return authService.refreshToken().pipe(
    switchMap(token => {
      console.log('Token refreshed:', token);
      const clonedReq = addToken(req, token.accessToken);
      return next(clonedReq);
    }),
    catchError(refreshError => {
      console.error('Token refresh failed:', refreshError);
      authService.logout();
      router.navigate(['/login']);
      return throwError(() => refreshError);
    })
  );
}
