import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutDeveloperComponent } from './about-developer.component';

describe('AboutDeveloperComponent', () => {
  let component: AboutDeveloperComponent;
  let fixture: ComponentFixture<AboutDeveloperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AboutDeveloperComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutDeveloperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
