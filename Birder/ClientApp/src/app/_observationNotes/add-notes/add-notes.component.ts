import { Component, OnInit } from '@angular/core';
import { ObservationNote } from '@app/_models/ObservationNote';

@Component({
  selector: 'app-add-notes',
  templateUrl: './add-notes.component.html',
  styleUrls: ['./add-notes.component.scss']
})
export class AddNotesComponent implements OnInit {
  notes: ObservationNote[] = [];

  constructor() { }

  ngOnInit(): void {
  }

  addNote(note: ObservationNote): void { 
    this.notes.push(note);
  }

  removeNote(note: ObservationNote): void { 
    const i = this.notes.indexOf(note);
    this.notes.splice(i);
  }




}
