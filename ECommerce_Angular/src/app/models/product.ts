export class Product {
  id: number;
  nombre: string;
  ruta: string; 
  precio: number;
  stock: number;
  descripcion: string;
  
  constructor(id: number, nombre: string, ruta: string, precio: number, stock: number,descripcion: string ) {
    this.id = id;
    this.nombre = nombre;
    this.ruta = ruta; 
    this.precio = precio;
    this.stock = stock;
    this.descripcion = descripcion;
  }
}
