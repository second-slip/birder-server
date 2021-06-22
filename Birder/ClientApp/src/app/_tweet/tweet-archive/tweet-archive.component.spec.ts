import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { TweetsService } from '../tweets.service';

import { TweetArchiveComponent } from './tweet-archive.component';

describe('TweetArchiveComponent', () => {
  let component: TweetArchiveComponent;
  let fixture: ComponentFixture<TweetArchiveComponent>;

  let mockTweetsService;

  beforeEach(async () => {
    mockTweetsService = jasmine.createSpyObj('TweetsService', ['getTweetArchive', 'getTweetDay']);

    await TestBed.configureTestingModule({
      declarations: [TweetArchiveComponent],
      providers: [{ provide: TweetsService, useValue: mockTweetsService }]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TweetArchiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    // mockTweetsService.getTweetArchive.and.returnValue(of(null));
    expect(component).toBeTruthy();
  });
});
