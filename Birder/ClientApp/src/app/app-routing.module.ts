import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BirdsIndexComponent } from './_birds/birds-index/birds-index.component';
import { AuthGuard } from './_services/auth-guard.service';
import { ObservationFeedComponent } from './_observationFeed/observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './_observations/observation-detail/observation-detail.component';
import { ObservationAddComponent } from './_observations/observation-add/observation-add.component';
import { RegisterComponent } from './_account/register/register.component';
import { ConfirmEmailComponent } from './_account/confirm-email/confirm-email.component';
import { ObservationEditComponent } from './_observations/observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './_observations/observation-delete/observation-delete.component';
import { LifeListComponent } from './_lists/life-list/life-list.component';
import { LayoutNoSidebarComponent } from './_layout/layout-no-sidebar/layout-no-sidebar.component';
import { LayoutSidebarComponent } from './_layout/layout-sidebar/layout-sidebar.component';
import { UserProfileComponent } from './_users/user-profile/user-profile.component';
import { LayoutAccountManagerComponent } from './_layout/layout-account-manager/layout-account-manager.component';
import { AccountManagerProfileComponent } from './_accountManager/account-manager-profile/account-manager-profile.component';
import { AccountManagerLocationComponent } from './_accountManager/account-manager-location/account-manager-location.component';
import { AccountManagerPasswordComponent } from './_accountManager/account-manager-password/account-manager-password.component';
import { ConfirmedEmailComponent } from './_account/confirmed-email/confirmed-email.component';
import { ResetPasswordComponent } from './_account/reset-password/reset-password.component';
import { ForgotPasswordComponent } from './_account/forgot-password/forgot-password.component';
import { ForgotPasswordConfirmationComponent } from './_account/forgot-password-confirmation/forgot-password-confirmation.component';
import { ResetPasswordConfirmationComponent } from './_account/reset-password-confirmation/reset-password-confirmation.component';
import { AccountManagerAvatarComponent } from './_accountManager/account-manager-avatar/account-manager-avatar.component';
import { AboutComponent } from './_home/about/about.component';
import { ObservationManagePhotosComponent } from './_photos/observation-manage-photos/observation-manage-photos.component';
import { TestingComponent } from './testing/testing.component';
import { NetworkComponent } from './_network/network/network.component';
import { WhatsNewComponent } from './whats-new/whats-new.component';
import { TweetArchiveComponent } from './_tweet/tweet-archive/tweet-archive.component';
import { LoginComponent } from './_login-out/login/login.component';
import { LogoutComponent } from './_login-out/logout/logout.component';
import { FollowersComponent } from './_network/followers/followers.component';
import { FollowingComponent } from './_network/following/following.component';
import { BirdDetailComponent } from './_birds/bird-detail/bird-detail.component';
import { HomeComponent } from './_home/home/home.component';
import { DeveloperComponent } from './_home/developer/developer.component';
import { ContactComponent } from './_home/contact/contact.component';
import { TechnologyComponent } from './_home/technology/technology.component';
import { FutureComponent } from './_home/future/future.component';


const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full' },
  {
    path: '',
    component: LayoutNoSidebarComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full' },
      { path: 'about', component: AboutComponent },
      { path: 'contact', component: ContactComponent },
      { path: 'technology', component: TechnologyComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'confirm-email', component: ConfirmEmailComponent },
      { path: 'confirmed-email', component: ConfirmedEmailComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'forgot-password-confirmation', component: ForgotPasswordConfirmationComponent },
      { path: 'reset-password/:code', component: ResetPasswordComponent },
      { path: 'reset-password-confirmation', component: ResetPasswordConfirmationComponent },
      { path: 'whats-new', component: WhatsNewComponent },
      { path: 'tweets-archive', component: TweetArchiveComponent },
      { path: 'developer', component: DeveloperComponent },
      { path: 'future', component: FutureComponent }
    ]
  },
  {
    path: '',
    component: LayoutSidebarComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        canActivateChild: [AuthGuard],
        children: [
          { path: '', component: HomeComponent, pathMatch: 'full' },
          { path: 'observation-feed', component: ObservationFeedComponent },
          { path: 'observation-detail/:id', component: ObservationDetailComponent },
          { path: 'observation-delete/:id', component: ObservationDeleteComponent },
          { path: 'observation-add', component: ObservationAddComponent, },
          { path: 'observation-edit/:id', component: ObservationEditComponent },
          { path: 'observation-manage-photos/:id', component: ObservationManagePhotosComponent },
          { path: 'birds-index', component: BirdsIndexComponent },
          { path: 'bird-detail/:id', component: BirdDetailComponent },
          { path: 'life-list/:username', component: LifeListComponent },
          { path: 'user-profile/:username', component: UserProfileComponent },
          { path: 'followers/:username', component: FollowersComponent },
          { path: 'following/:username', component: FollowingComponent },
          { path: 'network', component: NetworkComponent },
          { path: 'testing', component: TestingComponent },
          { path: 'logout', component: LogoutComponent },
        ]
      },
    ]
  },
  {
    path: '',
    component: LayoutAccountManagerComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        canActivateChild: [AuthGuard],
        children: [
          // { path: '', component: HomeComponent, pathMatch: 'full' },
          { path: 'account-manager-profile', component: AccountManagerProfileComponent },
          { path: 'account-manager-avatar', component: AccountManagerAvatarComponent },
          { path: 'account-manager-location', component: AccountManagerLocationComponent },
          { path: 'account-manager-password', component: AccountManagerPasswordComponent },
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    scrollPositionRestoration: 'top'
  })],
  exports: [RouterModule]
})

export class AppRoutingModule { }
