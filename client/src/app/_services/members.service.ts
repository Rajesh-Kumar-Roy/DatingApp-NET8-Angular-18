import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getMembers() : Observable<Member[]>{
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username: string) : Observable<Member> {
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member);
  }

}
