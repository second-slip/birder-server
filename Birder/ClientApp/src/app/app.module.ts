import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GoogleMapsModule } from '@angular/google-maps'
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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthGuard } from './_services/auth-guard.service';
import { BirdsIndexComponent } from './_birds/birds-index/birds-index.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { httpInterceptorProviders } from './_httpInterceptors';
import { ObservationFeedComponent } from './_observationFeed/observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './_observations/observation-detail/observation-detail.component';
import { ObservationAddComponent } from './_observations/observation-add/observation-add.component';
import { RegisterComponent } from './_account/register/register.component';
import { ConfirmEmailComponent } from './_account/confirm-email/confirm-email.component';
import { ObservationEditComponent } from './_observations/observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './_observations/observation-delete/observation-delete.component';
import { JwtModule } from '@auth0/angular-jwt';
import { InfoObservationCountComponent } from './_info/info-observation-count/info-observation-count.component';
import { InfoTopObservationsComponent } from './_info/info-top-observations/info-top-observations.component';
import { InfoNetworkComponent } from './_info/info-network/info-network.component';
import { LifeListComponent } from './_lists/life-list/life-list.component';
import { InfoAwardsComponent } from './_info/info-awards/info-awards.component';
import { LayoutSidebarComponent } from './_layout/layout-sidebar/layout-sidebar.component';
import { LayoutNoSidebarComponent } from './_layout/layout-no-sidebar/layout-no-sidebar.component';
import { UserProfileComponent } from './_users/user-profile/user-profile.component';
import { ToastrModule } from 'ngx-toastr';
import { AccountManagerProfileComponent } from './_accountManager/account-manager-profile/account-manager-profile.component';
import { AccountSideMenuComponent } from './_accountManager/account-side-menu/account-side-menu.component';
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
import { ObservationManagePhotosComponent } from './_photos/observation-manage-photos/observation-manage-photos.component';
import { TestingComponent } from './testing/testing.component';
import { AboutContributeComponent } from './_about/about-contribute/about-contribute.component';
import { NgcCookieConsentModule, NgcCookieConsentConfig } from 'ngx-cookieconsent';
import { AuthenticationService } from './_services/authentication.service';
import { TokenService } from './_services/token.service';
import { UserObservationsListComponent } from './_users/user-observations-list/user-observations-list.component';
import { BirdObservationsListComponent } from './_birds/bird-observations-list/bird-observations-list.component';
import { NetworkSearchComponent } from './_network/network-search/network-search.component';
import { NetworkSuggestionsComponent } from './_network/network-suggestions/network-suggestions.component';
import { NetworkComponent } from './_network/network/network.component';
import { BirdsIndexGridViewComponent } from './_birds/birds-index-grid-view/birds-index-grid-view.component';
import { InfoTweetComponent } from './_tweet/info-tweet/info-tweet.component';
import { ViewOnlySingleMarkerMapComponent } from './_maps/view-only-single-marker-map/view-only-single-marker-map.component';
import { ViewEditSingleMarkerMapComponent } from './_maps/view-edit-single-marker-map/view-edit-single-marker-map.component';
import { ViewOnlyNotesComponent } from './_observationNotes/view-notes/view-only-notes.component';
import { AddNotesComponent } from './_observationNotes/add-notes/add-notes.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { AddNoteDialogComponent } from './_observationNotes/add-note-dialog/add-note-dialog.component';
import { EditNoteDialogComponent } from './_observationNotes/edit-note-dialog/edit-note-dialog.component';
import { EditNotesComponent } from './_observationNotes/edit-notes/edit-notes.component';
import { ObservationFeedItemComponent } from './_observationFeed/observation-feed-item/observation-feed-item.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { WhatsNewComponent } from './whats-new/whats-new.component';
import { TweetArchiveComponent } from './_tweet/tweet-archive/tweet-archive.component';
import { NgxMatDatetimePickerModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { NgxMatMomentModule } from '@angular-material-components/moment-adapter';
import { MatStepperModule } from '@angular/material/stepper';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { SongRecordingsComponent } from './_birds/song-recordings/song-recordings.component';
import { LoginComponent } from './_login-out/login/login.component';
import { LogoutComponent } from './_login-out/logout/logout.component';
import { FollowingComponent } from './_network/following/following.component';
import { FollowersComponent } from './_network/followers/followers.component';
import { NetworkUserComponent } from './_network/network-user/network-user.component';
import { UserAchievementsComponent } from './_users/user-achievements/user-achievements.component';
import { DistributionMapUserComponent } from './_maps/distribution-map-user/distribution-map-user.component';
import { ErrorDisplayComponent } from './_error/error-display/error-display.component';
import { BirdImagesComponent } from './_birds/bird-images/bird-images.component';
import { BirdDetailComponent } from './_birds/bird-detail/bird-detail.component';
import { BirdDetailInfoComponent } from './_birds/bird-detail-info/bird-detail-info.component';
import { PhotoDisplayComponent } from './_photos/photo-display/photo-display.component';
import { AboutDeveloperComponent } from './_about/about-developer/about-developer.component';
import { BirdSelectSpeciesComponent } from './_birds/bird-select-species/bird-select-species.component';


// import { YearListComponent } from './_lists/year-list/year-list.component';

const cookieConfig: NgcCookieConsentConfig = {
  'cookie': {
    'domain': environment.cookieDomain
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
    // We use cookies to ensure that we give you the best experience on our website and monitoring traffic. If you continue to use this site we will assume that you are happy with it
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
    InfoNetworkComponent,
    LifeListComponent,
    InfoAwardsComponent,
    LayoutSidebarComponent,
    LayoutNoSidebarComponent,
    UserProfileComponent,
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
    UserObservationsListComponent,
    BirdObservationsListComponent,
    NetworkSearchComponent,
    NetworkSuggestionsComponent,
    NetworkComponent,
    BirdsIndexGridViewComponent,
    InfoTweetComponent,
    ViewOnlySingleMarkerMapComponent,
    ViewEditSingleMarkerMapComponent,
    ViewOnlyNotesComponent,
    AddNotesComponent,
    AddNoteDialogComponent,
    EditNoteDialogComponent,
    EditNotesComponent,
    ObservationFeedItemComponent,
    WhatsNewComponent,
    TweetArchiveComponent,
    SongRecordingsComponent,
    FollowingComponent,
    FollowersComponent,
    NetworkUserComponent,
    UserAchievementsComponent,
    DistributionMapUserComponent,
    ErrorDisplayComponent,
    BirdImagesComponent,
    BirdDetailComponent,
    BirdDetailInfoComponent,
    PhotoDisplayComponent,
    AboutDeveloperComponent,
    BirdSelectSpeciesComponent,
    // YearListComponent
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
    MatInputModule,
    MatTooltipModule,
    MatButtonModule,
    MatDatepickerModule,
    NgxMatDatetimePickerModule,
    NgxMatMomentModule,
    MatStepperModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatAutocompleteModule,
    MatExpansionModule,
    MatTabsModule,
    MatBadgeModule,
    MatAutocompleteModule,
    MatDialogModule,
    MatButtonToggleModule,
    ToastrModule.forRoot(),
    NgxDropzoneModule,
    GalleryModule,

    // LightboxModule,
    GoogleMapsModule,
    NgcCookieConsentModule.forRoot(cookieConfig),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ['localhost:55722', 'birder20210119224819.azurewebsites.net', 'birderweb.com'],
        disallowedRoutes: ['//localhost:55722/Authentication/Login', '//birder20210119224819.azurewebsites.net/Authentication/Login', '//birderweb.com/Authentication/Login'],
      }
    }),
  ],
  providers: [
    [AuthenticationService, TokenService],
    { provide: RequestCache, useClass: RequestCacheWithMap },
    httpInterceptorProviders,
    [AuthGuard],
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: LOCALE_ID, useValue: 'en-GB' },
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { showError: true }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
