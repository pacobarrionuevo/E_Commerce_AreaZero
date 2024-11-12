export class Product {
    id: number;
    nombre: string;  
    ruta: string;
    precio: number;

    constructor(id: number, nombre: string, ruta: string, precio: number) {
        this.id = id;
        this.nombre = nombre;
        this.ruta = ruta;
        this.precio = precio;
    }
}