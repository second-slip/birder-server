import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NetworkService } from '../network.service';

import { FollowingComponent } from './following.component';

describe('FollowingComponent', () => {
  let component: FollowingComponent;
  let fixture: ComponentFixture<FollowingComponent>;

  let mockNetworkService: any;

  beforeEach(async () => {
    mockNetworkService = jasmine.createSpyObj(['getFollowing']);

    await TestBed.configureTestingModule({

      imports: [RouterTestingModule.withRoutes([])],
      declarations: [FollowingComponent],
      providers: [{ provide: NetworkService, useValue: mockNetworkService }]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FollowingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
