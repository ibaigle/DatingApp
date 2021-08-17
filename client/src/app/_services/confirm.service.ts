import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModelRef!: BsModalRef;

  constructor(private modalService: BsModalService) { }

  confirm(
      title='Confirmation', 
      message = 'Are you sure want to do this?', 
      btnOkText = 'Ok',
      btnCancelText='Cancel') : Observable<boolean>{
        const config = {
          initialState:{
            title,
            message,
            btnOkText,
            btnCancelText
          }
        }
        this.bsModelRef = this.modalService.show('confirm', config);
        
        return new Observable<boolean>(this.getResult());
  }
  private getResult(){
    return (observer:any) =>{
      const subscription = this.bsModelRef.onHidden.subscribe(()=>{
        observer.next(this.bsModelRef.content.result);
        observer.complete();
      })
      return {
        unsubscribe(){
          subscription.unsubscribe();
        }
      }
    }
  }
}
