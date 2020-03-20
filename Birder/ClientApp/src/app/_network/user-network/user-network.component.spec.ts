import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserNetworkComponent } from './user-network.component';
import { NetworkService } from '@app/_services/network.service';
import { of } from 'rxjs';
import { ToastrModule, ToastrService } from 'ngx-toastr';

describe('UserNetworkComponent', () => {
  let component: UserNetworkComponent;
  let fixture: ComponentFixture<UserNetworkComponent>;

  let mockNetworkService;
  let mockToastr;

  beforeEach(async(() => {
    mockNetworkService = jasmine.createSpyObj(['getSearchNetwork', 'getNetworkSuggestions', 'postFollowUser', 'postUnfollowUser']);
    mockToastr = jasmine.createSpyObj(['info', 'error']);

    TestBed.configureTestingModule({
      imports: [ToastrModule.forRoot()],
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
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
