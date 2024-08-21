import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header.component';
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

// Component
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})

// Component props
export class AppComponent implements OnInit {
  // Base API url
  baseUrl = 'https://localhost:5001/api/';

  // Http client (Has to be passed in app.config.ts first)
  private http = inject(HttpClient);

  title = 'Skinet';
  products: Product[] = [];

  // When page loads
  ngOnInit(): void {
    // Get all products
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      next: (response) => (this.products = response.data), // What to do with the data
      error: (error) => console.log(error), // What to do when an error happens
      complete: () => console.log('complete'), // What to do when http response finishes
    });
  }
}
