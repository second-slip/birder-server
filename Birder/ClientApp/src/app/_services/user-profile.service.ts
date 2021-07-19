import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserProfileViewModel } from '@app/_models/UserProfileViewModel';

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {

  constructor(private readonly _http: HttpClient) { }

  getUserProfile(username: string): Observable<UserProfileViewModel> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this._http.get<UserProfileViewModel>('api/UserProfile', options);
  }
}
