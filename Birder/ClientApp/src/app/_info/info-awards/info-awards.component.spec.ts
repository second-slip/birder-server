import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoAwardsComponent } from './info-awards.component';

describe('InfoAwardsComponent', () => {
  let component: InfoAwardsComponent;
  let fixture: ComponentFixture<InfoAwardsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InfoAwardsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoAwardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
