import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  // API URL
  baseUrl = environment.apiUrl;

  // Http client
  private http = inject(HttpClient);

  // Cart signal
  cart = signal<Cart | null>(null);
  // Gets the number of items in the cart
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  });
  selectedDelivery = signal<DeliveryMethod | null>(null);
  // Gets the total price of the items in the cart
  totals = computed(() => {
    // Gets cart
    const cart = this.cart();
    const delivery = this.selectedDelivery();

    // If cart doesn't exist return null
    if (!cart) return null;

    // Else compute the subtotal
    const subtotal = cart.items.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );

    const shipping = delivery ? delivery.price : 0;
    const discount = 0;

    return {
      subtotal,
      shipping,
      discount,
      total: subtotal + shipping - discount,
    };
  });

  // Gets the cart from the redis database
  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map((cart) => {
        this.cart.set(cart);
        return cart;
      })
    );
  }

  // Saves the contents of the cart in the redis database
  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: (cart) => this.cart.set(cart),
    });
  }

  // Adds a item to the cart
  addItemToCart(item: CartItem | Product, quantity = 1) {
    // Checks if cart exists
    const cart = this.cart() ?? this.createCart();

    // If the item is a Product, it converts it to a CartItem
    if (this.isProduct(item)) {
      item = this.mapProductToCartItem(item);
    }

    // Adds or updates the given items
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);

    // Updates the cart
    this.setCart(cart);
  }

  // Removes an item from the cart
  removeItemFromCart(productId: number, quantity = 1) {
    // Gets the cart
    const cart = this.cart();

    // If cart is empty, return
    if (!cart) return;

    // Find the index of the product that matches the given product id
    const index = cart.items.findIndex((x) => x.productId === productId);

    // If product exists within the cart
    if (index !== -1) {
      // If the quantity of the product to be removed is less than the one that is in the cart
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity -= quantity;
      }
      // Else delete product instance from the cart
      else {
        cart.items.splice(index, 1);
      }
      // If cart is empty after deletion delete it
      if (cart.items.length === 0) {
        this.deleteCart();
      }
      // Else update it
      else {
        this.setCart(cart);
      }
    }
  }

  // Deletes the cart in the redis database and local storage
  deleteCart() {
    this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      },
    });
  }

  // Checks if the item needs to be added or updated
  private addOrUpdateItem(
    items: CartItem[],
    item: CartItem,
    quantity: number
  ): CartItem[] {
    // Gets the index of the given item
    const index = items.findIndex((x) => x.productId === item.productId);

    // If it wasn't found, add it to the list
    if (index === -1) {
      item.quantity = quantity;
      items.push(item);
    }
    // If it was found update it
    else {
      items[index].quantity += quantity;
    }

    // Return the cart items
    return items;
  }

  // Transforms Product into CartItem
  private mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type,
    };
  }

  // Checks if item is a product or not
  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }

  // Creates a new cart
  private createCart(): Cart {
    const cart = new Cart();

    // Saves the cart id to local storage
    localStorage.setItem('cart_id', cart.id);

    return cart;
  }
}
