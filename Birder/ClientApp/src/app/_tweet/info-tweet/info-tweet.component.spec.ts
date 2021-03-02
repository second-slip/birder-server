import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoTweetComponent } from './info-tweet.component';

describe('InfoTweetComponent', () => {
  let component: InfoTweetComponent;
  let fixture: ComponentFixture<InfoTweetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InfoTweetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoTweetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
