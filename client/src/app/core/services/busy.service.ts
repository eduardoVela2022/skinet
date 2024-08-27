import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  loading = false;
  busyRequestCount = 0;

  // App is loading
  busy() {
    this.busyRequestCount++;
    this.loading = true;
  }

  // App is not loading
  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.loading = false;
    }
  }
}
