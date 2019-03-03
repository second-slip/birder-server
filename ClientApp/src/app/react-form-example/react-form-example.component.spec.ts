import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReactFormExampleComponent } from './react-form-example.component';

describe('ReactFormExampleComponent', () => {
  let component: ReactFormExampleComponent;
  let fixture: ComponentFixture<ReactFormExampleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReactFormExampleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReactFormExampleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
