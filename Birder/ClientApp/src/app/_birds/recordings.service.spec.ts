import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { RecordingsService } from './recordings.service';

describe('RecordingsService', () => {
  let service: RecordingsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(RecordingsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
