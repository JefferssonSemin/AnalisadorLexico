import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private url = 'http://';

  constructor(private http: HttpClient) { }

  ApiService(value: string) {
    return this.http.post(this.url , value);
  }
}
