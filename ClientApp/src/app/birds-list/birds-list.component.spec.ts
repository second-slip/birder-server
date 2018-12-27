import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsListComponent } from './birds-list.component';

describe('BirdsListComponent', () => {
  let component: BirdsListComponent;
  let fixture: ComponentFixture<BirdsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BirdsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
