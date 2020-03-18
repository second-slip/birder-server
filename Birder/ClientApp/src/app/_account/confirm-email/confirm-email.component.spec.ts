import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailComponent } from './confirm-email.component';
import { AuthenticationService } from '@app/_services/authentication.service';
import { of } from 'rxjs';

describe('ConfirmEmailComponent', () => {
  let component: ConfirmEmailComponent;
  let fixture: ComponentFixture<ConfirmEmailComponent>;

  let mockAuthenticationService;

  beforeEach(async(() => {
    mockAuthenticationService = jasmine.createSpyObj(['logout']);

    TestBed.configureTestingModule({
      declarations: [ ConfirmEmailComponent ],
      providers: [
        { provide: AuthenticationService, useValue: mockAuthenticationService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmEmailComponent);
    component = fixture.componentInstance;
    mockAuthenticationService.logout.and.returnValue(of(false));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
