export interface ApiResponseList<T> {
  coderror: number;
  action: string;
  msg: string;
  ts: string;
  exectimems: number;
  count: number;
  data: T[];
}

export interface ApiResponseSingle<T> {
  coderror: number;
  action: string;
  msg: string;
  ts: string;
  exectimems: number;
  count: number;
  data: T;
}