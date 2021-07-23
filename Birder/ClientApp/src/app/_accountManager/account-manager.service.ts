import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ManageProfileViewModel } from '../_models/ManageProfileViewModel';
import { ChangePasswordViewModel } from '../_models/ChangePasswordViewModel';
import { SetLocationViewModel } from '../_models/SetLocationViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AccountManagerService {

  constructor(private readonly _http: HttpClient) { }

  getUserProfile(): Observable<ManageProfileViewModel> {
    return this._http.get<ManageProfileViewModel>('api/Manage/GetUserProfile');
  }

  postUpdateProfile(viewModel: ManageProfileViewModel): Observable<ManageProfileViewModel> {
    return this._http.post<ManageProfileViewModel>('api/Manage/UpdateProfile', viewModel, httpOptions);
  }

  postChangePassword(viewModel: ChangePasswordViewModel): Observable<ChangePasswordViewModel> {
    return this._http.post<ChangePasswordViewModel>('api/Manage/ChangePassword', viewModel, httpOptions);
  }

  postSetLocation(viewModel: SetLocationViewModel): Observable<SetLocationViewModel> {
    return this._http.post<SetLocationViewModel>('api/Manage/SetLocation', viewModel, httpOptions);
  }

  postAvatar(formData: FormData): Observable<any> {
    return this._http.post('api/Manage/UploadAvatar', formData, { reportProgress: true, observe: 'events' });
  }
}
