import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserProfileViewModel, NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { UserProfileService } from '@app/_services/user-profile.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserProfileComponent {
  analysis: ObservationAnalysisViewModel;
  userProfile: UserProfileViewModel;
  requesting: boolean;
  // tabstatus = {};
  // active;

  constructor(private networkService: NetworkService
    , private observationsAnalysisService: ObservationsAnalysisService
    , private userProfileService: UserProfileService
    , private route: ActivatedRoute
    , private toast: ToastrService
    , private router: Router) {
    route.params.subscribe(_ => {
      this.getObservationAnalysis();
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
          this.toast.error(error.friendlyMessage, 'An error occurred');
          this.router.navigate(['/']);
        },
        () => this.requesting = false
      );
  }

  getObservationAnalysis(): void {
    this.observationsAnalysisService.getObservationAnalysis()
      .subscribe(
        (data: ObservationAnalysisViewModel) => {
          this.analysis = data;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
        }
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



