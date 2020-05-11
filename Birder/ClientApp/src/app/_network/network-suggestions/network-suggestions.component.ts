import { Component, OnInit } from '@angular/core';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { ToastrService } from 'ngx-toastr';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-network-suggestions',
  templateUrl: './network-suggestions.component.html',
  styleUrls: ['./network-suggestions.component.scss']
})
export class NetworkSuggestionsComponent implements OnInit {
  users: NetworkUserViewModel[];

  constructor(private networkService: NetworkService
    , private toast: ToastrService) { }

  ngOnInit() {
    this.getNetwork();
  }

  getNetwork(): void {
    this.networkService.getNetworkSuggestions()
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
        },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.serverCustomMessage, 'An error occurred');
        });
  }

  // This is repeated in several places.  Factor into its own component...
  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this.networkService.postFollowUser(user)
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
      this.networkService.postUnfollowUser(user)
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
