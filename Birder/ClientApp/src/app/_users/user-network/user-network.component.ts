import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NetworkUserViewModel } from '../../../_models/UserProfileViewModel';
import { UserService } from '../../user.service';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { ToastrService } from 'ngx-toastr';

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
          if (this.users.length > 0) {
            this.toast.info(this.users.length.toString() + ' results were found', 'Search successful');
          } else {
            this.toast.warning('No results were found', 'Search unsuccessful');
          }
        },
        (error: ErrorReportViewModel) => {
          if (error.type === 'client-side or network error occurred') {
            this.toast.error(error.serverCustomMessage, 'An error occurred');
          } else {
            this.toast.error('Try a different search query', 'Search unsuccessful');
          }
        });
  }

  getNetwork(): void {
    this.userService.getNetwork()
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
        },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.serverCustomMessage, 'An error occurred');
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
