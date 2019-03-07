import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationIndexComponent } from './observation-index.component';

describe('ObservationIndexComponent', () => {
  let component: ObservationIndexComponent;
  let fixture: ComponentFixture<ObservationIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
