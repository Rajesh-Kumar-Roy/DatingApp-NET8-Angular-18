import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.css'
})
export class TestErrorComponent {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  validationErrors: string[] = [];

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe(
      {
        next: (response: any) => console.log(response),
        error: (error: any) => console.log(error),
      } );
  }
  get401Error() {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe(
      {
        next: (response: any) => console.log(response),
        error: (error: any) => console.log(error),
      } );
  }
  get404Error() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe(
      {
        next: (response: any) => console.log(response),
        error: (error: any) => console.log(error),
      } );
  }
  get500Error() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe(
      {
        next: (response: any) => console.log(response),
        error: (error: any) => console.log(error),
      } );
  }
  get400ValidationError() {
    this.http.post(this.baseUrl + 'account/register',{}).subscribe(
      {
        next: (response: any) => console.log(response),
        error: (error: any) => {
          console.log(error);
          this.validationErrors = error
        },
      } );
  }
}
