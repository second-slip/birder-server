import { TestBed } from '@angular/core/testing';

import { NetworkService } from './network.service';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('NetworkService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      NetworkService, HttpErrorHandlerService
      // { provide: HttpErrorHandlerService, useValue: mockHttpErrorHandlerService }
    ]
  }));

  it('should be created', () => {
    const service: NetworkService = TestBed.inject(NetworkService);
    expect(service).toBeTruthy();
  });
});
