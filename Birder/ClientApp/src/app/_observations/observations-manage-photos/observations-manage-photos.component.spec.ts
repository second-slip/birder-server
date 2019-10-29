import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationsManagePhotosComponent } from './observations-manage-photos.component';

describe('ObservationsManagePhotosComponent', () => {
  let component: ObservationsManagePhotosComponent;
  let fixture: ComponentFixture<ObservationsManagePhotosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationsManagePhotosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationsManagePhotosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
