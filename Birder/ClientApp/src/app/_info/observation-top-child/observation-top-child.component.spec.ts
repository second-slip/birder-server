import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationTopChildComponent } from './observation-top-child.component';

describe('ObservationTopChildComponent', () => {
  let component: ObservationTopChildComponent;
  let fixture: ComponentFixture<ObservationTopChildComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ObservationTopChildComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationTopChildComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
