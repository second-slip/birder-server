import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-user-network',
  templateUrl: './user-network.component.html',
  styleUrls: ['./user-network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserNetworkComponent implements OnInit {
  users: NetworkUserViewModel[];
  // searchTerm: string;
  customSearch = false;
  loading = false;

  constructor(private networkService: NetworkService
    , private toast: ToastrService) { }

  ngOnInit() {
    this.getNetwork();
  }

  // search(value: any): void {
    
  //   // console.log(value)
  //   // alert(value.searchTerm);
  //   this.searchNetwork(value.searchTerm);
  //   this.customSearch = true;
  // }

  searchNetwork(value: any): void {

    this.customSearch = true;
    this.loading = true;


    this.networkService.getSearchNetwork(value.searchTerm)
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
          if (this.users.length > 0) {
            this.toast.info(this.users.length.toString() + ' results were found', 'Search successful');
          } else {
            this.toast.warning('No results were found', 'Search unsuccessful');
          }
          this.loading = false;
        },
        (error: ErrorReportViewModel) => {
          if (error.type === 'client-side or network error occurred') {
            this.toast.error(error.serverCustomMessage, 'An error occurred');
          } else {
            this.toast.error('Try a different search query', 'Search unsuccessful');
          }
          this.loading = false;
        });
  }

  getNetwork(): void {
    this.loading = true;
    this.networkService.getNetworkSuggestions()
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
          this.loading = false;
        },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.serverCustomMessage, 'An error occurred');
        });
  }

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
