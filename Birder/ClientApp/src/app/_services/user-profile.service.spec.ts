import { TestBed } from '@angular/core/testing';

import { UserProfileService } from './user-profile.service';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('UserProfileService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      UserProfileService, HttpErrorHandlerService
    ]
  }));

  it('should be created', () => {
    const service: UserProfileService = TestBed.inject(UserProfileService);
    expect(service).toBeTruthy();
  });
});
