// import { async, ComponentFixture, TestBed } from '@angular/core/testing';

// import { InfoTopObservationsComponent } from './info-top-observations.component';
// import { ObservationService } from '@app/_services/observation.service';
// import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';

// describe('InfoTopObservationsComponent', () => {
//   let component: InfoTopObservationsComponent;
//   let fixture: ComponentFixture<InfoTopObservationsComponent>;

//   let mockObservationService;
//   let mockObservationsAnalysisService;

//   beforeEach(async(() => {
//     mockObservationService = jasmine.createSpyObj(['observationsChanged']);
//     mockObservationsAnalysisService = jasmine.createSpyObj(['getTopObservationsAnalysis']);

//     TestBed.configureTestingModule({
//       declarations: [ InfoTopObservationsComponent ],
//       providers: [
//         { provide: ObservationService, useValue: mockObservationService },
//         { provide: ObservationsAnalysisService, useValue: mockObservationsAnalysisService }
//       ]
//     })
//     .compileComponents();
//   }));

//   beforeEach(() => {
//     fixture = TestBed.createComponent(InfoTopObservationsComponent);
//     component = fixture.componentInstance;

//     fixture.detectChanges();
//   });

//   it('should create', () => {
//     fixture.detectChanges();
//     expect(component).toBeTruthy();
//   });
// });
