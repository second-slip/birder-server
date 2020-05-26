import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowcaseObservationsListComponent } from './showcase-observations-list.component';

describe('ShowcaseObservationsListComponent', () => {
  let component: ShowcaseObservationsListComponent;
  let fixture: ComponentFixture<ShowcaseObservationsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowcaseObservationsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowcaseObservationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
