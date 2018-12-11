import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private url = 'http://localhost:2745/api/Default/VerificarTexto';

  constructor(private http: HttpClient) { }

  consulta(value: string) {
    return this.http.post(this.url , {Texto : value});
  }
}
