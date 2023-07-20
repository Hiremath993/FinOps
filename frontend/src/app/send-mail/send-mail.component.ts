import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators} from '@angular/forms';
import { FloatLabelType } from '@angular/material/form-field';
import { Router } from '@angular/router';
import { loginService } from '../service/login.service';

@Component({
  selector: 'app-send-mail',
  templateUrl: './send-mail.component.html',
  styleUrls: ['./send-mail.component.css']
})
export class SendMailComponent implements OnInit {

  responsedata: any;
  Isloggedin = false;
  isUserValid = false;


  //isUserValid: boolean | undefined;
  hide = true;
  floatLabelControl = new FormControl('auto' as FloatLabelType);

  constructor(
    private loginservice: loginService,
    private _router: Router
    // private emailservice: ResetPasswordService
  ) { }

  ngOnInit(): void { }

  loginForm:FormGroup = new FormGroup({
      emailId: new FormControl('', [Validators.required,Validators.email]),
       password: new FormControl('', [
         Validators.required,
        // Validators.minLength(6),
         //Validators.maxLength(15),


        ])

  });


  sendEmail() {
    this.loginservice.sendEmail(this.loginForm)
    .subscribe ({
      next: (resource) => {
        this._router.navigate(['resource']);
      }

    });
    }


  get EmailId(): FormControl {
    return this.loginForm.get('emailId') as FormControl;
  }

}


