import { TestBed } from '@angular/core/testing';

import { TokenService } from './token.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { JwtModule } from '@auth0/angular-jwt';
import { tokenGetter } from '@app/app.module';
import { AuthenticationService } from './authentication.service';

describe('TokenService', () => {
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
      TokenService, AuthenticationService
    ]
  }));

  it('should be created', () => {
    const service: TokenService = TestBed.inject(TokenService);
    expect(service).toBeTruthy();
  });
});
