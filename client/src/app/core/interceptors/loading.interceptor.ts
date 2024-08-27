import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  // Service that controls loading
  const busyService = inject(BusyService);

  // App is loading
  busyService.busy();

  // Adds delay to the requests the app makes
  return next(req).pipe(
    // Artificial delay
    delay(500),
    // App is no longer loading
    finalize(() => busyService.idle())
  );
};
