import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewEditSingleMarkerMapComponent } from './view-edit-single-marker-map.component';

describe('ViewEditSingleMarkerMapComponent', () => {
  let component: ViewEditSingleMarkerMapComponent;
  let fixture: ComponentFixture<ViewEditSingleMarkerMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewEditSingleMarkerMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewEditSingleMarkerMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
