import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkSuggestionsComponent } from './network-suggestions.component';

describe('NetworkSuggestionsComponent', () => {
  let component: NetworkSuggestionsComponent;
  let fixture: ComponentFixture<NetworkSuggestionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NetworkSuggestionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkSuggestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
