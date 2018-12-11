import { Component } from '@angular/core';
import { ApiService } from './api.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  str = 'digite aqui...';
  ApiService: ApiService;
  retorno: string;

  enviaValores(value: string): void {
    console.log(value.toString);
    this.ApiService.ApiService(value).subscribe(data =>  this.retorno = data.toString());
  }
}
