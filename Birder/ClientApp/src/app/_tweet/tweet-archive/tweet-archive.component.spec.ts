import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TweetArchiveComponent } from './tweet-archive.component';

describe('TweetArchiveComponent', () => {
  let component: TweetArchiveComponent;
  let fixture: ComponentFixture<TweetArchiveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TweetArchiveComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TweetArchiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
