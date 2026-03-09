import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponseList, ApiResponseSingle } from '../interfaces/api-response.interface';
import { Ciclo, CreateCicloRequest } from '../interfaces/Ciclo';

@Injectable({
  providedIn: 'root'
})
export class CiclosService {

  private baseUrl = 'http://localhost:5029/api/v1/Ciclos';

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getAll(): Observable<ApiResponseList<Ciclo>> {
    return this.http.get<ApiResponseList<Ciclo>>(this.baseUrl, {
      headers: this.getHeaders()
    });
  }

  getById(id: number): Observable<ApiResponseSingle<Ciclo>> {
    return this.http.get<ApiResponseSingle<Ciclo>>(`${this.baseUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  create(ciclo: CreateCicloRequest): Observable<any> {
    return this.http.post(this.baseUrl, ciclo, {
      headers: this.getHeaders()
    });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }
}