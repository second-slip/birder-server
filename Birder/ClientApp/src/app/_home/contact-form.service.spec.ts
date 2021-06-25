import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { ContactFormService } from './contact-form.service';

describe('ContactFormService', () => {
  let service: ContactFormService;
  let httpClient: HttpClient;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ContactFormService]
    });

    httpClient = TestBed.inject(HttpClient);
    service = TestBed.inject(ContactFormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
