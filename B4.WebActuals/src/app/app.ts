import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { DataActualsService } from './features/data-actuals/data-actuals.service';
import { AuthService } from './core/auth/auth.service';

interface DataActualsGetDto {
  head: any;
  detail: any[];
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet],
  templateUrl: './app.html'
})
export class App {

  // ======================
  // LOGIN
  // ======================
  email = 'admin@ejemplo.com';
  password = '1234';
  loginMsg = '';

  // ======================
  // TOKEN MANUAL (opcional)
  // ======================
  token = '';
  storedToken = !!localStorage.getItem('b4_token');

  // ======================
  // GET
  // ======================
  planta = 10;
  ejercicio = 2024;
  epigrafe = 100;

  dto: DataActualsGetDto | null = null;
  rawGetResponse: any = null;
  rawGetJson = '';

  // ======================
  // POST
  // ======================
  mes = 1;
  tipo = 'EUR';

  postJson = `[
  {
    "e": 100,
    "m1": 0, "m2": 1, "m3": 2, "m4": 3, "m5": 4, "m6": 5,
    "m7": 6, "m8": 7, "m9": 8, "m10": 9, "m11": 10, "m12": 11, "m13": 12
  }
]`;

  inserted: number | null = null;

  // ======================
  // UI STATE
  // ======================
  loading = false;
  error = '';

  constructor(
    private api: DataActualsService,
    private auth: AuthService,
    private cd: ChangeDetectorRef
  ) {}

  // ======================
  // LOGIN
  // ======================
  async doLogin() {
    this.loading = true;
    this.error = '';
    this.loginMsg = '';
    this.cd.detectChanges();

    try {
      const token = await firstValueFrom(
        this.auth.login(this.email, this.password)
      );

      if (!token) throw new Error('No se recibió token');

      localStorage.setItem('b4_token', token);
      this.storedToken = true;
      this.token = token;

      this.loginMsg = 'Login OK ✅';
    } catch (e: any) {
      this.loginMsg = '';
      this.error =
        e?.error?.msg ||
        e?.message ||
        'Error en login';
    } finally {
      this.loading = false;
      this.cd.detectChanges();
    }
  }

  logout() {
    localStorage.removeItem('b4_token');
    this.token = '';
    this.storedToken = false;
  }

  // ======================
  // TOKEN MANUAL
  // ======================
  saveToken() {
    const t = this.token.trim();
    if (!t) return;
    localStorage.setItem('b4_token', t);
    this.storedToken = true;
  }

  clearToken() {
    localStorage.removeItem('b4_token');
    this.token = '';
    this.storedToken = false;
  }

  // ======================
  // GET
  // ======================
  get monthHeaders(): number[] {
    const n = this.dto?.detail?.[0]?.v?.length ?? 0;
    return Array.from({ length: n }, (_, i) => i + 1);
  }

  async fetch() {
    this.loading = true;
    this.error = '';
    this.dto = null;
    this.rawGetResponse = null;
    this.rawGetJson = '';
    this.cd.detectChanges();

    try {
      const raw = await firstValueFrom(
        this.api.getRaw(this.planta, this.ejercicio, this.epigrafe)
      );

      this.rawGetResponse = raw;
      this.rawGetJson = JSON.stringify(raw, null, 2);
      this.dto = (raw?.data ?? raw) as DataActualsGetDto;
    } catch (e: any) {
      this.error =
        e?.error?.msg ||
        e?.error?.data?.title ||
        'Error llamando a la API';
    } finally {
      this.loading = false;
      this.cd.detectChanges();
    }
  }

  // ======================
  // POST
  // ======================
  async create() {
    this.loading = true;
    this.error = '';
    this.inserted = null;
    this.cd.detectChanges();

    try {
      const body = JSON.parse(this.postJson);

      const r = await firstValueFrom(
        this.api.create(
          this.planta,
          this.ejercicio,
          this.mes,
          this.tipo,
          body
        )
      );

      this.inserted = r?.inserted ?? null;

    } catch (e: any) {
      this.error =
        e?.error?.msg ||
        e?.message ||
        'Error creando (POST). Revisa el JSON y/o la API.';
    } finally {
      this.loading = false;
      this.cd.detectChanges();
    }
  }
}
