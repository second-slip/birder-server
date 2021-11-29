import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationCountComponent } from './observation-count.component';

describe('ObservationCountComponent', () => {
  let component: ObservationCountComponent;
  let fixture: ComponentFixture<ObservationCountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ObservationCountComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationCountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
