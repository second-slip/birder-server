import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserProfileViewModel, NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { UserProfileService } from '@app/_services/user-profile.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserProfileComponent {
  userProfile: UserProfileViewModel;
  requesting: boolean;
  // tabstatus = {};
  // active;

  constructor(private networkService: NetworkService
    , private userProfileService: UserProfileService
    , private route: ActivatedRoute
    , private toast: ToastrService
    , private router: Router) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => this.getUser(pmap.get('username')));
      // this.getUser();
      // the next two statements reset the tabs.  This is required when the page is reloaded
      // with different data.  Otherwise the 'sightings' child component keeps its original data.
      // this.active = 1;
      // this.tabstatus = {};
    });
  }

  getUser(username: string): void {
    this.requesting = true;
    this.userProfileService.getUserProfile(username)
      .subscribe(
        (data: UserProfileViewModel) => {
          this.userProfile = data;
        },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.serverCustomMessage, 'An error occurred');
          this.router.navigate(['/']);
        },
        () => this.requesting = false
      );
  }
}



