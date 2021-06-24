import { async, ComponentFixture, discardPeriodicTasks, fakeAsync, flush, TestBed, tick, waitForAsync } from '@angular/core/testing';
import { BirdObservationsListComponent } from './bird-observations-list.component';
// import { ObservationService } from '@app/_sharedServices/observation.service';
// import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { Observable, of, throwError } from 'rxjs';
// import { ObservationDto } from '@app/_models/ObservationFeedDto';
// import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
// import { UserViewModel } from '@app/_models/UserViewModel';
import { ObservationsFetchService } from '@app/_services/observations-fetch.service';
import { ObservationsPagedDto, ObservationViewDto } from '@app/_models/ObservationViewDto';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('BirdObservationsListComponent', () => {
    let component: BirdObservationsListComponent;
    let fixture: ComponentFixture<BirdObservationsListComponent>;

    let getQuoteSpy: jasmine.Spy;

    //let spyObservationsFetchService;

    beforeEach(waitForAsync(() => {
        const observations: ObservationViewDto[] = [{
            observationId: 1, quantity: 1,
            observationDateTime: '2019-05-06T11:32:03.796', creationDate: '2019-05-06T11:32:03.796',
            lastUpdateDate: '2019-05-06T11:32:03.796', birdId: 1, englishName: '', species: '', username: ''
        }, {
            observationId: 1, quantity: 1,
            observationDateTime: '2019-05-06T11:32:03.796', creationDate: '2019-05-06T11:32:03.796',
            lastUpdateDate: '2019-05-06T11:32:03.796', birdId: 1, englishName: '', species: '', username: ''
        }];

        const query: ObservationsPagedDto = { totalItems: observations.length, items: observations };
        const x = jasmine.createSpyObj('ObservationsFetchService', ['getObservationsByBirdSpecies']);

        getQuoteSpy = x.getObservationsByBirdSpecies.and.returnValue(of(query));

        TestBed.configureTestingModule({
            //imports: [HttpClientTestingModule],
            declarations: [BirdObservationsListComponent],
            providers: [
                { provide: ObservationsFetchService, useValue: x }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(BirdObservationsListComponent);
        component = fixture.componentInstance;

        component.birdId = 1;
    });

    describe('X', () => {

        it('should not have observations after construction', () => {
            // Arrange -- common arrange steps factored out to beforeEach(()

            // fixture.detectChanges() runs ngOnInIt()
            // Assert
            expect(component).toBeTruthy();
            expect(component.observations$).toBeUndefined();
            expect(component.observations$).toBeFalsy();
        });

        it('should set errorObject on error response', () => {
            // Arrange
            getQuoteSpy.and.returnValue(throwError('should display error'));

            // Act
            fixture.detectChanges();  // onInit()

            // Assert
            expect(component.getObservations).toThrowError();
            expect(component.errorObject).toMatch('should display error');
            expect(component.errorObject).toBeTruthy();
        });

    });

    describe('Y', () => {

        it('should load observations on ngOnInIt', () => {
            // Arrange -- common arrange steps factored out to beforeEach(()
            // const observations: ObservationViewDto[] = [{
            //     observationId: 1, quantity: 1,
            //     observationDateTime: '2019-05-06T11:32:03.796', creationDate: '2019-05-06T11:32:03.796',
            //     lastUpdateDate: '2019-05-06T11:32:03.796', birdId: 1, englishName: '', species: '', username: ''
            // }, {
            //     observationId: 1, quantity: 1,
            //     observationDateTime: '2019-05-06T11:32:03.796', creationDate: '2019-05-06T11:32:03.796',
            //     lastUpdateDate: '2019-05-06T11:32:03.796', birdId: 1, englishName: '', species: '', username: ''
            // }];

            //spyObservationsFetchService.getObservationsByBirdSpecies.and.returnValue(of(query))
            // const query: ObservationsPagedDto = { totalItems: observations.length, items: observations };

            //getQuoteSpy.getObservationsByBirdSpecies.and.returnValue(of(query));

            // it is false at this point...
            //expect(component.observations$).toBeFalsy();

            // Act or change
            fixture.detectChanges();
            //component.ngOnInit();

            // Assert
            expect(component).toBeTruthy();
            expect(component.observations$).toBeTruthy(); //.toBe(2);
            // expect(component.getObservations).toHaveBeenCalled();
            // expect(component.observations.length).toBe(2);
            // expect(component.observations[0].birdId === 1).toBeTrue();
        });

        // it('should call change page', () => {
        //     // Arrange -- common arrange steps factored out to beforeEach(()
        //     component.birdId = 1;
        //     spyObservationsFetchService.getObservationsByBirdSpecies.and.returnValue(of(query));
        //     // component.birdId = 1;

        //     // Act or change
        //     component.changePage();

        //     // Assert
        //     expect(spyObservationsFetchService.getObservationsByBirdSpecies).toHaveBeenCalled();
        //     // expect(component.observations$.totalItems).toBe(2);
        //     // expect(component.observations$.length).toBe(2);
        //     expect(component.observations$[0].birdId === 1).toBeTrue();
        // });

        // it('should return ErrorReportViewModel if throws 404 error', () => {
        //     const username = 'test';

        //     spyObservationsFetchService.getObservationsByBirdSpecies (username).subscribe(
        //         data => fail('Should have failed with 404 error'),
        //         (error: any) => {
        //             expect(error.errorNumber).toEqual(404);
        //             expect(error.message).toContain('Not Found');
        //             expect(error.type).toContain('unsuccessful response code');
        //             expect(error.friendlyMessage).toContain('An error occurred retrieving data.');
        //         }
        //     );

        //     const req = httpTestingController.expectOne(`api/UserProfile?requestedUsername=${username}`);

        //     // respond with a 404 and the error message in the body
        //     const msg = 'deliberate 404 error';
        //     req.flush(msg, { status: 404, statusText: 'Not Found' });
        // });

        // it('should set errorObject on error response', () => {
        //     // Arrange
        //     const error = new Error('failing me');

        //     //spyObservationsFetchService.getObservationsByBirdSpecies.and.returnValue(of(error));

        //     const errorResponse = new Response('Not Found', {
        //         status: 404,
        //         statusText: 'Not Found',
        //     });

        //     //spyObservationsFetchService.getObservationsByBirdSpecies.and.returnValue(of(errorResponse));
        //     spyObservationsFetchService.getObservationsByBirdSpecies.and.returnValue(of('errorResponse'));
        //     //spyObservationsFetchService.getObservationsByBirdSpecies.and.returnValue(throwError(new Error("Failing HTTP call")));
        //     //spyObservationsFetchService.getObservationsByBirdSpecies.and.returnValue(new Error(''));

        //     // Act (or change)
        //     component.ngOnInit();  // or component.changePage();  or
        //     //fixture.detectChanges();


        //     // Assert
        //     expect(spyObservationsFetchService.getObservationsByBirdSpecies).toHaveBeenCalled();
        //     expect(component.getObservations).toThrowError();
        //     expect(component.errorObject).toBeTruthy();
        //     //expect(spyObservationsFetchService.getObservationsByBirdSpecies).toBe('error');
        // });
    });
});


// function errorMessage(): any {
//     throw new Error('Function not implemented.');
// }

