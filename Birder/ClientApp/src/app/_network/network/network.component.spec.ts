import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkComponent } from './network.component';
import { NetworkService } from '@app/_services/network.service';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserNetworkDto } from '@app/_models/UserNetworkDto';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Subscription } from 'rxjs';

describe('NetworkComponent', () => {
  let component: NetworkComponent;
  let fixture: ComponentFixture<NetworkComponent>;

  let mockNetworkService;
  let mockToastr;
  let suggestedUsers: UserNetworkDto;
  let mockError: ErrorReportViewModel;
  let mockSub: Subscription;

  beforeEach(async(() => {
    mockNetworkService = jasmine.createSpyObj(['getUserNetwork', 'postFollowUser', 'postUnfollowUser', 'networkChanged']);
    mockToastr = jasmine.createSpyObj(['info', 'warning', 'error']);

    // const spy = spyOn(mockNetworkService..messagePublished$, 'subscribe')

    mockSub = jasmine.createSpyObj(['subscription']);

    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [ NetworkComponent ],
      providers: [
        { provide: NetworkService, useValue: mockNetworkService },
        { provide: ToastrService, useValue: mockToastr },
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
