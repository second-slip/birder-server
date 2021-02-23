import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';

@Component({
  selector: 'app-following',
  templateUrl: './following.component.html',
  styleUrls: ['./following.component.scss']
})
export class FollowingComponent implements OnInit {
  username: string;
  requesting: boolean;
  following: NetworkUserViewModel[];

  constructor(private route: ActivatedRoute) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => {
        this.username = pmap.get('username');
        this.getFollowing();
        //this.getUser(pmap.get('username')));
      })
    });
  }

  ngOnInit(): void {
  }

  getFollowing(): void {
    
  }

}
