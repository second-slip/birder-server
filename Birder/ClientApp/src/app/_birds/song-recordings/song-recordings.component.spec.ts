import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SongRecordingsComponent } from './song-recordings.component';

describe('SongRecordingsComponent', () => {
  let component: SongRecordingsComponent;
  let fixture: ComponentFixture<SongRecordingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SongRecordingsComponent ]
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
