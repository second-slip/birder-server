import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAddEditNotesComponent } from './view-add-edit-notes.component';

describe('ViewAddEditNotesComponent', () => {
  let component: ViewAddEditNotesComponent;
  let fixture: ComponentFixture<ViewAddEditNotesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewAddEditNotesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAddEditNotesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
