import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NetworkUserViewModel } from '../../_models/UserProfileViewModel';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { Subscription } from 'rxjs';
import { UserNetworkDto } from '../../_models/UserNetworkDto';
import { NetworkService } from '../../_services/network.service';

@Component({
  selector: 'app-info-network',
  templateUrl: './info-network.component.html',
  styleUrls: ['./info-network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoNetworkComponent implements OnInit {
  // user: UserProfileViewModel;
  network: UserNetworkDto;
  subscription: Subscription;

  constructor(private networkService: NetworkService) { }

  ngOnInit() {
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

  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this.networkService.postFollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            // this.getUser(); // obsolete due to event subsciption
            // element.innerText = 'Unfollow';
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
          });
      return;
    } else {
      this.networkService.postUnfollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            // this.getUser(); // obsolete due to event subsciption
            // element.innerText = 'Follow';
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
          });
      return;
    }
  }
}
