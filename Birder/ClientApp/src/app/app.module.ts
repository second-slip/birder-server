import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatListModule } from '@angular/material/list';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_services/auth-guard.service';
import { LogoutComponent } from './logout/logout.component';
import { BirdsIndexComponent } from './_birds/birds-index/birds-index.component';
import { BirdsDetailComponent } from './_birds/birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { httpInterceptorProviders } from './_httpInterceptors';
import { ObservationFeedComponent } from './_observationsFeed/observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './_observations/observation-detail/observation-detail.component';
import { ObservationAddComponent } from './_observations/observation-add/observation-add.component';
import { RegisterComponent } from './_account/register/register.component';
import { ConfirmEmailComponent } from './_account/confirm-email/confirm-email.component';
import { ObservationEditComponent } from './_observations/observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './_observations/observation-delete/observation-delete.component';
import { JwtModule } from '@auth0/angular-jwt';
import { AgmCoreModule } from '@agm/core';
import { GeocodeService } from './_services/geocode.service';
import { InfoObservationCountComponent } from './_info/info-observation-count/info-observation-count.component';
import { InfoTopObservationsComponent } from './_info/info-top-observations/info-top-observations.component';
import { InfoTweetDayComponent } from './_info/info-tweet-day/info-tweet-day.component';
import { InfoNetworkComponent } from './_info/info-network/info-network.component';
import { LifeListComponent } from './life-list/life-list.component';
import { InfoAwardsComponent } from './_info/info-awards/info-awards.component';
import { LayoutSidebarComponent } from './_layout/layout-sidebar/layout-sidebar.component';
import { LayoutNoSidebarComponent } from './_layout/layout-no-sidebar/layout-no-sidebar.component';
import { UserProfileComponent } from './_users/user-profile/user-profile.component';
import { UserNetworkComponent } from './_network/user-network/user-network.component';
import { ToastrModule } from 'ngx-toastr';
import { AccountManagerProfileComponent } from './_accountManager/account-manager-profile/account-manager-profile.component';
import { AccountSideMenuComponent } from './account-side-menu/account-side-menu.component';
import { LayoutAccountManagerComponent } from './_layout/layout-account-manager/layout-account-manager.component';
import { AccountManagerLocationComponent } from './_accountManager/account-manager-location/account-manager-location.component';
import { AccountManagerPasswordComponent } from './_accountManager/account-manager-password/account-manager-password.component';
import { RequestCache, RequestCacheWithMap } from './_services/request-cache.service';
import { ConfirmedEmailComponent } from './_account/confirmed-email/confirmed-email.component';
import { ResetPasswordComponent } from './_account/reset-password/reset-password.component';
import { ForgotPasswordComponent } from './_account/forgot-password/forgot-password.component';
import { ForgotPasswordConfirmationComponent } from './_account/forgot-password-confirmation/forgot-password-confirmation.component';
import { ResetPasswordConfirmationComponent } from './_account/reset-password-confirmation/reset-password-confirmation.component';
import { AccountManagerAvatarComponent } from './_accountManager/account-manager-avatar/account-manager-avatar.component';
import { environment } from '../environments/environment';
import { AboutComponent } from './_about/about/about.component';
import { MatGridListModule } from '@angular/material';
import { AboutFeaturesComponent } from './_about/about-features/about-features.component';
import { AboutTechnologyComponent } from './_about/about-technology/about-technology.component';
import { AboutFeaturesDevelopmentComponent } from './_about/about-features-development/about-features-development.component';
import { AboutAiComponent } from './_about/about-ai/about-ai.component';
import { AboutContactComponent } from './_about/about-contact/about-contact.component';
import { LockedOutComponent } from './_account/locked-out/locked-out.component';
import { ConfirmEmailResendComponent } from './_account/confirm-email-resend/confirm-email-resend.component';
import { FooterComponent } from './footer/footer.component';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { GalleryModule } from '@ngx-gallery/core';
import { LightboxModule } from 'ngx-lightbox';
import { ObservationManagePhotosComponent } from './_observations/observation-manage-photos/observation-manage-photos.component';
import { TestingComponent } from './testing/testing.component';
import { AboutContributeComponent } from './_about/about-contribute/about-contribute.component';
import { NgcCookieConsentModule, NgcCookieConsentConfig} from 'ngx-cookieconsent';

const cookieConfig: NgcCookieConsentConfig = {
  'cookie': {
    'domain': environment.cookieDomain // 'localhost'
  },
  'position': 'bottom',
  'theme': 'classic',
  'palette': {
    'popup': {
      'background': '#000000',
      'text': '#ffffff',
      'link': '#ffffff'
    },
    'button': {
      'background': '#f1d600',
      'text': '#000000',
      'border': 'transparent'
    }
  },
  'type': 'info',
  'content': {
    'message': 'This website uses cookies to ensure you get the best experience on our website.',
    'dismiss': 'Got it!',
    'deny': 'Refuse cookies',
    'link': 'Learn more',
    'href': 'https://cookiesandyou.com',
    'policy': 'Cookie Policy'
  }
};

export function tokenGetter() {
  return localStorage.getItem('jwt');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
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
    AccountManagerProfileComponent,
    AccountSideMenuComponent,
    LayoutAccountManagerComponent,
    AccountManagerLocationComponent,
    AccountManagerPasswordComponent,
    ConfirmedEmailComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    ForgotPasswordConfirmationComponent,
    ResetPasswordConfirmationComponent,
    AccountManagerAvatarComponent,
    AboutComponent,
    AboutFeaturesComponent,
    AboutTechnologyComponent,
    AboutFeaturesDevelopmentComponent,
    AboutAiComponent,
    AboutContactComponent,
    LockedOutComponent,
    ConfirmEmailResendComponent,
    FooterComponent,
    ObservationManagePhotosComponent,
    TestingComponent,
    AboutContributeComponent,
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
    MatPaginatorModule, MatSortModule, MatTableModule, MatGridListModule, MatListModule,
    MatToolbarModule,
    ToastrModule.forRoot(),
    NgxDropzoneModule,
    GalleryModule,
    LightboxModule,
    NgcCookieConsentModule.forRoot(cookieConfig),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:55722'],
        blacklistedRoutes: ['http://localhost:55722/Authentication/Login']
      }
    }),
    AgmCoreModule.forRoot({
      apiKey: environment.mapKey
      // apiKey: 'MY_API_KEY'
    })

  ],
  providers: [
    [GeocodeService],
    { provide: RequestCache, useClass: RequestCacheWithMap },
    httpInterceptorProviders,
    [AuthGuard],
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: LOCALE_ID, useValue: 'en-GB' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
