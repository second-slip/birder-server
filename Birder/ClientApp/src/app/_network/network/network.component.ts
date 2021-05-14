import { Component, ViewEncapsulation } from '@angular/core';
import { NetworkSummaryDto } from '@app/_models/NetworkSummaryDto';
import { TokenService } from '@app/_services/token.service';
import { Observable, Subscription, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { NetworkService } from '../network.service';

@Component({
  selector: 'app-network',
  templateUrl: './network.component.html',
  styleUrls: ['./network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkComponent {
  active;
  tabstatus = {};

  network$: Observable<NetworkSummaryDto>;
  networkChangeSubscription: Subscription;
  public errorObject = null;
  username: string;

  constructor(private networkService: NetworkService, private tokenService: TokenService) {
    this.active = 1;
    this.tabstatus = {};
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
    this.network$ = this.networkService.getNetworkSummary()
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}
