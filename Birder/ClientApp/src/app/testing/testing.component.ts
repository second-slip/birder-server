import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ObservationNote, ObservationNoteType } from '@app/_models/ObservationNote';
import { AddNotesComponent, NoteModel } from '@app/_observationNotes/add-notes/add-notes.component';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  // @ViewChild(AddNotesComponent)
  // private notesComponent: AddNotesComponent;

  constructor() { }

  ngOnInit() {
  }

}
