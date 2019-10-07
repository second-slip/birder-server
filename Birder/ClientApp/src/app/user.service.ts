import { Injectable } from '@angular/core';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { UserProfileViewModel, NetworkUserViewModel } from '../_models/UserProfileViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private networkChanged = new Subject<any>();
  networkChanged$ = this.networkChanged.asObservable();

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getUser(username: string): Observable<UserProfileViewModel | ErrorReportViewModel> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<UserProfileViewModel>('api/UserProfile/GetUserProfile', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getNetwork(): Observable<NetworkUserViewModel[] | ErrorReportViewModel> {
    // const options = searchCriterion ?
    //   { params: new HttpParams().set('searchCriterion', searchCriterion) } : {};

    return this.http.get<NetworkUserViewModel[]>('api/Network/GetNetwork')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  // added
  getSearchNetwork(searchCriterion: string): Observable<NetworkUserViewModel[] | ErrorReportViewModel> {
    const options = searchCriterion ?
      { params: new HttpParams().set('searchCriterion', searchCriterion) } : {};

    return this.http.get<NetworkUserViewModel[]>('api/Network/SearchNetwork', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postFollowUser(viewModel: NetworkUserViewModel): Observable<NetworkUserViewModel | ErrorReportViewModel> {
    // const options = username ?
    //   { params: new HttpParams().set('username', username) } : {};
    //   console.log(options);

    return this.http.post<NetworkUserViewModel>('api/Network/Follow', viewModel, httpOptions)
    .pipe(
      tap(network => { this.announceNetworkChanged(); }),
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postUnfollowUser(viewModel: NetworkUserViewModel): Observable<NetworkUserViewModel | ErrorReportViewModel> {
    return this.http.post<NetworkUserViewModel>('api/Network/Unfollow', viewModel, httpOptions)
    .pipe(
      tap(network => { this.announceNetworkChanged(); }),
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  announceNetworkChanged(): void {
    this.networkChanged.next();
  }
}
