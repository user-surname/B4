import { Injectable, signal, WritableSignal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  
  constructor() { }

  titulo = signal("Titulo");

}
