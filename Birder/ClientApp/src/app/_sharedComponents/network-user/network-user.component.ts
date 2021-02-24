import { Component, Input, ViewEncapsulation } from '@angular/core';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-network-user',
  templateUrl: './network-user.component.html',
  styleUrls: ['./network-user.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkUserComponent {
  @Input() user: NetworkUserViewModel

  constructor(private networkService: NetworkService, private toast: ToastrService) { }

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
