import { Product } from './product';

export class ProductoCarrito {
  productoId: number;
  carritoId: number;
  cantidad: number;
  producto: Product; // Incluimos el producto completo

  constructor(productoId: number, carritoId: number, cantidad: number, producto: Product) {
    this.productoId = productoId;
    this.carritoId = carritoId;
    this.cantidad = cantidad;
    this.producto = producto; // Asignamos el producto completo
  }
}
