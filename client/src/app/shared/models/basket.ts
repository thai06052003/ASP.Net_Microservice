export interface IBasketItem {
  quantity: number;
  imageFile: string;
  price: number;
  productId: string;
  productName: string;
}

export interface IBasket {
  userName: string;
  items: IBasketItem[];
  totalPrice: number;
}

export class Basket implements IBasket {
  userName: string = 'thai';
  totalPrice: number = 0;
  items: IBasketItem[] = [];
}

export interface IBasketTotal{
  total: number;
}
