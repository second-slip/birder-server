import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-followers',
  templateUrl: './followers.component.html',
  styleUrls: ['./followers.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FollowersComponent {
  username: string;
  public errorObject = null;
  followers$: Observable<NetworkUserViewModel[]>;

  constructor(private route: ActivatedRoute
    , private networkService: NetworkService) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => {
        this.username = pmap.get('username');
        this.getFollowers();
      })
    });
  }

  getFollowers(): void {
    this.followers$ = this.networkService.getFollowers(this.username)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}
