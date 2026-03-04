export interface ApiWrapper<T> {
  coderror: number;
  action: string;
  msg: string;
  ts: string;
  exectimems: number;
  count: number;
  data: T;
}