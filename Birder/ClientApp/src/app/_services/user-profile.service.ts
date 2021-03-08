import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserProfileViewModel } from '@app/_models/UserProfileViewModel';
import { first } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {

  constructor(private http: HttpClient) { }

  getUserProfile(username: string): Observable<UserProfileViewModel> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<UserProfileViewModel>('api/UserProfile', options)
      .pipe(first());
  }
}
