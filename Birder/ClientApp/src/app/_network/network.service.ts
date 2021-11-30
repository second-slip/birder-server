import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkSummaryDto } from '@app/_models/NetworkSummaryDto';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class NetworkService {

  private networkChanged = new Subject<any>();
  networkChanged$ = this.networkChanged.asObservable();

  constructor(private readonly _http: HttpClient) { }

  getNetworkSummary(): Observable<NetworkSummaryDto> {
    return this._http.get<NetworkSummaryDto>('api/Network');
  }

  getFollowers(username: string): Observable<NetworkUserViewModel[]> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this._http.get<NetworkUserViewModel[]>('api/Network/GetFollowers', options);
  }

  getFollowing(username: string): Observable<NetworkUserViewModel[]> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this._http.get<NetworkUserViewModel[]>('api/Network/GetFollowing', options);
  }

  getSearchNetwork(searchCriterion: string): Observable<NetworkUserViewModel[]> {
    const options = searchCriterion ?
      { params: new HttpParams().set('searchCriterion', searchCriterion) } : {};

    return this._http.get<NetworkUserViewModel[]>('api/Network/SearchNetwork', options);
  }

  postFollowUser(viewModel: NetworkUserViewModel): Observable<NetworkUserViewModel> {
    return this._http.post<NetworkUserViewModel>('api/Network/Follow', viewModel, httpOptions)
      .pipe(tap(_ => { this._onNetworkChanged(); }));
  }

  postUnfollowUser(viewModel: NetworkUserViewModel): Observable<NetworkUserViewModel> {
    return this._http.post<NetworkUserViewModel>('api/Network/Unfollow', viewModel, httpOptions)
      .pipe(tap(_ => { this._onNetworkChanged(); }));
  }

  private _onNetworkChanged(): void {
    this.networkChanged.next(1);
  }
}
