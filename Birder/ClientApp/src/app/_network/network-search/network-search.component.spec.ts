import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkSearchComponent } from './network-search.component';
import { FormsModule } from '@angular/forms';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';

import { NetworkService } from '@app/_network/network.service';

import { of, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';


describe('NetworkSearchComponent', () => {
  let component: NetworkSearchComponent;
  let fixture: ComponentFixture<NetworkSearchComponent>;

  let mockNetworkService;

  let suggestedUsers: NetworkUserViewModel[];
  let mockError: any;
  let mockError1: HttpErrorResponse;

  beforeEach(async(() => {
    mockNetworkService = jasmine.createSpyObj(['getSearchNetwork']);


    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [NetworkSearchComponent],
      providers: [
        { provide: NetworkService, useValue: mockNetworkService }
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
      // expect(component.users.length).toBe(3);
      // expect(component.users[0].userName === '1').toBeTrue();
    });
  });
});
