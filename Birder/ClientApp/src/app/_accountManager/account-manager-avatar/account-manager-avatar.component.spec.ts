import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountManagerAvatarComponent } from './account-manager-avatar.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule, ToastrService } from 'ngx-toastr';

import { of } from 'rxjs';

describe('AccountManagerAvatarComponent', () => {
  let component: AccountManagerAvatarComponent;
  let fixture: ComponentFixture<AccountManagerAvatarComponent>;

  let mockAccountManagerService;
  let mockToastr;

  beforeEach(async(() => {
    mockAccountManagerService = jasmine.createSpyObj(['postAvatar']);
    mockToastr = jasmine.createSpyObj(['success']);

    TestBed.configureTestingModule({
      imports: [ ToastrModule.forRoot(),
        RouterTestingModule.withRoutes([
          // { path: 'login', component: DummyLoginLayoutComponent },
        ])
      ],
      declarations: [ AccountManagerAvatarComponent ],
      providers: [
        // { provide: AccountManagerService, useValue: mockAccountManagerService },
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountManagerAvatarComponent);
    component = fixture.componentInstance;
    mockAccountManagerService.postAvatar.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
