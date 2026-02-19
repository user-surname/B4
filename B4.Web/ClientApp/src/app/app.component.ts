import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `<h1>Bienvenido a Ciclos</h1>
             <app-ciclos></app-ciclos>`,
  standalone: true,
  imports: [
    CiclosComponent  // importas tu componente standalone aquí
  ]
})
export class AppComponent { }
