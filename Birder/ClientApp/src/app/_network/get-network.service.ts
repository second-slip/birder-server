import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetNetworkService {

  private readonly _isError$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private readonly _isLoading$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private readonly _following$: BehaviorSubject<NetworkUserViewModel[]> = new BehaviorSubject<NetworkUserViewModel[]>(null);
  private readonly _followers$: BehaviorSubject<NetworkUserViewModel[]> = new BehaviorSubject<NetworkUserViewModel[]>(null);

  constructor(private readonly _httpClient: HttpClient) { }

  public get isError(): Observable<boolean> {
    return this._isError$.asObservable();
  }

  public get isLoading(): Observable<boolean> {
    return this._isLoading$.asObservable();
  }

  public get getFollowers(): Observable<NetworkUserViewModel[]> {
    return this._followers$.asObservable();
  }

  public get getFollowing(): Observable<NetworkUserViewModel[]> {
    return this._following$.asObservable();
  }
}
