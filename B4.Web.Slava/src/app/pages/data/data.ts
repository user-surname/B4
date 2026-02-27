import { Component, inject } from '@angular/core';
import { HeaderService } from '../../core/services/header.service';

@Component({
  selector: 'app-data',
  standalone: false,
  templateUrl: './data.html',
  styleUrl: './data.css',
})
export class Data {
  headerService = inject(HeaderService)

  ngOnInit(): void {
  this.headerService.titulo.set( "Data" ); 
}
}
