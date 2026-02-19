import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LkCiclos } from '../Models/LkCiclos';

@Injectable({
  providedIn: 'root'
})
export class CiclosService {

  private apiUrl = 'https://localhost:7257/api/v1/Ciclos'; // URL de tu API

  constructor(private http: HttpClient) { }

  getCiclos(): Observable<LkCiclos[]> {
    return this.http.get<LkCiclos[]>(this.apiUrl);
  }
}
