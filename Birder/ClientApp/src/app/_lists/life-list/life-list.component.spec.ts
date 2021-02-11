import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LifeListComponent } from './life-list.component';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';
import { of } from 'rxjs';

describe('LifeListComponent', () => {
  let component: LifeListComponent;
  let fixture: ComponentFixture<LifeListComponent>;

  let mockObservationsAnalysisService;

  beforeEach(async(() => {
    mockObservationsAnalysisService = jasmine.createSpyObj(['getLifeList', 'getObservationAnalysis']);

    TestBed.configureTestingModule({
      declarations: [ LifeListComponent ],
      providers: [
      { provide: ObservationsAnalysisService, useValue: mockObservationsAnalysisService }
    ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LifeListComponent);
    component = fixture.componentInstance;
    // mockObservationsAnalysisService.getObservationAnalysis.and.returnValue(of(null));
    // mockObservationsAnalysisService.getLifeList.and.returnValue(of(null));
    // mockObservationsAnalysisService.getObservationAnalysis.and.returnValue(of(null));
    // fixture.detectChanges();
  });

  it('should create', () => {
    mockObservationsAnalysisService.getObservationAnalysis.and.returnValue(of(null));
    mockObservationsAnalysisService.getLifeList.and.returnValue(of([]));
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });
});
