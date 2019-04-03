import { Component, OnInit } from '@angular/core';
import { ObservationService } from '../observation.service';
import { ObservationsAnalysisService } from '../observations-analysis.service';

@Component({
  selector: 'app-info-top-observations',
  templateUrl: './info-top-observations.component.html',
  styleUrls: ['./info-top-observations.component.scss']
})
export class InfoTopObservationsComponent implements OnInit {

  constructor(private observationService: ObservationService
            , private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
  }

}
