import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsVoiceComponent } from './birds-voice.component';

describe('BirdsVoiceComponent', () => {
  let component: BirdsVoiceComponent;
  let fixture: ComponentFixture<BirdsVoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BirdsVoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsVoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
