import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkSearchComponent } from './network-search.component';
import { FormsModule } from '@angular/forms';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { NetworkService } from '@app/_network/network.service';
import { ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';

describe('NetworkSearchComponent', () => {
  let component: NetworkSearchComponent;
  let fixture: ComponentFixture<NetworkSearchComponent>;

  let mockNetworkService;
  let mockToastr;
  let suggestedUsers: NetworkUserViewModel[];
  let mockError: ErrorReportViewModel;

  beforeEach(async(() => {
    mockNetworkService = jasmine.createSpyObj(['getSearchNetwork', 'postFollowUser', 'postUnfollowUser']);
    mockToastr = jasmine.createSpyObj(['info', 'warning', 'error']);

    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [NetworkSearchComponent],
      providers: [
        { provide: NetworkService, useValue: mockNetworkService },
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkSearchComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Search network', () => {

    it('should get network suggestions', () => {
      // Arrange
      suggestedUsers = [
        { userName: '1', avatar: '', isFollowing: false, isOwnProfile: false },
        { userName: '2', avatar: '', isFollowing: false, isOwnProfile: false },
        { userName: '3', avatar: '', isFollowing: false, isOwnProfile: false }
      ]

      let mockFormValue = {
        searchTerm: ''
      }

      mockNetworkService.getSearchNetwork.and.returnValue(of(suggestedUsers));

      // Act or change
      component.searchNetwork(mockFormValue);

      // Assert
      expect(component).toBeTruthy();
      expect(component.users.length).toBe(3);
      expect(component.users[0].userName === '1').toBeTrue();
    });

    it('it should return ErrorReportViewModel and show toastr error', () => {
      // Arrange
      mockError = {
        message: '', 
        type: 'string',
        errorNumber: 404,
        serverCustomMessage: 'Try a different search query',
        friendlyMessage: 'string',
        modelStateErrors: []
      }

      let mockFormValue = {
        searchTerm: ''
      }
      
      mockNetworkService.getSearchNetwork.and.returnValue(throwError(mockError));

      // Act or change
      component.searchNetwork(mockFormValue);

      // Assert
      expect(component).toBeTruthy();
      expect(mockNetworkService.getSearchNetwork).toHaveBeenCalled();
      expect(mockToastr.error).toHaveBeenCalledWith(mockError.serverCustomMessage, 'Search unsuccessful');
    });

  });

});
