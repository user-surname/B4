import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DataActualsService } from './data-actuals.service';
import { DataActualsGetDto } from './data-actuals.model';
import { TokenService } from '../../core/auth/token.service';

@Component({
  selector: 'app-data-actuals-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './data-actuals-page.component.html'
})
export class DataActualsPageComponent {
  // Auth
  token = '';

  // Filtros GET
  planta = 1;
  ejercicio = 2025;
  epigrafe = 100;

  loading = false;
  error = '';
  dto: DataActualsGetDto | null = null;

  constructor(
    private api: DataActualsService,
    private tokens: TokenService
  ) {}

  saveToken() {
    this.tokens.set(this.token.trim());
  }

  clearToken() {
    this.tokens.clear();
    this.token = '';
  }

  fetch() {
    this.loading = true;
    this.error = '';
    this.dto = null;

    this.api.get(this.planta, this.ejercicio, this.epigrafe).subscribe({
      next: (data) => { this.dto = data; this.loading = false; },
      error: (err) => {
        this.loading = false;
        // si backend devuelve msg dentro del wrapper, a veces vendrá en err.error.msg
        this.error = err?.error?.msg || err?.error?.data?.raw || 'Error llamando a la API';
      }
    });
  }

  monthsHeaders(): string[] {
    // Tu API manda detail.v como array (no sabemos si 12 o 13, así que lo sacamos dinámico)
    const n = this.dto?.detail?.[0]?.v?.length ?? 0;
    return Array.from({ length: n }, (_, i) => `M${String(i + 1).padStart(2, '0')}`);
  }
}