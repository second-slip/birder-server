import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutTechnologyComponent } from './about-technology.component';

describe('AboutTechnologyComponent', () => {
  let component: AboutTechnologyComponent;
  let fixture: ComponentFixture<AboutTechnologyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutTechnologyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutTechnologyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
