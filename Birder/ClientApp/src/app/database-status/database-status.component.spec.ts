import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { DatabaseStatusComponent } from './database-status.component';
import { DatabaseService } from './database.service';

describe('DatabaseStatusComponent', () => {
  let component: DatabaseStatusComponent;
  let fixture: ComponentFixture<DatabaseStatusComponent>;

  let activeSection: HTMLElement;
  let service: DatabaseService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [DatabaseStatusComponent],
      providers: [DatabaseService]
    })
      .compileComponents();
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });


  describe('success', () => {




    beforeEach(() => {
      fixture = TestBed.createComponent(DatabaseStatusComponent);
      component = fixture.componentInstance;
      //fixture.detectChanges();
      activeSection = fixture.nativeElement.querySelector('.active-icon-colour');

      service = TestBed.inject(DatabaseService);
    });

    it('display h4 title when async() and whenStable()', async () => {
      // async() knows about all the pending promises defined in it's function body.
  
      fixture.detectChanges();
      //expect(el.nativeElement.textContent.trim()).toBe(null);
      //expect(el.nativeElement.textContent.trim()).toBe('Features under development');
      spyOn(service, 'getWakeUpDatabase').and.returnValue(of(true)); // Promise.resolve(true));
  
      //spyOn(featuresService, 'getFeatures').and.throwError('');
  
      //fixture.whenStable().then(() => {
        // This is called when ALL pending promises have been resolved
        fixture.detectChanges();
  
        expect(service.getWakeUpDatabase().pipe()).toBeTruthy();
        // expect(service.getWakeUpDatabase()).toHaveBeenCalled();
        expect(service.getWakeUpDatabase()).toEqual(of(true));
        expect(activeSection).toBeTruthy();
        //expect(el.nativeElement.textContent.trim()).toBe('Features under development');
      //});
  
      //component.ngOnInit();
  
    });


  })


});
