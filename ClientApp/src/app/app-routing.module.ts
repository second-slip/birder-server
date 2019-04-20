import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { BirdsIndexComponent } from './_birds/birds-index/birds-index.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth-guard.service';
import { LogoutComponent } from './logout/logout.component';
import { BirdsDetailComponent } from './_birds/birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ObservationFeedComponent } from './_observations/observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './_observations/observation-detail/observation-detail.component';
import { ObservationAddComponent } from './_observations/observation-add/observation-add.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ObservationEditComponent } from './_observations/observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './_observations/observation-delete/observation-delete.component';
import { LifeListComponent } from './life-list/life-list.component';
import { LayoutNoSidebarComponent } from './_layout/layout-no-sidebar/layout-no-sidebar.component';
import { LayoutSidebarComponent } from './_layout/layout-sidebar/layout-sidebar.component';
import { UserProfileComponent } from './_users/user-profile/user-profile.component';
import { UserNetworkComponent } from './_users/user-network/user-network.component';
import { AccountManageComponent } from './account-manage/account-manage.component';
import { LayoutAccountManagerComponent } from './_layout/layout-account-manager/layout-account-manager.component';

const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full' },
  {
    path: '',
    component: LayoutNoSidebarComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'counter', component: CounterComponent },  //
      { path: 'confirm-email', component: ConfirmEmailComponent },
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
          { path: 'account-manage', component: AccountManageComponent},
          // { path: 'user-network', component: UserNetworkComponent },
          // { path: 'logout', component: LogoutComponent },
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
