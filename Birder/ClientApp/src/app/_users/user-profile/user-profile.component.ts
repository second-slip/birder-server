import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
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
export class UserProfileComponent implements OnInit {
  analysis: ObservationAnalysisViewModel;
  userProfile: UserProfileViewModel;
  username: string;
  requesting: boolean;
  // tabstatus = {};
  // active;

  constructor(private networkService: NetworkService
    , private observationsAnalysisService: ObservationsAnalysisService
    , private userProfileService: UserProfileService
    , private route: ActivatedRoute
    , private toast: ToastrService
    , private router: Router) { 
      this.route.paramMap.subscribe(params => {
        this.username = params.get('username');
        this.ngOnInit();
        //this.hj(params.get('username'));
    });
  // route.params.subscribe(_ => {
  //   this.route.paramMap.subscribe(params => {
  //     this.username = params.get('username');
  //     this.getObservationAnalysis(params.get('username'));
  //     this.getUser(params.get('username'));
  //   })

  // });
  }

  ngOnInit() {
    this.getObservationAnalysis(this.username);
    this.getUser(this.username);
  }

  // hj(un: string) {
  //   // this.route.params.subscribe(params => {
  //   //   this.username = params['username'];
  //   // });
  //   //const un = this.route.paramMap.subscribe(p => p.get('username'));
  //   // this.hero$ = this.service.getHero(heroId);
  //   console.log(un);
  //   this.getObservationAnalysis(un);
  //   this.getUser(un);
  // }

  // ngOnInit() {
  //   this.getObservationAnalysis();
  //   this.getUser();
  // }

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

  getObservationAnalysis(username: string): void {
    this.observationsAnalysisService.getObservationAnalysis(username)
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



