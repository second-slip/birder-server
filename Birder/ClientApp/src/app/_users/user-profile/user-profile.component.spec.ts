import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProfileComponent } from './user-profile.component';

import { UserProfileService } from '@app/_services/user-profile.service';
import { NetworkService } from '@app/_network/network.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('UserProfileComponent', () => {
  let component: UserProfileComponent;
  let fixture: ComponentFixture<UserProfileComponent>;

  let mockUserProfileService;
  let mockNetworkService;


  beforeEach(async(() => {
    mockUserProfileService = jasmine.createSpyObj(['getUserProfile']);
    mockNetworkService = jasmine.createSpyObj(['postFollowUser', 'postUnfollowUser']);


    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([
        // { path: 'login', component: DummyLoginLayoutComponent },
      ]) ],
      declarations: [UserProfileComponent],
      providers: [
        { provide: UserProfileService, useValue: mockUserProfileService },
        { provide: NetworkService, useValue: mockNetworkService },
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProfileComponent);
    component = fixture.componentInstance;
    // mockUserProfileService.getUserProfile.and.returnValue(of(null));
    // mockNetworkService.postFollowUser.and.returnValue(of(true));
    // mockNetworkService.postUnfollowUser.and.returnValue(of(true));
     fixture.detectChanges();
  });

  it('should create', () => {
        // mockNetworkService.postUnfollowUser.and.returnValue(of(true));
        fixture.detectChanges();
    expect(component).toBeTruthy();
  });
});
