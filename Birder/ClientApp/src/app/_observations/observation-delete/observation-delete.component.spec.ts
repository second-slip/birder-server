import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationDeleteComponent } from './observation-delete.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ObservationService } from '@app/_services/observation.service';

describe('ObservationDeleteComponent', () => {
  let component: ObservationDeleteComponent;
  let fixture: ComponentFixture<ObservationDeleteComponent>;

  let mockObservationService;

  beforeEach(async(() => {
    mockObservationService = jasmine.createSpyObj(['getObservation', 'deleteObservation']);

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
          // { path: 'login', component: DummyLoginLayoutComponent },
        ])
      ],
      declarations: [ ObservationDeleteComponent ],
      providers: [
        { provide: ObservationService, useValue: mockObservationService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
