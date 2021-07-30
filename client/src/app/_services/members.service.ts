import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {Member} from '../_models/member';
import { Photo } from '../_models/photo';

/* const httpOptions = {
  headers: new HttpHeaders({
    //The ! operator behind is for checking that the string is not null, and the ? mark to take token if it has
    Authorization: 'Bearer '+ JSON.parse(localStorage.getItem('user')!)?.token
    
  })
} */

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers(){
    if(this.members.length > 0) return of(this.members); 
    return this.http.get<Member[]>(this.baseUrl + 'users' /* httpOptions */).pipe(
      map(members =>{
          this.members = members;
          return members;
        })
    );
  }

  getMember(username : string){
    const member = this.members.find(x => x.username === username);
    if(member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/'+username,/* httpOptions */)
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(()=> {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo' + photoId);
  }
}