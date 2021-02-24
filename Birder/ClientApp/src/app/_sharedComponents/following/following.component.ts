import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-following',
  templateUrl: './following.component.html',
  styleUrls: ['./following.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FollowingComponent {
  username: string;
  requesting: boolean;
  following: NetworkUserViewModel[];

  constructor(private route: ActivatedRoute
    , private networkService: NetworkService
    , private toast: ToastrService) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => {
        this.username = pmap.get('username');
        this.getFollowing();
      })
    });
  }

  getFollowing(): void {
    this.requesting = true;
    this.networkService.getFollowing(this.username)
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.following = data;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
        },
        () => this.requesting = false
      );
  }

  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this.networkService.postFollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            element.innerText = 'Unfollow';
            this.toast.info('You have followed ' + data.userName, 'Success');
          },
          (error: ErrorReportViewModel) => {
            this.toast.error(error.friendlyMessage, 'An error occurred');
          });
      return;
    } else {
      this.networkService.postUnfollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            element.innerText = 'Follow';
            this.toast.info('You have unfollowed ' + data.userName, 'Success');
          },
          (error: ErrorReportViewModel) => {
            this.toast.error(error.friendlyMessage, 'An error occurred');
          });
      return;
    }
  }
}
