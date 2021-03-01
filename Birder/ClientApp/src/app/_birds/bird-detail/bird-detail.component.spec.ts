import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdDetailComponent } from './bird-detail.component';

describe('BirdDetailComponent', () => {
  let component: BirdDetailComponent;
  let fixture: ComponentFixture<BirdDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BirdDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
