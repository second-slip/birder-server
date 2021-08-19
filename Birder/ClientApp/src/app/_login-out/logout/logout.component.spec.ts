import { async, ComponentFixture, TestBed } from '@angular/core/testing';


import { RouterTestingModule } from '@angular/router/testing';
import { AuthenticationService } from '@app/_services/authentication.service';
import { of } from 'rxjs';
import { FormBuilder } from '@angular/forms';

import { LogoutComponent } from './logout.component';

describe('LogoutComponent', () => {
  let component: LogoutComponent;
  let fixture: ComponentFixture<LogoutComponent>;

  let mockAuthenticationService;

  beforeEach(async(() => {
    mockAuthenticationService = jasmine.createSpyObj(['logout']);

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
          // { path: 'login', component: DummyLoginLayoutComponent },
        ])
      ],
      declarations: [ LogoutComponent ],
      providers: [
        { provide: AuthenticationService, useValue: mockAuthenticationService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogoutComponent);
    component = fixture.componentInstance;
    mockAuthenticationService.logout.and.returnValue(of(false));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
