import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NetworkService } from '@app/_services/network.service';
import { UserNetworkDto } from '@app/_models/UserNetworkDto';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Subscription } from 'rxjs';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';

@Component({
  selector: 'app-network',
  templateUrl: './network.component.html',
  styleUrls: ['./network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkComponent implements OnInit {
  network: UserNetworkDto;
  subscription: Subscription;
  tabstatus = {};
  active;
  
  constructor(private networkService: NetworkService
    , private toast: ToastrService) { }

    ngOnInit() {
      this.active = 1;
      this.tabstatus = {};
      
      this.getUserNetwork();
      this.subscription = this.networkService.networkChanged$
      .subscribe(_ => {
        this.onNetworkChanged();
      });
    }
  
    onNetworkChanged() {
      this.getUserNetwork();
    }
  
    getUserNetwork(): void {
      this.networkService.getUserNetwork()
        .subscribe(
          (data: UserNetworkDto) => {
            this.network = data;
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
          });
    }

    // This is repeated in several places.  Factor into its own component...
    followOrUnfollow(element, user: NetworkUserViewModel): void {
      const action = element.innerText;
  
      if (action === 'Follow') {
        this.networkService.postFollowUser(user)
          .subscribe(
            (data: NetworkUserViewModel) => {
              this.toast.info('You are now following ' + data.userName, 'Success');
            },
            (error: ErrorReportViewModel) => {
              this.toast.error(error.serverCustomMessage, 'An error occurred');
            });
        return;
      } else {
        this.networkService.postUnfollowUser(user)
          .subscribe(
            (data: NetworkUserViewModel) => { // _______________________
              this.toast.info('You have unfollowed ' + data.userName, 'Success');
            },
            (error: ErrorReportViewModel) => {
              this.toast.error(error.serverCustomMessage, 'An error occurred');
            });
        return;
      }
    }

}
