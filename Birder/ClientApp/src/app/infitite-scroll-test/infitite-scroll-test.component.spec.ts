import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfititeScrollTestComponent } from './infitite-scroll-test.component';

describe('InfititeScrollTestComponent', () => {
  let component: InfititeScrollTestComponent;
  let fixture: ComponentFixture<InfititeScrollTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InfititeScrollTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InfititeScrollTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
