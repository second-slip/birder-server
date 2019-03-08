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
              MatProgressSpinnerModule, MAT_DATE_LOCALE} from '@angular/material';
import { LoginComponent } from './login/login.component';
import { JwtHelper } from 'angular2-jwt';
import { AuthGuard } from './auth-guard.service';
import { ReactFormExampleComponent } from './react-form-example/react-form-example.component';
import { LogoutComponent } from './logout/logout.component';
import { BirdsIndexComponent } from './birds-index/birds-index.component';
import { BirdsDetailComponent } from './birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { httpInterceptorProviders } from '../_httpInterceptors';
import { ObservationFeedComponent } from './observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './observation-detail/observation-detail.component';
import { ObservationAddComponent } from './observation-add/observation-add.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SideMenuComponent,
    LoginComponent,
    ReactFormExampleComponent,
    LogoutComponent,
    BirdsIndexComponent,
    BirdsDetailComponent,
    PageNotFoundComponent,
    ObservationFeedComponent,
    ObservationDetailComponent,
    ObservationAddComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    NgbModule,
    AppRoutingModule,
    BrowserAnimationsModule, MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  providers: [
    JwtHelper,
    AuthGuard,
    httpInterceptorProviders,
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
