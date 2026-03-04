import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router'

@Component({
  selector: 'app-tabs',
  standalone: false,
  templateUrl: './tabs.html',
  styleUrl: './tabs.css',
})
export class Tabs {

  constructor( private router: Router) {
    this.router.events.subscribe(event => {
      if(event instanceof NavigationEnd){
        console.log("EVENTO",event)
        switch (event.urlAfterRedirects){
          case "/":
            this.seleccionado = [true, false, false]
            break;
          case "/data":
            this.seleccionado = [false, true, false]
            break;
          case "/lk":
            this.seleccionado = [false, false, true]
            break;
          default:
            this.seleccionado = [false, false, false]
            break;
          }
      }
    })
}
  seleccionado = [false,false,false]

  navegar(direccion:string){
    this.router.navigate([direccion])
  }

}
