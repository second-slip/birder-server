import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BirdsService } from '@app/_services/birds.service';

import { BirdDetailComponent } from './bird-detail.component';

describe('BirdDetailComponent', () => {
  let component: BirdDetailComponent;
  let fixture: ComponentFixture<BirdDetailComponent>;

  let mockBirdsService: any;

  beforeEach(async () => {
    mockBirdsService = jasmine.createSpyObj(['getBird']);
    
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      declarations: [ BirdDetailComponent ],
      providers: [{ provide: BirdsService, useValue: mockBirdsService }]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
