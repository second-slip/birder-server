import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { UserNetworkDto } from '@app/_models/UserNetworkDto';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-info-network',
  templateUrl: './info-network.component.html',
  styleUrls: ['./info-network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoNetworkComponent implements OnInit, OnDestroy {
  // user: UserProfileViewModel;
  network: UserNetworkDto;
  networkChangeSubscription: Subscription;
  requesting: boolean

  constructor(private networkService: NetworkService) { }

  ngOnInit() {
    this.getUserNetwork();
    this.networkChangeSubscription = this.networkService.networkChanged$
      .subscribe(_ => {
        this.onNetworkChanged();
      });
  }

  ngOnDestroy() {
    this.networkChangeSubscription.unsubscribe();
  }

  onNetworkChanged() {
    this.getUserNetwork();
  }

  getUserNetwork(): void {

    this.requesting = true;
    this.networkService.getUserNetwork()
      .subscribe(
        (data: UserNetworkDto) => {
          this.network = data;
          this.requesting = false;
        },
        (error: ErrorReportViewModel) => {
          this.requesting = false;
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
