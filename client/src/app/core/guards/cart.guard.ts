import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { CartService } from '../services/cart.service';
import { SnackbarService } from '../services/snackbar.service';
import { of } from 'rxjs';

// Protects the routes that require a cart to be accessed
export const cartGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const snackbarService = inject(SnackbarService);

  // If the cart isn't empty, let the user pass
  if (cartService.cart() !== null) {
    return true;
  }
  // Else display a snackbar error
  else {
    snackbarService.error('Your cart is empty!');
    return false;
  }
};
