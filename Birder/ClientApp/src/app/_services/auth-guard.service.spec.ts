// import { TestBed } from '@angular/core/testing';

// import { AuthGuard } from './auth-guard.service';
// import { RouterTestingModule } from '@angular/router/testing';
// import { HttpClientTestingModule } from '@angular/common/http/testing';
// import { JwtModule } from '@auth0/angular-jwt';
// import { tokenGetter } from '@app/app.module';

// describe('AuthGuardService', () => {
//   beforeEach(() => TestBed.configureTestingModule({
//     imports: [ HttpClientTestingModule,
//       RouterTestingModule.withRoutes([
//         // { path: 'login', component: DummyLoginLayoutComponent }, 
//       ]), JwtModule.forRoot({
//         config: {
//           tokenGetter: tokenGetter,
//           // Exclude this URL from JWT (doesn't add the authentication header)
//           blacklistedRoutes: [
//             '/api/login',
//           ]
//         }
//       }),
//     ],
//     providers: [ AuthGuard ]
//   }));

//   it('should be created', () => {
//     const service: AuthGuard = TestBed.inject(AuthGuard);
//     expect(service).toBeTruthy();
//   });
// });
