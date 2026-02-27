// auth.interface.ts
export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  coderror: number;
  action: string;
  msg: string;
  ts: string;
  exectimems: number;
  count: number;
  data: {
    token: string;
  };
}