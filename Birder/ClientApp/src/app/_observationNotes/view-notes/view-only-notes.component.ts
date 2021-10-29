import { Component, Input } from '@angular/core';
import { ObservationNote } from '@app/_models';

@Component({
  selector: 'app-view-only-notes',
  templateUrl: './view-only-notes.component.html',
  styleUrls: ['./view-only-notes.component.scss']
})
export class ViewOnlyNotesComponent {
  @Input() notes: ObservationNote[];
}
