import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { LkCiclos } from '../Models/LkCiclos';
import { CiclosService } from '../Services/ciclos.service';

@Component({
  selector: 'app-ciclos',
  standalone: true,   // ← clave: este componente es independiente
  imports: [CommonModule, HttpClientModule], // módulos necesarios
  templateUrl: './ciclos.component.html',
  styleUrls: ['./ciclos.component.css']
})
export class CiclosComponent implements OnInit {

  ciclos: LkCiclos[] = [];

  constructor(private ciclosService: CiclosService) { }

  ngOnInit(): void {
    this.ciclosService.getCiclos().subscribe(
      data => this.ciclos = data,
      err => console.error(err)
    );
  }

}
