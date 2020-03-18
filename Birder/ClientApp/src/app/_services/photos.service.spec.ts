import { TestBed } from '@angular/core/testing';

import { PhotosService } from './photos.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpErrorHandlerService } from './http-error-handler.service';

describe('PhotosService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      PhotosService, HttpErrorHandlerService
    ]
  }));

  it('should be created', () => {
    const service: PhotosService = TestBed.inject(PhotosService);
    expect(service).toBeTruthy();
  });
});
