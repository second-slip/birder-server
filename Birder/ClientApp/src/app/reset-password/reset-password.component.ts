import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  code: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    // const id = this.route.snapshot.paramMap.get('code');
    this.code = this.route.snapshot.paramMap.get('code');
  }

}
