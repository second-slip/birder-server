import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ShowcaseObservationFeedComponent } from './showcase-observations-list.component';



describe('ShowcaseObservationsListComponent', () => {
  let component: ShowcaseObservationFeedComponent;
  let fixture: ComponentFixture<ShowcaseObservationFeedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowcaseObservationFeedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowcaseObservationFeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
