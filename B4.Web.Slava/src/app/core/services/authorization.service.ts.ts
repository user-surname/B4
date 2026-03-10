import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { LoginRequest, LoginResponse } from '../interfaces/auth.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationService {
  private apiUrl = 'http://localhost:5029/api/v1/Auth';

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<{ success: boolean; token?: string; msg?: string }> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        map(response => {
          if (response.coderror === 0 && response.data?.token) {
            return { success: true, token: response.data.token };
          } else {
            return { success: false, msg: response.msg || 'Error desconocido' };
          }
        })
      );
  }
}
