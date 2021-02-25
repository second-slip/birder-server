import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DistributionMapUserComponent } from './distribution-map-user.component';

describe('DistributionMapUserComponent', () => {
  let component: DistributionMapUserComponent;
  let fixture: ComponentFixture<DistributionMapUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DistributionMapUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DistributionMapUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
