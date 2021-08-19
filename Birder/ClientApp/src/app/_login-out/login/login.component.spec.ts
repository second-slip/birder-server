import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthenticationService } from '@app/_services/authentication.service';
import { of } from 'rxjs';
import { FormBuilder } from '@angular/forms';


describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  let mockAuthenticationService;


  beforeEach(async(() => {
    mockAuthenticationService = jasmine.createSpyObj(['logout']);


    TestBed.configureTestingModule({
      imports: [ RouterTestingModule.withRoutes([
          // { path: 'login', component: DummyLoginLayoutComponent },
        ])
      ],
      declarations: [ LoginComponent ],
      providers: [ FormBuilder,
        { provide: AuthenticationService, useValue: mockAuthenticationService },

      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    mockAuthenticationService.logout.and.returnValue(of(false));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
