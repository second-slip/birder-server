import { TestBed } from '@angular/core/testing';

import { UsernameValidationServiceService } from './username-validation-service.service';

describe('UsernameValidationServiceService', () => {
  let service: UsernameValidationServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UsernameValidationServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
