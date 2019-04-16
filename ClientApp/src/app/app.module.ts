import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { MatIconModule, MatButtonModule, MatInputModule,
              MatDatepickerModule, MatNativeDateModule, MatCheckboxModule, MatSelectModule,
              MatProgressSpinnerModule, MAT_DATE_LOCALE, MatTooltipModule, MatChipsModule,
              MatAutocompleteModule, MatExpansionModule, MatTabsModule, MatBadgeModule} from '@angular/material';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth-guard.service';
import { LogoutComponent } from './logout/logout.component';
import { BirdsIndexComponent } from './_birds/birds-index/birds-index.component';
import { BirdsDetailComponent } from './_birds/birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { httpInterceptorProviders } from '../_httpInterceptors';
import { ObservationFeedComponent } from './_observations/observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './_observations/observation-detail/observation-detail.component';
import { ObservationAddComponent } from './_observations/observation-add/observation-add.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ObservationEditComponent } from './_observations/observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './_observations/observation-delete/observation-delete.component';
import { JwtModule } from '@auth0/angular-jwt';
import { AgmCoreModule } from '@agm/core';
import { GeocodeService } from './geocode.service';
import { InfoObservationCountComponent } from './_info/info-observation-count/info-observation-count.component';
import { InfoTopObservationsComponent } from './_info/info-top-observations/info-top-observations.component';
import { InfoTweetDayComponent } from './_info/info-tweet-day/info-tweet-day.component';
import { InfoNetworkComponent } from './_info/info-network/info-network.component';
import { LifeListComponent } from './life-list/life-list.component';
import { InfoAwardsComponent } from './_info/info-awards/info-awards.component';
import { LayoutSidebarComponent } from './_layout/layout-sidebar/layout-sidebar.component';
import { LayoutNoSidebarComponent } from './_layout/layout-no-sidebar/layout-no-sidebar.component';
import { UserProfileComponent } from './_users/user-profile/user-profile.component';
import { UserNetworkComponent } from './_users/user-network/user-network.component';
import { ToastrModule } from 'ngx-toastr';
import { AccountManageComponent } from './account-manage/account-manage.component';

export function tokenGetter() {
  return localStorage.getItem('jwt');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
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
    ObservationDeleteComponent,
    InfoObservationCountComponent,
    InfoTopObservationsComponent,
    InfoTweetDayComponent,
    InfoNetworkComponent,
    LifeListComponent,
    InfoAwardsComponent,
    LayoutSidebarComponent,
    LayoutNoSidebarComponent,
    UserProfileComponent,
    UserNetworkComponent,
    AccountManageComponent,
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
    MatChipsModule,
    MatAutocompleteModule,
    MatExpansionModule,
    MatTabsModule,
    MatBadgeModule,
    ToastrModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:55722'],
        blacklistedRoutes: ['http://localhost:55722/Authentication/Login']
      }
    }),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyD4IghqI4x7Sld9KP3sP6FtbN7wCPGySmY'
    })

  ],
  providers: [
    [GeocodeService],
    AuthGuard,
    httpInterceptorProviders,
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: LOCALE_ID, useValue: 'en-GB' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
