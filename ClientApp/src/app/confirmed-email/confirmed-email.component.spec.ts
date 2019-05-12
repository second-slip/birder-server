import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmedEmailComponent } from './confirmed-email.component';

describe('ConfirmedEmailComponent', () => {
  let component: ConfirmedEmailComponent;
  let fixture: ComponentFixture<ConfirmedEmailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmedEmailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmedEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
