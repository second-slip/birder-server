import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutFeaturesComponent } from './about-features.component';

describe('AboutFeaturesComponent', () => {
  let component: AboutFeaturesComponent;
  let fixture: ComponentFixture<AboutFeaturesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutFeaturesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutFeaturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
