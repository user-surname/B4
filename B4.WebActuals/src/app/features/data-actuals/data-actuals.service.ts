import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class DataActualsService {
  private base = '/api/v1/DataActuals';

  constructor(private http: HttpClient) {}

  getRaw(planta: number, ejercicio: number, epigrafe: number) {
    return this.http.get<any>(`${this.base}/${planta}/${ejercicio}/${epigrafe}`);
  }

  get(planta: number, ejercicio: number, epigrafe: number) {
    return this.http.get<any>(`${this.base}/${planta}/${ejercicio}/${epigrafe}`).pipe(
      map(res => res?.data ?? res)
    );
  }

  create(planta: number, ejercicio: number, mes: number, tipo: string, body: any[]) {
    return this.http.post<any>(`${this.base}/${planta}/${ejercicio}/${mes}/${tipo}`, body).pipe(
      map(res => res?.data ?? res)
    );
  }
}
