import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-following',
  templateUrl: './following.component.html',
  styleUrls: ['./following.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FollowingComponent {
  username: string;
  public errorObject = null;
  following$: Observable<NetworkUserViewModel[]>;

  constructor(private route: ActivatedRoute
    , private networkService: NetworkService) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => {
        this.username = pmap.get('username');
        this.getFollowing();
      })
    });
  }

  getFollowing(): void {
    this.following$ = this.networkService.getFollowing(this.username)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}
