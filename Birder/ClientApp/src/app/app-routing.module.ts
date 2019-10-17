import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BirdsIndexComponent } from './_birds/birds-index/birds-index.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_services/auth-guard.service';
import { LogoutComponent } from './logout/logout.component';
import { BirdsDetailComponent } from './_birds/birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ObservationFeedComponent } from './_observationsFeed/observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './_observations/observation-detail/observation-detail.component';
import { ObservationAddComponent } from './_observations/observation-add/observation-add.component';
import { RegisterComponent } from './_account/register/register.component';
import { ConfirmEmailComponent } from './_account/confirm-email/confirm-email.component';
import { ObservationEditComponent } from './_observations/observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './_observations/observation-delete/observation-delete.component';
import { LifeListComponent } from './life-list/life-list.component';
import { LayoutNoSidebarComponent } from './_layout/layout-no-sidebar/layout-no-sidebar.component';
import { LayoutSidebarComponent } from './_layout/layout-sidebar/layout-sidebar.component';
import { UserProfileComponent } from './_users/user-profile/user-profile.component';
import { UserNetworkComponent } from './_network/user-network/user-network.component';
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
import { AboutComponent } from './_about/about/about.component';
import { InfititeScrollTestComponent } from './infitite-scroll-test/infitite-scroll-test.component';

const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full' },
  {
    path: '',
    component: LayoutNoSidebarComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full' },
      { path: 'about', component: AboutComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'confirm-email', component: ConfirmEmailComponent },
      { path: 'confirmed-email', component: ConfirmedEmailComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'forgot-password-confirmation', component: ForgotPasswordConfirmationComponent },
      { path: 'reset-password/:code', component: ResetPasswordComponent },
      { path: 'reset-password-confirmation', component: ResetPasswordConfirmationComponent },
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
          { path: 'infinite-scroll-test', component: InfititeScrollTestComponent },
          { path: 'observation-detail/:id', component: ObservationDetailComponent },
          { path: 'observation-delete/:id', component: ObservationDeleteComponent },
          { path: 'observation-add', component: ObservationAddComponent, },
          { path: 'observation-edit/:id', component: ObservationEditComponent },
          { path: 'birds-index', component: BirdsIndexComponent },
          { path: 'birds-detail/:id', component: BirdsDetailComponent },
          { path: 'life-list', component: LifeListComponent },
          { path: 'user-profile/:username', component: UserProfileComponent },
          // { path: 'account-manage', component: AccountManageComponent},
          { path: 'user-network', component: UserNetworkComponent },
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
          { path: 'account-manager-profile', component: AccountManagerProfileComponent},
          { path: 'account-manager-avatar', component: AccountManagerAvatarComponent},
          { path: 'account-manager-location', component: AccountManagerLocationComponent},
          { path: 'account-manager-password', component: AccountManagerPasswordComponent},
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
