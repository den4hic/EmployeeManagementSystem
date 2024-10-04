import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ApiConfigService } from '../services/api-config.service';

export const apiUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const apiConfigService = inject(ApiConfigService);
  const apiUrl = apiConfigService.getBaseUrl();

  if (!req.url.startsWith(apiUrl) && !req.url.startsWith('http')) {
    const apiReq = req.clone({ url: `${apiUrl}/${req.url}` });
    return next(apiReq);
  }

  return next(req);
};
