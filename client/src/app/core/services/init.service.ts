import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, of } from 'rxjs';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
// Service to init the app
export class InitService {
  // Waits until the cart is obtained from the local storage
  private cartService = inject(CartService);
  // Persists the login state of the user
  private accountService = inject(AccountService);

  init() {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);

    // Waits until all the http request are completed
    return forkJoin({
      cart: cart$,
      user: this.accountService.getUserInfo(),
    });
  }
}
