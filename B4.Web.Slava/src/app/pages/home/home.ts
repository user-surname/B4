import { HeaderService } from '../../core/services/header.service';
import { Router } from '@angular/router';
import { LoginRequest } from '../../core/interfaces/auth.interface';
import { AuthorizationService } from '../../core/services/authorization.service.ts';
import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrls: ['./home.css'],
})
export class Home implements OnInit {
  headerService = inject(HeaderService);

  credentials: LoginRequest = { email: '', password: '' };
  message: string | null = null;
  loading = false;

  constructor(
    private authService: AuthorizationService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.headerService.titulo.set('Home');
  }

  onSubmit() {
    this.loading = true;
    this.message = null;

    this.authService.login(this.credentials).subscribe({
      next: res => {
        this.loading = false;

        if (res.success && res.token) {
          // Guardar token
          localStorage.setItem('jwt', res.token);
          this.message = 'Autentificacion exitosa';

        } else {
          this.message = `Error de autenticación: ${res.msg}`;
        }

        setTimeout(() => this.cdr.detectChanges());
      },
      error: err => {
        this.message = 'Error en la conexión con el servidor';
        this.loading = false;
        setTimeout(() => this.cdr.detectChanges());
      }
    });
  }
}