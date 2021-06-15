import { TestBed } from '@angular/core/testing';

import { AccountManagerService } from './account-manager.service';
import { HttpErrorHandlerService } from '../_services/http-error-handler.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('AccountManagerService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      AccountManagerService, HttpErrorHandlerService
      // { provide: HttpErrorHandlerService, useValue: mockHttpErrorHandlerService }
    ]
  }));

  it('should be created', () => {
    const service: AccountManagerService = TestBed.inject(AccountManagerService);
    expect(service).toBeTruthy();
  });
});
