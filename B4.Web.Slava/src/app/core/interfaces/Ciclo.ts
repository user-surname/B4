export interface Ciclo {
    createdAt: Date,
    updatedAt: Date,
    isActive: number,
    idCiclo: number,
    ciclo: String,
    descripcion: String,
}

export interface CreateCicloRequest {
  ciclo: string;
  descripcion: string;
}