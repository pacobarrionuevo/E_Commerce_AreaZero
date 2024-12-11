import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-admin',
    imports: [CommonModule, FormsModule],
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  usuarios: any[] = [];
  productos: any[] = [];
  nuevoProducto = {
    nombre: '',
    ruta: '',
    descripcion: '',
    precio: 0,
    stock: 0
  };

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsuarios();
    this.loadProductos();
  }

  loadUsuarios(): void {
    this.adminService.getUsuarios().subscribe(usuarios => {
      this.usuarios = usuarios;
    });
  }

  loadProductos(): void {
    this.adminService.getProductos().subscribe(productos => {
      this.productos = productos;
    });
  }

  updateUsuario(usuario: any): void {
    this.adminService.updateUsuario(usuario.id, usuario).subscribe(() => {
      this.loadUsuarios();
    });
  }

  deleteUsuario(userId: number): void {
    const confirmation = confirm('¿Estás seguro de que deseas eliminar este usuario?');
    if (confirmation) {
      this.adminService.deleteUsuario(userId).subscribe(() => {
        this.loadUsuarios();
      });
    }
  }

  createProducto(): void {
    this.adminService.createProducto(this.nuevoProducto).subscribe(() => {
      this.loadProductos();
      this.nuevoProducto = {
        nombre: '',
        ruta: '',
        descripcion: '',
        precio: 0,
        stock: 0
      };
    });
  }

  updateProducto(producto: any): void {
    this.adminService.updateProducto(producto.id, producto).subscribe(() => {
      this.loadProductos();
    });
  }

  deleteProducto(productId: number): void {
    const confirmation = confirm('¿Estás seguro de que deseas eliminar este producto?');
    if (confirmation) {
      this.adminService.deleteProducto(productId).subscribe(() => {
        this.loadProductos();
      });
    }
  }
}
