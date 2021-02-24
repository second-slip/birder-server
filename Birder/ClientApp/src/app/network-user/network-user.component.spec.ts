import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkUserComponent } from './network-user.component';

describe('NetworkUserComponent', () => {
  let component: NetworkUserComponent;
  let fixture: ComponentFixture<NetworkUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NetworkUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
