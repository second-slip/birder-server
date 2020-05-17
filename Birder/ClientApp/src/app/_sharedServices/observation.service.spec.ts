import { TestBed } from '@angular/core/testing';

import { ObservationService } from './observation.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpErrorHandlerService } from '../_services/http-error-handler.service';

describe('ObservationService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      ObservationService, HttpErrorHandlerService
    ]
  }));

  it('should be created', () => {
    const service: ObservationService = TestBed.inject(ObservationService);
    expect(service).toBeTruthy();
  });
});
