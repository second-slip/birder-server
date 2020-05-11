import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkSuggestionsComponent } from './network-suggestions.component';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { NetworkService } from '@app/_services/network.service';
import { of, throwError } from 'rxjs';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

describe('NetworkSuggestionsComponent', () => {
  let component: NetworkSuggestionsComponent;
  let fixture: ComponentFixture<NetworkSuggestionsComponent>;

  let mockNetworkService;
  let mockToastr;
  let suggestedUsers: NetworkUserViewModel[];
  let mockError: ErrorReportViewModel;
  

  beforeEach(async(() => {
    mockNetworkService = jasmine.createSpyObj(['getSearchNetwork', 'getNetworkSuggestions', 'postFollowUser', 'postUnfollowUser']);
    mockToastr = jasmine.createSpyObj(['info', 'error']);

    TestBed.configureTestingModule({
      imports: [ToastrModule.forRoot(), FormsModule],
      declarations: [ NetworkSuggestionsComponent ],
      providers: [
        { provide: NetworkService, useValue: mockNetworkService },
        { provide: ToastrService, useValue: mockToastr }
       ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkSuggestionsComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('getNetwork on load', () => {

    it('should get suggested users on ngOnInIt', () => {
      // Arrange
      suggestedUsers = [
        { userName: '1', avatar: '', isFollowing: false, isOwnProfile: false },
        { userName: '2', avatar: '', isFollowing: false, isOwnProfile: false },
        { userName: '3', avatar: '', isFollowing: false, isOwnProfile: false }
      ]

      mockNetworkService.getNetworkSuggestions.and.returnValue(of(suggestedUsers));

      // Act or change
      fixture.detectChanges();

      // Assert
      expect(component).toBeTruthy();
      expect(component.users.length).toBe(3);
      expect(component.users[0].userName === '1').toBeTrue();
    });  

  });

  describe('error returned by service', () => {

    it('it should return ErrorReportViewModel and show toastr error', () => {
      // Arrange
      mockError = {
        message: '', 
        type: 'string',
        errorNumber: 404,
        serverCustomMessage: 'custom message',
        friendlyMessage: 'string',
        modelStateErrors: []
      }
      
      mockNetworkService.getNetworkSuggestions.and.returnValue(throwError(mockError));

      // Act or change
      component.getNetwork();

      // Assert
      expect(component).toBeTruthy();
      expect(mockNetworkService.getNetworkSuggestions).toHaveBeenCalled();
      expect(mockToastr.error).toHaveBeenCalledWith(mockError.serverCustomMessage, 'An error occurred');
      expect(component).toBeTruthy();
    });
  })
    
});
