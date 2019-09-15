import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutFeaturesDevelopmentComponent } from './about-features-development.component';

describe('AboutFeaturesDevelopmentComponent', () => {
  let component: AboutFeaturesDevelopmentComponent;
  let fixture: ComponentFixture<AboutFeaturesDevelopmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutFeaturesDevelopmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutFeaturesDevelopmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
