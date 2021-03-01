import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdDetailInfoComponent } from './bird-detail-info.component';

describe('BirdDetailInfoComponent', () => {
  let component: BirdDetailInfoComponent;
  let fixture: ComponentFixture<BirdDetailInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BirdDetailInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdDetailInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
