import { TestBed } from '@angular/core/testing';

import { ObservationService } from './observation.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';


describe('ObservationService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      ObservationService
    ]
  }));

  it('should be created', () => {
    const service: ObservationService = TestBed.inject(ObservationService);
    expect(service).toBeTruthy();
  });
});
