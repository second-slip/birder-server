import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProfileComponent } from './user-profile.component';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { UserProfileService } from '@app/_services/user-profile.service';
import { NetworkService } from '@app/_services/network.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('UserProfileComponent', () => {
  let component: UserProfileComponent;
  let fixture: ComponentFixture<UserProfileComponent>;

  let mockUserProfileService;
  let mockNetworkService;
  let mockToastr;

  beforeEach(async(() => {
    mockUserProfileService = jasmine.createSpyObj(['getUserProfile']);
    mockNetworkService = jasmine.createSpyObj(['postFollowUser', 'postUnfollowUser']);
    mockToastr = jasmine.createSpyObj(['info']);

    TestBed.configureTestingModule({
      imports: [ToastrModule.forRoot(), RouterTestingModule.withRoutes([
        // { path: 'login', component: DummyLoginLayoutComponent },
      ]) ],
      declarations: [UserProfileComponent],
      providers: [
        { provide: UserProfileService, useValue: mockUserProfileService },
        { provide: NetworkService, useValue: mockNetworkService },
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProfileComponent);
    component = fixture.componentInstance;
    mockUserProfileService.getUserProfile.and.returnValue(of(null));
    mockNetworkService.postFollowUser.and.returnValue(of(null));
    mockNetworkService.postUnfollowUser.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
