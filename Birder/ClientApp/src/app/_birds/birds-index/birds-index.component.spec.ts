import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsIndexComponent } from './birds-index.component';
import { RouterTestingModule } from '@angular/router/testing';
import { BirdsService } from '@app/_services/birds.service';
import { of } from 'rxjs';

import { HarnessLoader } from '@angular/cdk/testing';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatPaginatorModule } from '@angular/material/paginator';

// let loader: HarnessLoader;

describe('BirdsIndexComponent', () => {
    let component: BirdsIndexComponent;
    let fixture: ComponentFixture<BirdsIndexComponent>;

    let mockBirdsService;

    beforeEach(async(() => {
        mockBirdsService = jasmine.createSpyObj(['getBirds']);

        TestBed.configureTestingModule({
            imports: [MatPaginatorModule, BrowserAnimationsModule

            ],
            declarations: [BirdsIndexComponent],
            providers: [
                { provide: BirdsService, useValue: mockBirdsService }
            ]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(BirdsIndexComponent);
        //loader = TestbedHarnessEnvironment.loader(fixture);
        component = fixture.componentInstance;
        mockBirdsService.getBirds.and.returnValue(of([]));
        fixture.detectChanges();
        //loader = TestbedHarnessEnvironment.loader(fixture);
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
