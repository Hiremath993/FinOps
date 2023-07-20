import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {

  baseApiUrl = 'https://localhost:7170/api/';
   constructor(private http:HttpClient) { }
   SendforGotPasswordEmail(resetemail:any){
     return this.http.post<any>(this.baseApiUrl+'api/Auth/ForgotPassword',
     {email:resetemail}); } resetpassword(resetemail:any,newpassword:any){
      return this.http.put<any>(
        this.baseApiUrl+'api/Auth/reset-email',{
          email:resetemail,newPassword:newpassword
        }); }
}
