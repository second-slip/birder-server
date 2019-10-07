import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ObservationViewModel } from '../../../_models/ObservationViewModel';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { UserProfileViewModel, NetworkUserViewModel } from '../../../_models/UserProfileViewModel';
import { ToastrService } from 'ngx-toastr';
import { UserProfileService } from '../user-profile.service';
import { NetworkService } from '../../../_services/network.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserProfileComponent {
  userProfile: UserProfileViewModel;
  observations: ObservationViewModel[]; // lazy load on demand

  constructor(private networkService: NetworkService
    , private userProfileService: UserProfileService
            , private route: ActivatedRoute
            , private toast: ToastrService
            , private router: Router) {
                route.params.subscribe(_ => {
                  this.getUser();
                });
  }

  getUser(): void {
    const username = this.route.snapshot.paramMap.get('username');

    this.userProfileService.getUserProfile(username)
      .subscribe(
        (data: UserProfileViewModel) => {
          this.userProfile = data;
        },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.serverCustomMessage, 'An error occurred');
          this.router.navigate(['/']);
        });
  }

  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this.networkService.postFollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            this.toast.info('You are now following ' + data.userName, 'Success');
            this.getUser();
            element.innerText = 'Unfollow';
          },
          (error: ErrorReportViewModel) => {
            // console.log(error);
            this.toast.error(error.serverCustomMessage, 'An error occurred');
          });
      return;
    } else {
      this.networkService.postUnfollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            this.toast.info('You have unfollowed ' + data.userName, 'Success');
            this.getUser();
            element.innerText = 'Follow';
          },
          (error: ErrorReportViewModel) => {
            // console.log(error);
            this.toast.error(error.serverCustomMessage, 'An error occurred');
          });
      return;
    }
  }

  getObservations(): void { }

}

            // const index = this.user.following.findIndex(i => i.userName === data.userName);
            // if (index >= 0) {
            //   // this.user.following.splice(index, 1);
            //   this.user.following[index].isFollowing = false;
            // }
            // const index1 = this.user.followers.findIndex(i => i.userName === data.userName);
            // if (index >= 0) {
            //   // this.user.following.splice(index, 1);
            //   // alert(this.user.followers[index1].userName + '; ' + this.user.followers[index1].isFollowing);
            //   this.user.followers[index1].isFollowing = false;
            //   // alert(this.user.followers[index1].userName + '; ' + this.user.followers[index1].isFollowing);
            // }
