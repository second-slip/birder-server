import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NetworkUserViewModel } from '../../../_models/UserProfileViewModel';
import { UserService } from '../../user.service';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-network',
  templateUrl: './user-network.component.html',
  styleUrls: ['./user-network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserNetworkComponent implements OnInit {
  users: NetworkUserViewModel[];
  searchTerm: string;
  customSearch = false;

  constructor(private userService: UserService
    , private router: Router
    , private toast: ToastrService) { }

  ngOnInit() {
    this.getNetwork();
  }

  search(): void {
    this.searchNetwork(this.searchTerm);
    this.customSearch = true;
  }

  searchNetwork(searchCriterion: string): void {
    this.userService.getSearchNetwork(searchCriterion)
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
        },
        (error: ErrorReportViewModel) => { // <--------- this needs to change
          this.toast.error(error.serverCustomMessage, 'An error occurred');
          // this.router.navigate(['/']);
        });
  }

  getNetwork(): void {
    this.userService.getNetwork()
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
        },
        (error: ErrorReportViewModel) => { // <--------- this needs to change
          this.toast.error(error.serverCustomMessage, 'An error occurred');
          // this.router.navigate(['/']);
        });
  }


  

  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this.userService.postFollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            this.toast.info('You are now following ' + data.userName, 'Success');
            element.innerText = 'Unfollow';
          },
          (error: ErrorReportViewModel) => {
            this.toast.error(error.serverCustomMessage, 'An error occurred');
          });
      return;
    } else {
      this.userService.postUnfollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            this.toast.info('You have unfollowed ' + data.userName, 'Success');
            element.innerText = 'Follow';
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
            this.toast.error(error.serverCustomMessage, 'An error occurred');
          });
      return;
    }
  }
}
