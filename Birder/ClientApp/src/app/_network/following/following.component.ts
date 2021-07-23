import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_network/network.service';
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

  constructor(private _route: ActivatedRoute
    , private _networkService: NetworkService) {
    _route.params.subscribe(_ => {
      this._route.paramMap.subscribe(pmap => {
        this.username = pmap.get('username');
        this.getFollowing();
      })
    });
  }

  private getFollowing(): void {
    this.following$ = this._networkService.getFollowing(this.username)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}
