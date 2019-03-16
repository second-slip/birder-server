import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { MatIconModule, MatButtonModule, MatInputModule,
              MatDatepickerModule, MatNativeDateModule, MatCheckboxModule, MatSelectModule,
              MatProgressSpinnerModule, MAT_DATE_LOCALE, MatTooltipModule} from '@angular/material';
import { LoginComponent } from './login/login.component';
// import { JwtHelper } from 'angular2-jwt';
import { AuthGuard } from './auth-guard.service';
import { LogoutComponent } from './logout/logout.component';
import { BirdsIndexComponent } from './birds-index/birds-index.component';
import { BirdsDetailComponent } from './birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { httpInterceptorProviders } from '../_httpInterceptors';
import { ObservationFeedComponent } from './observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './observation-detail/observation-detail.component';
import { ObservationAddComponent } from './observation-add/observation-add.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ObservationEditComponent } from './observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './observation-delete/observation-delete.component';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SideMenuComponent,
    LoginComponent,
    LogoutComponent,
    BirdsIndexComponent,
    BirdsDetailComponent,
    PageNotFoundComponent,
    ObservationFeedComponent,
    ObservationDetailComponent,
    ObservationAddComponent,
    RegisterComponent,
    ConfirmEmailComponent,
    ObservationEditComponent,
    ObservationDeleteComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    MatTooltipModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: function  tokenGetter() {
             return localStorage.getItem('jwt'); },
        whitelistedDomains: ['localhost:55722'],
        blacklistedRoutes: ['http://localhost:55722/Authentication/Login']
      }
    })
  ],
  providers: [
    // JwtHelper,
    // JwtHelperService,
    // JwtModule,
    AuthGuard,
    httpInterceptorProviders,
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
