import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdSelectSpeciesComponent } from './bird-select-species.component';

describe('BirdSelectSpeciesComponent', () => {
  let component: BirdSelectSpeciesComponent;
  let fixture: ComponentFixture<BirdSelectSpeciesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BirdSelectSpeciesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdSelectSpeciesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
