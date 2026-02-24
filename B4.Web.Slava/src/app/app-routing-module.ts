import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Data } from './pages/data/data';
import { LkComponent } from './pages/lk/lk';
import { Home } from './pages/home/home';

const routes: Routes = [
    {
    path: "", 
    component: Home
  },
  {
    path: "data",
    component: Data
  },
    {
    path: "lk",
    component: LkComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
