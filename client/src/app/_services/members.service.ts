import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AccountService } from './account.service';
import { Observable } from 'rxjs';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountServie = inject(AccountService);
  baseUrl = environment.apiUrl;

  getMembers() : Observable<Member[]>{
    return this.http.get<Member[]>(this.baseUrl + 'users', this.getHttpOptions());
  }

  getMember(username: string) : Observable<Member> {
    return this.http.get<Member>(this.baseUrl + 'users/' + username, this.getHttpOptions());
  }


  getHttpOptions() {
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${this.accountServie.currentUser()?.token}`  
      })
    }
  }
}
