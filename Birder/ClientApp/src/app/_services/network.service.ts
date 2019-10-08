import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { UserNetworkDto } from '../_models/UserNetworkDto';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError, tap } from 'rxjs/operators';
import { NetworkUserViewModel } from '../_models/UserProfileViewModel';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';

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
