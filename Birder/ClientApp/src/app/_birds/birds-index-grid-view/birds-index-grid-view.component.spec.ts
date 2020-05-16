import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsIndexGridViewComponent } from './birds-index-grid-view.component';

describe('BirdsIndexGridViewComponent', () => {
  let component: BirdsIndexGridViewComponent;
  let fixture: ComponentFixture<BirdsIndexGridViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BirdsIndexGridViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsIndexGridViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
