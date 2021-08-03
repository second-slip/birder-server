import { Component, ViewEncapsulation } from '@angular/core';
import { NetworkSummaryDto } from '@app/_models/NetworkSummaryDto';
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
  network$: Observable<NetworkSummaryDto>;
  networkChangeSubscription: Subscription;
  public errorObject = null;
  public username: string;

  constructor(private readonly _networkService: NetworkService, private readonly _tokenService: TokenService) {
    this.username = this._tokenService.getUsername();
    this.getData();
    this.networkChangeSubscription = this._networkService.networkChanged$
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
    this.network$ = this._networkService.getNetworkSummary()
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}

