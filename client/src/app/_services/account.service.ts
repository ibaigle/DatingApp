import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  //this is a buffer object that store how many values inside as we wanted
  private currentUserSource= new ReplaySubject<User>(1);
  //to see this as an observable we use the $ sign
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){
    //let response !: User ;
    return this.http.post(this.baseUrl + 'account/login',model).pipe(
      //response should be of type 'User', but in this case wasnt able 
      map((response: any) => {
        const user : User = response;
        if(user){
          this.setCurrentUser(user);
          /* this.currentUserSource.next(user); */
        }
      })
    )
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: any) =>{
        if (user){
          this.setCurrentUser(user);
          /* this.currentUserSource.next(user); */
        }
        //return user; ##just was an example to watch at the console
      })
    )
  }

  setCurrentUser(user: User){
    user.roles=[];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles=roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next();
  }
  getDecodedToken(token: any){
    return JSON.parse(atob(token.split('.')[1]));
  }
}
