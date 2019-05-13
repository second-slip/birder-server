import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LifeListComponent } from './life-list.component';

describe('LifeListComponent', () => {
  let component: LifeListComponent;
  let fixture: ComponentFixture<LifeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LifeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LifeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
