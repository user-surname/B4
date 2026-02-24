import { Component, inject, OnInit } from '@angular/core';
import { Ciclo, CreateCicloRequest } from '../../core/interfaces/Ciclo';
import { CiclosService } from '../../core/services/ciclos.service';
import { HeaderService } from '../../core/services/header.service';

@Component({
  standalone: false,
  selector: 'app-lk',
  templateUrl: './lk.html',
  styleUrls: ['./lk.css']
})
export class LkComponent implements OnInit {

  headerService = inject(HeaderService)
  ciclos: Ciclo[] = [];
  cicloSeleccionado?: Ciclo;
  mensaje: string = '';
  error: string = '';

  nuevoCiclo: CreateCicloRequest = {
    ciclo: '',
    descripcion: ''
  };

  idBusqueda: number = 0;
  idEliminar: number = 0;

  constructor(private ciclosService: CiclosService) { }

  ngOnInit(): void {
    this.headerService.titulo.set( "LK" ); 
    this.getAll();
  }

  getAll(): void {
    this.ciclosService.getAll().subscribe({
      next: res => {
        this.ciclos = res.data;
      },
      error: err => {
        this.error = 'Error al obtener los ciclos';
      }
    });
  }

  getById(): void {
    this.ciclosService.getById(this.idBusqueda).subscribe({
      next: res => {
        this.cicloSeleccionado = res.data;
      },
      error: () => {
        this.error = 'No se encontró el ciclo';
      }
    });
  }

  create(): void {
    this.ciclosService.create(this.nuevoCiclo).subscribe({
      next: () => {
        this.mensaje = 'Ciclo creado correctamente';
        this.getAll();
      },
      error: () => {
        this.error = 'Error al crear el ciclo';
      }
    });
  }

  delete(): void {
    this.ciclosService.delete(this.idEliminar).subscribe({
      next: () => {
        this.mensaje = 'Ciclo eliminado correctamente';
        this.getAll();
      },
      error: () => {
        this.error = 'Error al eliminar el ciclo';
      }
    });
  }
}