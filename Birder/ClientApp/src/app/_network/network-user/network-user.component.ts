import { Component, Input, ViewEncapsulation } from '@angular/core';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_network/network.service';
// 

@Component({
  selector: 'app-network-user',
  templateUrl: './network-user.component.html',
  styleUrls: ['./network-user.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkUserComponent {
  @Input() user: NetworkUserViewModel

  constructor(private networkService: NetworkService) { }

  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this.networkService.postFollowUser(user)
        .subscribe({
          next: (data: NetworkUserViewModel) => {
            element.innerText = 'Unfollow';
            // this.toast.info('You have followed ' + data.userName, 'Success');
          },
          error: (error => {
            // this.toast.error(error.friendlyMessage, 'An error occurred');
          })
        });
      return;
    } else {
      this.networkService.postUnfollowUser(user)
        .subscribe({
          next: (data: NetworkUserViewModel) => {
            element.innerText = 'Follow';
            // this.toast.info('You have unfollowed ' + data.userName, 'Success');
          },
          error: (error => {
            // this.toast.error(error.friendlyMessage, 'An error occurred');
          })
        });
      return;
    }
  }
}
