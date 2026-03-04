import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<any>('/api/v1/Auth/login', { email, password }).pipe(
      map(res => res?.data?.token ?? res?.token) // por si un día quitáis el wrapper
    );
  }
}