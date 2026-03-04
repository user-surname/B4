import { Ciclo } from "./Ciclo";

export interface ApiResponse {
  coderror: number;
  action: string;
  msg: string;
  ts: string;
  exectimems: number;
  count: number;
  data: Ciclo;   
}