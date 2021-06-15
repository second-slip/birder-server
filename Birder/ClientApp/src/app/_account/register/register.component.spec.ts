import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterComponent } from './register.component';
import { FormBuilder } from '@angular/forms';
import { AccountService } from '@app/_account/account.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;

  let mockAccountService;

  beforeEach(async(() => {
    mockAccountService = jasmine.createSpyObj(['checkValidUsername', 'register']);

    TestBed.configureTestingModule({
      imports: [ RouterTestingModule.withRoutes([
          // { path: 'login', component: DummyLoginLayoutComponent },
        ])
      ],
      declarations: [ RegisterComponent ],
      providers: [ FormBuilder,
        { provide: AccountService, useValue: mockAccountService },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    mockAccountService.register.and.returnValue(of(null));
    mockAccountService.checkValidUsername.and.returnValue(of(true));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
