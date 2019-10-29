import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotosTestComponent } from './photos-test.component';

describe('PhotosTestComponent', () => {
  let component: PhotosTestComponent;
  let fixture: ComponentFixture<PhotosTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotosTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotosTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
