import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutContributeComponent } from './about-contribute.component';

describe('AboutContributeComponent', () => {
  let component: AboutContributeComponent;
  let fixture: ComponentFixture<AboutContributeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutContributeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutContributeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
