import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {Member} from '../_models/member';

const httpOptions = {
  headers: new HttpHeaders({
    //The ! operator behind is for checking that the string is not null, and the ? mark to take token if it has
    Authorization: 'Bearer '+ JSON.parse(localStorage.getItem('user')!)?.token
    
  })
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMembers(){
    return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
  }

  getMember(username : string){
    return this.http.get<Member>(this.baseUrl + 'users/'+username,httpOptions)
  }
}
