// import { async, ComponentFixture, TestBed } from '@angular/core/testing';

// import { NavMenuComponent } from './nav-menu.component';
// import { NO_ERRORS_SCHEMA } from '@angular/core';
// import { AuthenticationService } from '@app/_services/authentication.service';
// import { TokenService } from '@app/_services/token.service';
// import { of, BehaviorSubject } from 'rxjs';
// import { UserViewModel } from '@app/_models/UserViewModel';

// describe('NavMenuComponent', () => {
//   let component: NavMenuComponent;
//   let fixture: ComponentFixture<NavMenuComponent>;
//   let mockAuthenticationService;
//   let mockTokenService;

//   let authenticatedUser: UserViewModel;

//   beforeEach(async(() => {
//     mockAuthenticationService = jasmine.createSpyObj(['isAuthenticated', 'checkIsAuthenticated']);
//     mockTokenService = jasmine.createSpyObj(['getAuthenticatedUserDetails']);

//     TestBed.configureTestingModule({
//       imports: [],
//       declarations: [ NavMenuComponent ],
//       providers: [
//         { provide: AuthenticationService, useValue: mockAuthenticationService },
//         { provide: TokenService, useValue: mockTokenService }
//       ]
//       // ,
//       // schemas: [NO_ERRORS_SCHEMA]
//     })
//     .compileComponents();
//   }));

//   beforeEach(() => {
//     fixture = TestBed.createComponent(NavMenuComponent);
//     component = fixture.componentInstance;
//     //
//     mockAuthenticationService.isAuthenticated.and.returnValue(of(true));
//     // mockAuthenticationService.isAuthenticated.and.returnValue(new BehaviorSubject<boolean>(true).asObservable());
//     mockAuthenticationService.checkIsAuthenticated.and.returnValue(of(true));
//     mockTokenService.getAuthenticatedUserDetails.and.returnValue(of(null));


//     fixture.detectChanges();
//   });

//   // it('should create', () => {
//   //   // fixture.detectChanges();
//   //   expect(component).toBeTruthy();
//   // });
// });
