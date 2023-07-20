import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { subscription } from 'src/app/Models/subscription';
import { SubscriptionsService } from 'src/app/service/subscriptions.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-subscription',
  templateUrl: './subscription.component.html',
  styleUrls: ['./subscription.component.css']
})

export class SubscriptionComponent {
  subscriptionData: subscription = {
    clientId: '',
    tenantId: '',
    clientSecret: '',
    subscriptionId: ''
  };
  verificationStatus: string = '';

  constructor(private SubscriptionService: SubscriptionsService, private router: Router) { }

  verifySubcription() {
    this.SubscriptionService.getAllSubscription(this.subscriptionData).subscribe(
      subscriptionExists => {
        if (subscriptionExists) {
          this.verificationStatus = 'Subscription exists';
          //this.router.navigate(['/home']);
        } else {
          this.verificationStatus = 'Invalid subscription';
        }
      },
      error => {
        console.log(error);
        this.verificationStatus = 'Error occurred while verifying subscription';
      }
    );
  }

}
