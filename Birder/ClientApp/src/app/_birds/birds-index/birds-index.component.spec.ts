import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsIndexComponent } from './birds-index.component';

describe('BirdsIndexComponent', () => {
  let component: BirdsIndexComponent;
  let fixture: ComponentFixture<BirdsIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BirdsIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
