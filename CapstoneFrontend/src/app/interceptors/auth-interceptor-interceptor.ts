import { HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Auth } from '../services/auth/auth';

function withAuthHeader(req: HttpRequest<unknown>, token: string | null) {
  if (!token) {
    return req;
  }
  if (req.headers.has('Authorization')) {
    return req;
  }
  return req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });
}

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(Auth);
  const token = auth.getToken();
  const authReq = withAuthHeader(req, token);
  return next(authReq);
};

