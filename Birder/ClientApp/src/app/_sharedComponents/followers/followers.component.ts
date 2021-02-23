import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';

@Component({
  selector: 'app-followers',
  templateUrl: './followers.component.html',
  styleUrls: ['./followers.component.scss']
})
export class FollowersComponent implements OnInit {
  username: string;
  requesting: boolean;
  followers: NetworkUserViewModel[];

  constructor(private route: ActivatedRoute) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => {
        this.username = pmap.get('username');
        this.getFollowers();
        //this.getUser(pmap.get('username')));
      })
    });
  }



  ngOnInit(): void {
  }

  getFollowers(): void { 
    this.requesting = true;
  this.followers = [];
  this.requesting = false;
  }

}
