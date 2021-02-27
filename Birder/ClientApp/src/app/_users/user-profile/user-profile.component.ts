import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserProfileViewModel, NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { UserProfileService } from '@app/_services/user-profile.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';
import { catchError, share } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserProfileComponent implements OnInit {
  analysis$: Observable<ObservationAnalysisViewModel>;
  userProfile$: Observable<UserProfileViewModel>;
  username: string;

  constructor(private networkService: NetworkService
    , private observationsAnalysisService: ObservationsAnalysisService
    , private userProfileService: UserProfileService
    , private route: ActivatedRoute
    , private toast: ToastrService) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username');
      this.ngOnInit();
    });

  }

  ngOnInit() {

    this.userProfile$ = this.userProfileService.getUserProfile(this.username)
      .pipe(share()),
      catchError(err => {
        //this.errorObject = err;
        return throwError(err);
      });

    this.analysis$ = this.observationsAnalysisService.getObservationAnalysis(this.username)
      .pipe(share()),
      catchError(err => {
        //this.errorObject = err;
        return throwError(err);
      });

  }



  //getUser(username: string) {
  //this.requesting = true;
  // this.userProfile$ = this.userProfileService.getUserProfile(username)
  // .pipe(share())

  // catchError(err => {
  //   //this.errorObject = err;
  //   return throwError(err);
  // })
  // .subscribe(
  //   (data: UserProfileViewModel) => {
  //     this.userProfile = data;
  //   },
  // (error: any) => {
  //   //this.toast.error(error.friendlyMessage, 'An error occurred');
  //   this.router.navigate(['/']);
  // },
  // () => console.log('finally'))

  //}

  // getObservationAnalysis(username: string): void {
  //   this.observationsAnalysisService.getObservationAnalysis(username)
  //     .subscribe(
  //       (data: ObservationAnalysisViewModel) => {
  //         this.analysis = data;
  //       },
  //       (error: ErrorReportViewModel) => {
  //         console.log(error);
  //       }
  //     );
  // }

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



