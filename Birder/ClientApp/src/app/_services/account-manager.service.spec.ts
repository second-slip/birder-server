import { TestBed } from '@angular/core/testing';

import { AccountManagerService } from './account-manager.service';

describe('AccountManagerService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AccountManagerService = TestBed.inject(AccountManagerService);
    expect(service).toBeTruthy();
  });
});
