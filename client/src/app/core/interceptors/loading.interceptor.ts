import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize, identity } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { environment } from '../../../environments/environment';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  // Service that controls loading
  const busyService = inject(BusyService);

  // App is loading
  busyService.busy();

  // Adds delay to the requests the app makes
  return next(req).pipe(
    // Artificial delay
    environment.production ? identity : delay(500),
    // App is no longer loading
    finalize(() => busyService.idle())
  );
};
