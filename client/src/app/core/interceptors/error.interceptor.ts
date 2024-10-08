import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

// Catches errors
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  // Router
  const router = inject(Router);
  // Snackbar service
  const snackbar = inject(SnackbarService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      // 400 error
      if (err.status === 400) {
        // 400 validation error
        if (err.error.errors) {
          const modelStatesErrors = [];

          for (const key in err.error.errors) {
            if (err.error.errors[key]) {
              modelStatesErrors.push(err.error.errors[key]);
            }
          }

          throw modelStatesErrors.flat();
        }
        // Normal 400 error
        else {
          snackbar.error(err.error.title || err.error);
        }
      }

      // 401 error
      if (err.status === 401) {
        snackbar.error(err.error.title || err.error);
      }

      // 404 error
      if (err.status === 404) {
        router.navigateByUrl('/not-found');
      }

      // 500 error
      if (err.status === 500) {
        const navigationExtras: NavigationExtras = {
          state: { error: err.error },
        };
        router.navigateByUrl('/server-error', navigationExtras);
      }

      return throwError(() => err);
    })
  );
};
