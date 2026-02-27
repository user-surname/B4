import { Routes } from '@angular/router';
import { DataActualsPageComponent } from './features/data-actuals/data-actuals-page.component';

export const routes: Routes = [
  { path: '', redirectTo: 'data-actuals', pathMatch: 'full' },
  { path: 'data-actuals', component: DataActualsPageComponent }
];