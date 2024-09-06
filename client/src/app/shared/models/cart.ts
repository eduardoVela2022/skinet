// Imports
import { nanoid } from 'nanoid';

// Cart type
export type CartType = {
  id: string;
  items: CartItem[];
  deliveryMethod?: number;
  paymentIntentId?: string;
  clientSecret?: string;
};

// Cart item type
export type CartItem = {
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  brand: string;
  type: string;
};

// Cart class
export class Cart implements CartType {
  id = nanoid();
  items: CartItem[] = [];
  deliveryMethod?: number;
  paymentIntentId?: string;
  clientSecret?: string;
}
