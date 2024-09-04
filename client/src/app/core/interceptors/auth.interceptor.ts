import { HttpInterceptorFn } from '@angular/common/http';

// Adds the with credentials to requests
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    withCredentials: true,
  });

  return next(clonedRequest);
};
