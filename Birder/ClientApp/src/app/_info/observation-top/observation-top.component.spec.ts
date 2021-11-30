import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationTopComponent } from './observation-top.component';

describe('ObservationTopComponent', () => {
  let component: ObservationTopComponent;
  let fixture: ComponentFixture<ObservationTopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ObservationTopComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationTopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
