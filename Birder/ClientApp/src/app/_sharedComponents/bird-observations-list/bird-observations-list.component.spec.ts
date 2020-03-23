import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdObservationsListComponent } from './bird-observations-list.component';

describe('BirdObservationsListComponent', () => {
  let component: BirdObservationsListComponent;
  let fixture: ComponentFixture<BirdObservationsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BirdObservationsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdObservationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
