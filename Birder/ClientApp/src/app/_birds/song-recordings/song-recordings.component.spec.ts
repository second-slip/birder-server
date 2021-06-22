import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { RecordingsService } from '../recordings.service';

import { RecordingViewModel } from '@app/_models/RecordingViewModel';

import { SongRecordingsComponent } from './song-recordings.component';

describe('SongRecordingsComponent', () => {
  let component: SongRecordingsComponent;
  let fixture: ComponentFixture<SongRecordingsComponent>;

  let recordingsServiceSpy = jasmine.createSpyObj('RecordingsService', ['getRecordings']);

  recordingsServiceSpy.getRecordings.and.returnValue(of(null));

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SongRecordingsComponent],
      providers: [{ provide: RecordingsService, useValue: recordingsServiceSpy }]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SongRecordingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
