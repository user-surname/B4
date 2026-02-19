import { appConfig } from './app/app.config';
import { App } from './app/app';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
import { bootstrapApplication } from '@angular/platform-browser';
import { CiclosComponent } from './app/Components/ciclos.component';

bootstrapApplication(CiclosComponent)
  .catch(err => console.error(err));
