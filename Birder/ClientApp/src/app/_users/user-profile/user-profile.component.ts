import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserProfileViewModel, NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_network/network.service';
import { UserProfileService } from '@app/_services/user-profile.service';
import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserProfileComponent {
  analysis$: Observable<ObservationAnalysisViewModel>;
  userProfile$: Observable<UserProfileViewModel>;
  public errorObject = null;

  constructor(private readonly _networkService: NetworkService
    , private readonly _observationsAnalysisService: ObservationsAnalysisService
    , private readonly _userProfileService: UserProfileService
    , private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.getData(params.get('username'));
    });
  }


  // Do not use the DATA SERVICE to get other user's analysis

  private getData(username: string) {
    this.userProfile$ = this._userProfileService.getUserProfile(username)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err); // error thrown by interceptor...
        })
      );

    this.analysis$ = this._observationsAnalysisService.getObservationAnalysis(username)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err); // error thrown by interceptor...
        })
      );
  }

  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this._networkService.postFollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            element.innerText = 'Unfollow';
            // this.toast.info('You have followed ' + data.userName, 'Success');
          },
          (error: any) => {
            // this.toast.error(error.friendlyMessage, 'An error occurred');
          });
      return;
    } else {
      this._networkService.postUnfollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            element.innerText = 'Follow';
            // this.toast.info('You have unfollowed ' + data.userName, 'Success');
          },
          (error: any) => {
            // this.toast.error(error.friendlyMessage, 'An error occurred');
          });
      return;
    }
  }
}
