import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DebugElement } from '@angular/core';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { Observable, of } from 'rxjs';
import { FeaturesService } from './features.service';
import { IFeatures, WhatsNewComponent } from './whats-new.component';

describe('WhatsNewComponent', () => {
  let component: WhatsNewComponent;
  let fixture: ComponentFixture<WhatsNewComponent>;

  let featuresService: FeaturesService;
  let el: DebugElement;
  let r: DebugElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [WhatsNewComponent],
      providers: [FeaturesService]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WhatsNewComponent);
    component = fixture.componentInstance;

    // UserService provided to the TestBed
    featuresService = TestBed.get(FeaturesService);

    //  get the "a" element by CSS selector (e.g., by class name)
    el = fixture.debugElement.query(By.css('title'));
    r = fixture.debugElement.query(By.css('error-message'));

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should return error', fakeAsync (() => {

    fixture.detectChanges();

    spyOn(featuresService, 'getFeatures').and.throwError('error');


    fixture.whenStable().then(() => {
      // This is called when ALL pending promises have been resolved
      fixture.detectChanges();
      expect(() => featuresService.getFeatures()).toThrowError(Error);
      fixture.detectChanges();
      //expect(r.nativeElement.textContent.trim()).toBe('error');

    });

    tick();
    //component.
    fixture.detectChanges();
    //expect(r.nativeElement.textContent.trim()).toBe('error');

    // expect(component.errorObject).toBeTruthy();
    // expect(component.errorObject).toBe('chukka');

  }));

  it('display h4 title when async() and whenStable()', async () => {
    // async() knows about all the pending promises defined in it's function body.

    const features: IFeatures[] = [{
      id: 1,
      feature: 'string',
      description: 'string',
      progress: 'string',
      priority: 'string',
      colourCode: 'string'
    }, {
      id: 1,
      feature: 'string',
      description: 'string',
      progress: 'string',
      priority: 'string',
      colourCode: 'string'
    }];

    fixture.detectChanges();
    //expect(el.nativeElement.textContent.trim()).toBe(null);
    //expect(el.nativeElement.textContent.trim()).toBe('Features under development');
    spyOn(featuresService, 'getFeatures').and.returnValue(of(features)) // Promise.resolve(true));

    //spyOn(featuresService, 'getFeatures').and.throwError('');

    fixture.whenStable().then(() => {
      // This is called when ALL pending promises have been resolved
      fixture.detectChanges();

      expect(featuresService.getFeatures().pipe()).toBeTruthy();
      expect(featuresService.getFeatures()).toHaveBeenCalled();
      expect(featuresService.getFeatures()).toEqual(of(features));
      expect(el.nativeElement.textContent.trim()).toBe('Features under development');
    });

    //component.ngOnInit();

  });

});
