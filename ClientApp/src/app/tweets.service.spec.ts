import { TestBed } from '@angular/core/testing';

import { TweetsService } from './tweets.service';

describe('TweetsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TweetsService = TestBed.get(TweetsService);
    expect(service).toBeTruthy();
  });
});
