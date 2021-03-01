import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdImagesComponent } from './bird-images.component';

describe('BirdImagesComponent', () => {
  let component: BirdImagesComponent;
  let fixture: ComponentFixture<BirdImagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BirdImagesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdImagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
