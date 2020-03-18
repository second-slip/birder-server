import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdsIndexComponent } from './birds-index.component';
import { RouterTestingModule } from '@angular/router/testing';
import { BirdsService } from '@app/_services/birds.service';
import { of } from 'rxjs';

describe('BirdsIndexComponent', () => {
  let component: BirdsIndexComponent;
  let fixture: ComponentFixture<BirdsIndexComponent>;

  let mockBirdsService;

  beforeEach(async(() => {
    mockBirdsService = jasmine.createSpyObj(['getBirds']);

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
          // { path: 'login', component: DummyLoginLayoutComponent },
        ])
      ],
      declarations: [ BirdsIndexComponent ],
      providers: [
        { provide: BirdsService, useValue: mockBirdsService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsIndexComponent);
    component = fixture.componentInstance;
    mockBirdsService.getBirds.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
