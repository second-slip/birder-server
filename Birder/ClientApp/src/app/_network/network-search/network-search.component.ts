import { Component, ViewEncapsulation } from '@angular/core';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_network/network.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-network-search',
  templateUrl: './network-search.component.html',
  styleUrls: ['./network-search.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkSearchComponent {
  results$: Observable<NetworkUserViewModel[]>;
  public errorObject = null;

  constructor(private networkService: NetworkService) { }

  searchNetwork(value: any): void {
    this.results$ = this.networkService.getSearchNetwork(value.searchTerm)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}
