import { TestBed } from '@angular/core/testing';

import { BirdsListService } from './birds-list.service';

describe('BirdsListService', () => {
  let service: BirdsListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BirdsListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
