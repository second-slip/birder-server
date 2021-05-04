import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StickyMenuComponent } from './sticky-menu.component';

describe('StickyMenuComponent', () => {
  let component: StickyMenuComponent;
  let fixture: ComponentFixture<StickyMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StickyMenuComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StickyMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
