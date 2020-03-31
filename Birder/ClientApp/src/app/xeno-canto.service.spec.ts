import { TestBed } from '@angular/core/testing';

import { XenoCantoService } from './xeno-canto.service';

describe('XenoCantoService', () => {
  let service: XenoCantoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(XenoCantoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
