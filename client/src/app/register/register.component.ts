import { Component, EventEmitter, inject, input, Input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
 //usersFormHomeComponent = input.required<any>(); //input signal
 cancelRegister = output<boolean>(); //output signal
  model: any = {};
  private toastrService = inject(ToastrService);

  reigster() {
    this.accountService.register(this.model).subscribe({
      next: response=>{
        console.log(response);
        this.cancel();
      },error: error =>{
       this.toastrService.error(error.error); 
      }
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
