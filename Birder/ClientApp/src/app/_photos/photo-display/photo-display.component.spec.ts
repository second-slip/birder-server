import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotoDisplayComponent } from './photo-display.component';

describe('PhotoDisplayComponent', () => {
  let component: PhotoDisplayComponent;
  let fixture: ComponentFixture<PhotoDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PhotoDisplayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotoDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
