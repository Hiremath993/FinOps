import { Injectable, reflectComponentType } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';


//import{GetAllEmployeeResponse} from '../Models/api-models/getallstudentresponse.models'

@Injectable({
  providedIn: 'root',
})
export class loginService {

  constructor(private httpClient: HttpClient) {}
  currentUser: BehaviorSubject<any> = new BehaviorSubject(null);

  baseApiUrl = 'https://localhost:7170/api/';

  //constructor(private httpClient: HttpClient) {}


  SignUpUser(user: any) {
    debugger;
    return this.httpClient.post(
      this.baseApiUrl + 'Login/SignUp',
      {

        Name: user[0],
        EmailId: user[1],
        Password: user[2],
      },
      {
        responseType: 'text',
      }
    );
  }

  login(model: any){
     debugger;
     return this.httpClient.post (this.baseApiUrl + 'Login/LoginAsync',model);
  }

 loginUser(loginInfo:Array<String>){
 debugger;
    return this.httpClient.post(

      this.baseApiUrl + 'Login/LoginAsync',

      {
         EmailId: loginInfo[0],
         Password: loginInfo[1]

       },
      {
         responseType: 'text',
       }

      );
 }

sendEmail(email:any){
  return this.httpClient.post<any>(
    this.baseApiUrl + 'Email', email
  )
}
}
