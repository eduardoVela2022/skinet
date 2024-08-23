import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  // Base API url
  baseUrl = 'https://localhost:5001/api/';

  // Http client (Has to be passed in app.config.ts first)
  private http = inject(HttpClient);

  // Filter by types
  types: string[] = [];

  // Filter by brands
  brands: string[] = [];

  // Gets the products
  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    // Brands filter
    if (shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }

    // Types filter
    if (shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }

    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }

    // Pagination filter
    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {
      params,
    });
  }

  // Gets the brands of the products
  getBrands() {
    if (this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: (response) => (this.brands = response),
    });
  }

  // Gets the types of the products
  getTypes() {
    if (this.types.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: (response) => (this.types = response),
    });
  }

  constructor() {}
}
