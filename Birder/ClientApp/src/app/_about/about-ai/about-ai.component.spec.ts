import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutAiComponent } from './about-ai.component';

describe('AboutAiComponent', () => {
  let component: AboutAiComponent;
  let fixture: ComponentFixture<AboutAiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutAiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutAiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
