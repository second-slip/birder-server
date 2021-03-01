import { Component, Input, ViewEncapsulation } from '@angular/core';
import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';

@Component({
  selector: 'app-bird-detail-info',
  templateUrl: './bird-detail-info.component.html',
  styleUrls: ['./bird-detail-info.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdDetailInfoComponent {
  @Input() bird: BirdDetailViewModel;

  constructor() { }

}
