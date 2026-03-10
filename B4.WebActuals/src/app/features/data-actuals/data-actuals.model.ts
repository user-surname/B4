export interface DataActualsGetDto {
  head: {
    ts: string;     // Timestamp
    p: string;      // Planta
    e: number;      // Ejercicio
    c: number;      // IdCiclo
    f: string;      // IdFase
    m: string;      // Moneda
  };
  detail: Array<{
    e: number;        // IdEpigrafe
    v: number[];      // valores (array de meses, etc.)
  }>;
}

export interface DataActualsPostDto {
  e: number;
  m1: number;  m2: number;  m3: number;  m4: number;
  m5: number;  m6: number;  m7: number;  m8: number;
  m9: number;  m10: number; m11: number; m12: number;
  m13: number;
}

export interface InsertedResponse {
  inserted: number;
}