import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { UserProfileViewModel } from '../../../_models/UserProfileViewModel';

@Component({
  selector: 'app-info-network',
  templateUrl: './info-network.component.html',
  styleUrls: ['./info-network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoNetworkComponent implements OnInit {
  user: UserProfileViewModel;

  constructor() { }

  ngOnInit() {
  }

}
