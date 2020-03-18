import { TestBed } from '@angular/core/testing';

import { AccountService } from './account.service';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('AccountService', () => {
  beforeEach(() => TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [
        AccountService, HttpErrorHandlerService
        // { provide: HttpErrorHandlerService, useValue: mockHttpErrorHandlerService }
      ]
  }));

  it('should be created', () => {
    const service: AccountService = TestBed.inject(AccountService);
    expect(service).toBeTruthy();
  });
});
