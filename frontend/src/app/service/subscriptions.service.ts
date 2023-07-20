import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { subscription } from '../Models/subscription';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionsService {

  private apiUrl = 'https://localhost:7170/api/Subscription';

  constructor(private http: HttpClient) { }

  getAllSubscription(data: subscription): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}?clientId=${data.clientId}&tenantId=${data.tenantId}&clientSecret=${data.clientSecret}&subscriptionId=${data.subscriptionId}`);
  }


  addSubscription(resource: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, resource);
  }
}
