import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsVoiceComponent } from './birds-voice.component';
import { XenoCantoService } from '@app/_services/xeno-canto.service';
import { IXenoCantoResponse, IRecording } from '@app/_models/IXenoCantoResponse';
import { of, throwError } from 'rxjs';
import { FlickrService } from '@app/_services/flickr.service';

describe('BirdsVoiceComponent', () => {
  let component: BirdsVoiceComponent;
  let fixture: ComponentFixture<BirdsVoiceComponent>;

  let mockXenoCantoService;
  let mockFlickrService;

  let response: IXenoCantoResponse;
  let recordings: IRecording[];

  beforeEach(async(() => {
    mockXenoCantoService = jasmine.createSpyObj(['getRecordings']);
    mockFlickrService = jasmine.createSpyObj(['getPhotoThumnail']);

    TestBed.configureTestingModule({
      declarations: [BirdsVoiceComponent],
      providers: [
        { provide: XenoCantoService, useValue: mockXenoCantoService },
        { provide: FlickrService, useValue: mockFlickrService }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsVoiceComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();

    recordings = [{ id: 1, url: 'string' }, { id: 2, url: 'string' }, { id: 3, url: 'string' }];

    response = {
      numRecordings: 'string', numSpecies: 'string', page: 'string', numPages: 'string',
      recordings: recordings
    };

  });

  it('should create', () => {
    expect(component).toBeTruthy();
    expect(component.error).toBeFalse();
    expect(component.recordings).toBeUndefined();
  });

  it('should load xeno-canto response on ngOnInIt', () => {
    // Arrange -- common arrange steps factored out to beforeEach(()
    mockXenoCantoService.getRecordings.and.returnValue(of(response));

    // Act or change
    fixture.detectChanges();

    // Assert
    expect(component).toBeTruthy();
    expect(component.recordings).toBeTruthy();
    expect(component.recordings.recordings.length).toBe(3);
    expect(mockXenoCantoService.getRecordings).toHaveBeenCalled();
  });

  it('should set error property to true on error response', () => {
    // Arrange -- common arrange steps factored out to beforeEach(()
    mockXenoCantoService.getRecordings.and.returnValue(throwError('error'));

    // Act or change
    fixture.detectChanges();

    // Assert
    expect(mockXenoCantoService.getRecordings).toHaveBeenCalled();
    // expect(mockXenoCantoService.getRecordings).toThrowError();
    expect(component.error).toBeTrue();
  });

});
