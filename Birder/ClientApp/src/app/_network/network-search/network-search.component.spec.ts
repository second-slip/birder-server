import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkSearchComponent } from './network-search.component';

describe('NetworkSearchComponent', () => {
  let component: NetworkSearchComponent;
  let fixture: ComponentFixture<NetworkSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NetworkSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
