import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FeaturesComponent } from './features.component';

describe('FeaturesComponent', () => {
  let component: FeaturesComponent;
  let fixture: ComponentFixture<FeaturesComponent>;

  let title: HTMLElement;
  let cssTitle: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeaturesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeaturesComponent);
    component = fixture.componentInstance;
    //fixture.detectChanges();
    title = fixture.nativeElement.querySelector('h4');
    cssTitle = fixture.nativeElement.querySelector('.main-title');
  });

  it('should display title - selected by element', async () => {
    fixture.detectChanges();
    expect(title.textContent).toContain('The Features');
  });

  it('should display title - selected by css', async () => {
    fixture.detectChanges();
    expect(cssTitle.textContent).toContain('The Features');
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
