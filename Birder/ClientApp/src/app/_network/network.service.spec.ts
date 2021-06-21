import { TestBed } from '@angular/core/testing';

import { NetworkService } from './network.service';

import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('NetworkService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      NetworkService
    ]
  }));

  it('should be created', () => {
    const service: NetworkService = TestBed.inject(NetworkService);
    expect(service).toBeTruthy();
  });
});
