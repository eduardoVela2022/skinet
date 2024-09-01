// Imports
import { nanoid } from 'nanoid';

// Cart type
export type CartType = {
  id: string;
  items: CartItem[];
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
}
