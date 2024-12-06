import { ProductoCarrito } from "./producto-carrito";


export class Carrito {
  id: number;
  userId: number;
  productoCarrito: ProductoCarrito[];

  constructor(id: number, userId: number, productoCarrito: ProductoCarrito[]) {
    this.id = id;
    this.userId = userId;
    this.productoCarrito = productoCarrito;
  }
}