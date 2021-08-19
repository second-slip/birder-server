import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkComponent } from './network.component';
import { NetworkService } from '@app/_network/network.service';
import { FormsModule } from '@angular/forms';

import { UserNetworkDto } from '@app/_models/UserNetworkDto';

import { Subscription } from 'rxjs';

describe('NetworkComponent', () => {
  let component: NetworkComponent;
  let fixture: ComponentFixture<NetworkComponent>;

  let mockNetworkService;

  let suggestedUsers: UserNetworkDto;
  let mockError: any;
  let mockSub: Subscription;

  beforeEach(async(() => {
    mockNetworkService = jasmine.createSpyObj(['getUserNetwork', 'postFollowUser', 'postUnfollowUser', 'networkChanged']);


    // const spy = spyOn(mockNetworkService..messagePublished$, 'subscribe')

    mockSub = jasmine.createSpyObj(['networkChangeSubscription']);

    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [ NetworkComponent ],
      providers: [
        { provide: NetworkService, useValue: mockNetworkService },
        
        { provide: Subscription, userValue: mockSub }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkComponent);
    component = fixture.componentInstance;

  });

  it('should create', () => {
    // Can't resolve all parameters for Subscription: (?)
    // mockNetworkService.getSearchNetwork.and.returnValue(of(suggestedUsers));
    // mockSub.
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });
});
