import { TestBed } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';

import { tokenGetter } from '@app/app.module';

describe('AuthenticationService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule, JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        // Exclude this URL from JWT (doesn't add the authentication header)
        // blacklistedRoutes: [
        //   '/api/login',
        // ]
      }
    }), ],
    providers: [
      AuthenticationService, JwtHelperService

    ]
  }));

  it('should be created', () => {
    const service: AuthenticationService = TestBed.inject(AuthenticationService);
    expect(service).toBeTruthy();
  });
});
