import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ManageProfileViewModel } from '../_models/ManageProfileViewModel';
import { ChangePasswordViewModel } from '../_models/ChangePasswordViewModel';
import { SetLocationViewModel } from '../_models/SetLocationViewModel';
import { first } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AccountManagerService {

  constructor(private http: HttpClient) { }

  getUserProfile(): Observable<ManageProfileViewModel> {
    return this.http.get<ManageProfileViewModel>('api/Manage/GetUserProfile')
      .pipe(first());
  }

  postUpdateProfile(viewModel: ManageProfileViewModel): Observable<ManageProfileViewModel> {
    return this.http.post<ManageProfileViewModel>('api/Manage/UpdateProfile', viewModel, httpOptions)
      .pipe(first());
  }

  postChangePassword(viewModel: ChangePasswordViewModel): Observable<ChangePasswordViewModel> {
    return this.http.post<ChangePasswordViewModel>('api/Manage/ChangePassword', viewModel, httpOptions)
      .pipe(first());
  }

  postSetLocation(viewModel: SetLocationViewModel): Observable<SetLocationViewModel> {
    return this.http.post<SetLocationViewModel>('api/Manage/SetLocation', viewModel, httpOptions)
      .pipe(first());
  }

  postAvatar(formData: FormData): Observable<any> {
    return this.http.post('api/Manage/UploadAvatar', formData, { reportProgress: true, observe: 'events' })
      .pipe(first());
  }
}
