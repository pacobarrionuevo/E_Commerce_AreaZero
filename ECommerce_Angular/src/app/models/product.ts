export class Product {
    id: number;
    nombre: string;
    ruta: string;

    constructor(id: number, nombre: string, ruta: string) {
        this.id = id;
        this.nombre = nombre;
        this.ruta = ruta;
    }
}