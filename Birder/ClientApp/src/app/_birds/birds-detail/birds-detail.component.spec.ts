import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsDetailComponent } from './birds-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { BirdsService } from '@app/_services/birds.service';
import { of } from 'rxjs';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';
import { HarnessLoader } from '@angular/cdk/testing';
import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';
import { Router } from '@angular/router';
import { ActivatedRouteStub } from 'testing/activated-route-stub';

// let loader: HarnessLoader;

describe('BirdsDetailComponent', () => {
  let component: BirdsDetailComponent;
  let fixture: ComponentFixture<BirdsDetailComponent>;
  let router: Router;
  let bird: BirdDetailViewModel;

  let activatedRoute: ActivatedRouteStub;

  let mockBirdsService;

  beforeEach(async(() => {
    mockBirdsService = jasmine.createSpyObj(['getBird', 'getObservations']);

    TestBed.configureTestingModule({
      imports: [ RouterTestingModule.withRoutes([
          // { path: 'login', component: DummyLoginLayoutComponent },
        ])
      ],
      declarations: [ BirdsDetailComponent ],
      providers: [
        { provide: BirdsService, useValue: mockBirdsService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsDetailComponent);

    router = TestBed.inject(Router);
    // loader = TestbedHarnessEnvironment.loader(fixture);
    
    component = fixture.componentInstance;
    mockBirdsService.getBird.and.returnValue(of(null));
    mockBirdsService.getObservations.and.returnValue(of([]));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
