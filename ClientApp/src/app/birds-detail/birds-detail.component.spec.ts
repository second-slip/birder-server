import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsDetailComponent } from './birds-detail.component';

describe('BirdsDetailComponent', () => {
  let component: BirdsDetailComponent;
  let fixture: ComponentFixture<BirdsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BirdsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
