import { Product } from './product';

export class ProductoCarrito {
  productoId: number;
  carritoId: number;
  cantidad: number;
<<<<<<< HEAD
  producto: Product;
  id: any;
=======
  producto: Product; // Incluimos el producto completo
>>>>>>> origin/salperro2

  constructor(productoId: number, carritoId: number, cantidad: number, producto: Product) {
    this.productoId = productoId;
    this.carritoId = carritoId;
    this.cantidad = cantidad;
    this.producto = producto; // Asignamos el producto completo
  }
}
