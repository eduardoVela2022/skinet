import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/service/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CurrencyPipe,
    MatButton,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    MatDivider,
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  // Shop service
  private shopService = inject(ShopService);
  // Route
  private activatedRoute = inject(ActivatedRoute);
  // Product
  product?: Product;

  // Loads product info on init
  ngOnInit(): void {
    this.loadProduct();
  }

  // Loads product
  loadProduct() {
    // Id is obtained from URL params
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    // Does id exists?
    if (!id) return;

    // Sets product to obtained product
    this.shopService.getProduct(+id).subscribe({
      next: (product) => (this.product = product),
      error: (error) => console.log(error),
    });
  }
}
