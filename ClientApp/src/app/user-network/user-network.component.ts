import { Component, OnInit } from '@angular/core';
import { NetworkUserViewModel } from '../../_models/UserProfileViewModel';
import { UserService } from '../user.service';
import { ErrorReportViewModel } from 'src/_models/ErrorReportViewModel';

@Component({
  selector: 'app-user-network',
  templateUrl: './user-network.component.html',
  styleUrls: ['./user-network.component.scss']
})
export class UserNetworkComponent implements OnInit {
  users: NetworkUserViewModel[];

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getNetwork();
  }

  honker(element) {
    // alert(element.textContent);
     const action = element.innerText;
    // alert(f);

    if (action === 'Follow') {
      //alert('Follow');
       element.innerText = 'Unfollow';
       return;
    } else {
      //alert('Unfollow');
       element.innerText = 'Follow';
       return;
    }

    // this.users.splice()
  }

  getNetwork(): void {
    // const username = this.route.snapshot.paramMap.get('username');

    this.userService.getNetwork('')
    .subscribe(
      (data: NetworkUserViewModel[]) => {
        this.users = data;
      },
      (error: ErrorReportViewModel) => {
        console.log('bad request');
        // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
      });


  }

}
