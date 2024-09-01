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
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

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
    FormsModule,
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  // Shop service
  private shopService = inject(ShopService);
  // Route
  private activatedRoute = inject(ActivatedRoute);
  private cartService = inject(CartService);
  // Product
  product?: Product;
  // Product quantities
  quantityInCart = 0;
  quantity = 1;

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
      next: (product) => {
        this.product = product;
        this.updateQuantityInCart();
      },
      error: (error) => console.log(error),
    });
  }

  // Updates the cart
  updateCart() {
    // When product is still being fetched from database, return
    if (!this.product) return;

    // Update the cart
    if (this.quantity > this.quantityInCart) {
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.quantityInCart += itemsToAdd;
      this.cartService.addItemToCart(this.product, itemsToAdd);
    } else {
      const itemsToRemove = this.quantityInCart - this.quantity;
      this.quantityInCart -= itemsToRemove;
      this.cartService.removeItemFromCart(this.product.id, itemsToRemove);
    }
  }

  // Updates the quantity in cart state
  updateQuantityInCart() {
    // Gets the quantity of the product that is in the cart
    this.quantityInCart =
      this.cartService
        .cart()
        ?.items.find((x) => x.productId === this.product?.id)?.quantity || 0;

    // Sets the product quantity
    this.quantity = this.quantityInCart || 1;
  }

  // Creates the text of the button, depending on the quantity in cart of the product
  getButtonText() {
    return this.quantityInCart > 0 ? 'Update cart' : 'Add to cart';
  }
}
