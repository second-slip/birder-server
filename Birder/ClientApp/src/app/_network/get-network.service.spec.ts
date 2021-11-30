import { TestBed } from '@angular/core/testing';

import { GetNetworkService } from './get-network.service';

describe('GetNetworkService', () => {
  let service: GetNetworkService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetNetworkService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
