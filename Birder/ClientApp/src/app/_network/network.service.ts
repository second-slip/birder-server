import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { catchError, first, tap } from 'rxjs/operators';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpErrorHandlerService } from '@app/_services/http-error-handler.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { UserNetworkDto } from '@app/_models/UserNetworkDto';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkSidebarSummaryDto } from './network-sidebar/network-sidebar.component';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class NetworkService {

  private networkChanged = new Subject<any>();
  networkChanged$ = this.networkChanged.asObservable();

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getUserNetwork(): Observable<UserNetworkDto | ErrorReportViewModel> {
    return this.http.get<UserNetworkDto>('api/Network')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getFollowers(username: string): Observable<NetworkUserViewModel[]> {
    const options = username ?
    { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<NetworkUserViewModel[]>('api/Network/GetFollowers', options)
    .pipe(first());
  }

  getFollowing(username: string): Observable<NetworkUserViewModel[]> {
    const options = username ?
    { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<NetworkUserViewModel[]>('api/Network/GetFollowing', options)
    .pipe(first());
  }

  getNetworkSidebarSummary(): Observable<NetworkSidebarSummaryDto> {
    return this.http.get<NetworkSidebarSummaryDto>('api/Network/NetworkSidebarSummary')
      .pipe();
  }

  getNetworkSuggestions(): Observable<NetworkUserViewModel[] | ErrorReportViewModel> {
    return this.http.get<NetworkUserViewModel[]>('api/Network/NetworkSuggestions')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getSearchNetwork(searchCriterion: string): Observable<NetworkUserViewModel[] | ErrorReportViewModel> {
    const options = searchCriterion ?
      { params: new HttpParams().set('searchCriterion', searchCriterion) } : {};

    return this.http.get<NetworkUserViewModel[]>('api/Network/SearchNetwork', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postFollowUser(viewModel: NetworkUserViewModel): Observable<NetworkUserViewModel | ErrorReportViewModel> {
    return this.http.post<NetworkUserViewModel>('api/Network/Follow', viewModel, httpOptions)
    .pipe(
      tap(_ => { this.announceNetworkChanged(); }),
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postUnfollowUser(viewModel: NetworkUserViewModel): Observable<NetworkUserViewModel | ErrorReportViewModel> {
    return this.http.post<NetworkUserViewModel>('api/Network/Unfollow', viewModel, httpOptions)
    .pipe(
      tap(_ => { this.announceNetworkChanged(); }),
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  announceNetworkChanged(): void {
    this.networkChanged.next();
  }
}
