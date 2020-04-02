import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-birds-voice',
  templateUrl: './birds-voice.component.html',
  styleUrls: ['./birds-voice.component.scss']
})
export class BirdsVoiceComponent implements OnInit {
  @Input() species: string;

  constructor() { }

  ngOnInit(): void {
  }

}
