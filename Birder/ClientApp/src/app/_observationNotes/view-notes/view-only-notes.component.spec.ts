import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOnlyNotesComponent } from './view-only-notes.component';

describe('ViewOnlyNotesComponent', () => {
  let component: ViewOnlyNotesComponent;
  let fixture: ComponentFixture<ViewOnlyNotesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewOnlyNotesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOnlyNotesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
