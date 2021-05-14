import { Component, ViewEncapsulation } from '@angular/core';
import { UserNetworkDto } from '@app/_models/UserNetworkDto';
import { NetworkService } from '@app/_network/network.service';
import { TokenService } from '@app/_services/token.service';
import { Observable, Subscription, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-network-sidebar',
  templateUrl: './network-sidebar.component.html',
  styleUrls: ['./network-sidebar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkSidebarComponent {
  network$: Observable<NetworkSidebarSummaryDto>;
  networkChangeSubscription: Subscription;
  public errorObject = null;
  username: string;

  constructor(private networkService: NetworkService, private tokenService: TokenService) {
    this.username = this.tokenService.getUsername();
    this.getData();
    this.networkChangeSubscription = this.networkService.networkChanged$
      .subscribe(_ => {
        this.onNetworkChanged();
      });
  }

  ngOnDestroy() {
    this.networkChangeSubscription.unsubscribe();
  }

  onNetworkChanged() {
    this.getData();
  }

  getData() {
    this.network$ = this.networkService.getNetworkSidebarSummary()
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}

export interface NetworkSidebarSummaryDto {
  followersCount: number;
  followingCount: number;
  suggestedUsersToFollow: UserNetworkDto[];
}
