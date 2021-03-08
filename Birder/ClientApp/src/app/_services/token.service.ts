import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SetLocationViewModel } from '@app/_models/SetLocationViewModel';
import { AuthenticationService } from './authentication.service';
import { UserViewModel } from '@app/_models/UserViewModel';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private jwtHelper: JwtHelperService
    , private authenticationService: AuthenticationService) { }

  getFlikrKey(): string {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const tokenDecoded = this.jwtHelper.decodeToken(token);
      return tokenDecoded.FlickrKey;
    } else {
      return null;
    }
  }

  getMapKey(): string {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const tokenDecoded = this.jwtHelper.decodeToken(token);
      return tokenDecoded.MapKey;
    } else {
      return null;
    }
  }

  checkIsRecordOwner(username: string): boolean {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      const tokenDecoded = this.jwtHelper.decodeToken(token);
      if (tokenDecoded.unique_name === username) {
        return true;
      } else {
        return false;
      }

    } else {
      // TODO: Remove token (could be expired)
      return false;
    }
  }

  getAuthenticatedUserDetails(): UserViewModel {

    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const tokenDecoded = this.jwtHelper.decodeToken(token);

      return <UserViewModel>{
        userName: tokenDecoded.unique_name,
        avatar: tokenDecoded.ImageUrl,
        defaultLocationLatitude: Number(tokenDecoded.DefaultLatitude),
        defaultLocationLongitude: Number(tokenDecoded.DefaultLongitude)
      };
    } else {
      this.authenticationService.logout();
    }
  }

  getUsername(): string {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const tokenDecoded = this.jwtHelper.decodeToken(token);
      return tokenDecoded.unique_name;
    } else {
      this.authenticationService.logout();
      return null;
    }
  }

  getDefaultLocation(): SetLocationViewModel {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const tokenDecoded = this.jwtHelper.decodeToken(token);

      const model = <SetLocationViewModel>{
        defaultLocationLatitude: Number(tokenDecoded.DefaultLatitude),
        defaultLocationLongitude: Number(tokenDecoded.DefaultLongitude)
      };
      return model;

    } else {
      this.authenticationService.logout();
      return null;
    }
  }
}
