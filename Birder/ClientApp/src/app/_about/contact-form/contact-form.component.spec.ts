import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ContactFormService } from '../contact-form.service';

import { ContactFormComponent } from './contact-form.component';

describe('ContactFormComponent', () => {
  let component: ContactFormComponent;
  let fixture: ComponentFixture<ContactFormComponent>;

  let mockContactFormService: any;

  beforeEach(async () => {
    mockContactFormService = jasmine.createSpyObj(['postMessage']);

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [ContactFormComponent],
      providers: [{ provide: ContactFormService, useValue: mockContactFormService }]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
