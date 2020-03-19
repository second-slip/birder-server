// import { async, ComponentFixture, TestBed } from '@angular/core/testing';

// import { ObservationDeleteComponent } from './observation-delete.component';
// import { RouterTestingModule } from '@angular/router/testing';
// import { ObservationService } from '@app/_services/observation.service';
// import { ToastrService, ToastrModule } from 'ngx-toastr';
// import { TokenService } from '@app/_services/token.service';
// import { of } from 'rxjs';
// import { ActivatedRoute } from '@angular/router';

// describe('ObservationDeleteComponent', () => {
//   let component: ObservationDeleteComponent;
//   let fixture: ComponentFixture<ObservationDeleteComponent>;

//   let mockObservationService;
//   let mockToastr;
//   let mockTokenService;

//   beforeEach(async(() => {
//     mockObservationService = jasmine.createSpyObj(['getObservation', 'deleteObservation']);

//     TestBed.configureTestingModule({
//       imports: [ ToastrModule.forRoot(),
//         RouterTestingModule.withRoutes([
//           // { path: 'login', component: DummyLoginLayoutComponent },
//         ])
//       ],
//       declarations: [ ObservationDeleteComponent ],
//       providers: [
//         { provide: ObservationService, useValue: mockObservationService },
//         { provide: TokenService, useValue: mockTokenService },
//         { provide: ToastrService, useValue: mockToastr },
//         // { provide: Location, useValue: window.location },
//         // { provide: ActivatedRoute, useValue: { params: of([{id: 1}]) } }
//       ]
//     })
//     .compileComponents();
//   }));

//   beforeEach(() => {
//     fixture = TestBed.createComponent(ObservationDeleteComponent);
//     component = fixture.componentInstance;
//     mockObservationService.getObservation.and.returnValue(of(null));
//     mockObservationService.deleteObservation.and.returnValue(of(null));
//     fixture.detectChanges();
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });
// });
