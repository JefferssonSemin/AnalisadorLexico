import { Component } from '@angular/core';
import { ApiService } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  str = '';
  mensagem: string;

  constructor(private apiService: ApiService) { }

  enviaValores(value: string): void {
    this.apiService.consulta(value).subscribe((data => this.mensagem = data.toString()));
  }
}
