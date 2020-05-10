import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserNetworkComponent } from './user-network.component';
import { NetworkService } from '@app/_services/network.service';
import { of } from 'rxjs';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { FormsModule } from '@angular/forms';

describe('UserNetworkComponent', () => {
  let component: UserNetworkComponent;
  let fixture: ComponentFixture<UserNetworkComponent>;

  let mockNetworkService;
  let mockToastr;
  let suggestedUsers: NetworkUserViewModel[];
  let searchResults: NetworkUserViewModel[];

  beforeEach(async(() => {
    mockNetworkService = jasmine.createSpyObj(['getSearchNetwork', 'getNetworkSuggestions', 'postFollowUser', 'postUnfollowUser']);
    mockToastr = jasmine.createSpyObj(['info', 'error']);

    TestBed.configureTestingModule({
      imports: [ToastrModule.forRoot(), FormsModule],
      declarations: [ UserNetworkComponent ],
      providers: [
        { provide: NetworkService, useValue: mockNetworkService },
        { provide: ToastrService, useValue: mockToastr }
       ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserNetworkComponent);
    component = fixture.componentInstance;
    // mockNetworkService.getSearchNetwork.and.returnValue(of(null));
    // mockNetworkService.getNetworkSuggestions.and.returnValue(of(null));
    // mockNetworkService.postFollowUser.and.returnValue(of(null));
    // mockNetworkService.postUnfollowUser.and.returnValue(of(null));
    
    // fixture.detectChanges();
    suggestedUsers = [{ userName: '1', avatar: '', isFollowing: false, isOwnProfile: false },
    { userName: '2', avatar: '', isFollowing: false, isOwnProfile: false },
    { userName: '3', avatar: '', isFollowing: false, isOwnProfile: false }]

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('getNetwork on load', () => {

    it('should get suggested users on ngOnInIt', () => {
      // Arrange
      mockNetworkService.getNetworkSuggestions.and.returnValue(of(suggestedUsers));

      // Act or change
      fixture.detectChanges();

      // Assert
      expect(component).toBeTruthy();
      // expect(component.totalItems).toBe(2);
      expect(component.users.length).toBe(3);
      expect(component.users[0].userName === '1').toBeTrue();
    });


  })


});
