import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [MatButton],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss',
})
export class TestErrorComponent {
  // Base api url
  baseUrl = 'https://localhost:5001/api/';
  // http client is used to make requests
  private http = inject(HttpClient);
  // Holds all the validation errors produced
  validationErrors?: string[];

  // Not found error
  get404Error() {
    this.http.get(this.baseUrl + 'buggy/notfound').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }

  // Bad request error
  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }

  // Unauthorized error
  get401Error() {
    this.http.get(this.baseUrl + 'buggy/unauthorized').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }

  // Internal error
  get500Error() {
    this.http.get(this.baseUrl + 'buggy/internalerror').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }

  // Validation error
  get400ValidationError() {
    this.http.post(this.baseUrl + 'buggy/validationerror', {}).subscribe({
      next: (response) => console.log(response),
      error: (error) => (this.validationErrors = error),
    });
  }
}
