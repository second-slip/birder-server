import { Component, OnInit } from '@angular/core';
import { NetworkUserViewModel } from '../../_models/UserProfileViewModel';
import { UserService } from '../user.service';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-user-network',
  templateUrl: './user-network.component.html',
  styleUrls: ['./user-network.component.scss']
})
export class UserNetworkComponent implements OnInit {
  users: NetworkUserViewModel[];

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getNetwork();
  }

  followOrUnfollow(element, user: NetworkUserViewModel) {
    console.log(user);
    const action = element.innerText;

    if (action === 'Follow') {
      this.userService.postFollowUser(user.userName)
        .subscribe(
          (data: NetworkUserViewModel) => {
            // this.user = data;
            console.log(data);
            element.innerText = 'Unfollow';
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
            // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
          });
      //
      // element.innerText = 'Unfollow';
      return;
    } else {
      //
      this.userService.postUnfollowUser(user.userName)
        .subscribe(
          (data: NetworkUserViewModel) => {
            // this.user = data;
            console.log(data);
            element.innerText = 'Follow';
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
            // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
          });

      //
      // element.innerText = 'Follow';
      return;
    }
  }

  getNetwork(): void {
    // const username = this.route.snapshot.paramMap.get('username');

    this.userService.getNetwork('')
    .subscribe(
      (data: NetworkUserViewModel[]) => {
        this.users = data;
      },
      (error: ErrorReportViewModel) => {
        console.log('bad request');
        // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
      });


  }

}
