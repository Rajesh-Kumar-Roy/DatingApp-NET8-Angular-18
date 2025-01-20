import { Component, EventEmitter, inject, input, Input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

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

  reigster() {
    this.accountService.register(this.model).subscribe({
      next: response=>{
        console.log(response);
        this.cancel();
      },error: error =>{
        console.log(error);
      }
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
