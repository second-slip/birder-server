import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOnlySingleMarkerMapComponent } from './view-only-single-marker-map.component';

describe('ViewOnlySingleMarkerMapComponent', () => {
  let component: ViewOnlySingleMarkerMapComponent;
  let fixture: ComponentFixture<ViewOnlySingleMarkerMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewOnlySingleMarkerMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOnlySingleMarkerMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
