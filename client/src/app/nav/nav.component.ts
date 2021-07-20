import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  //currentUser$: Observable<User> | undefined;
  //we use the pipe in async mode to automaticly unsubscribe if a nav.component.ts is no longer visible
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currentUser$;
  }

  login():void {
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);
      
    },error =>{
      console.log(error);
    })
  }
  logout(){
    this.accountService.logout();
  }

  /*getCurrentUser(){
    this.accountService.currentUser$.subscribe(user =>{
      // !! turn an object into a value
      this.loggedIn = !!user;
    },error =>{
      console.log(error);
    })
  }*/

}
