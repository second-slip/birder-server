import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoTopObservationsComponent } from './info-top-observations.component';

describe('InfoTopObservationsComponent', () => {
  let component: InfoTopObservationsComponent;
  let fixture: ComponentFixture<InfoTopObservationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InfoTopObservationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoTopObservationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
