import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NetworkUserViewModel } from '../../../_models/UserProfileViewModel';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { UserService } from '../../../app/user.service';
import { Subscription } from 'rxjs';
import { UserNetworkDto } from '../../../_models/UserNetworkDto';

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

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getUserNetwork();
    this.subscription = this.userService.networkChanged$
    .subscribe(_ => {
      this.onNetworkChanged();
    });
  }

  onNetworkChanged() {
    this.getUserNetwork();
  }

  getUserNetwork(): void {
    this.userService.getUserNetwork()
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
      this.userService.postFollowUser(user)
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
      this.userService.postUnfollowUser(user)
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
