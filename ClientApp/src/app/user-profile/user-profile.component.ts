import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  constructor(private userService: UserService
    , private route: ActivatedRoute
    , private router: Router) {
    route.params.subscribe(val => {
      this.getUser();
    });
  }

getUser(): void {
  const username = +this.route.snapshot.paramMap.get('username');

  
  }

  ngOnInit() {
  }

}
