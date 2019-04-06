import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { BirdsIndexComponent } from './birds-index/birds-index.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth-guard.service';
import { LogoutComponent } from './logout/logout.component';
import { BirdsDetailComponent } from './birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ObservationFeedComponent } from './observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './observation-detail/observation-detail.component';
import { ObservationAddComponent } from './observation-add/observation-add.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ObservationEditComponent } from './observation-edit/observation-edit.component';
import { ObservationDeleteComponent } from './observation-delete/observation-delete.component';
import { LifeListComponent } from './life-list/life-list.component';
import { LayoutNoSidebarComponent } from './_layout/layout-no-sidebar/layout-no-sidebar.component';
import { LayoutSidebarComponent } from './_layout/layout-sidebar/layout-sidebar.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutNoSidebarComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'counter', component: CounterComponent },  //
      { path: 'confirm-email', component: ConfirmEmailComponent },
      { path: 'observation-add', component: ObservationAddComponent, canActivate: [AuthGuard] },
      { path: 'observation-edit/:id', component: ObservationEditComponent, canActivate: [AuthGuard] }
    ]
  },
  {
    path: '',
    component: LayoutSidebarComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'observation-feed', component: ObservationFeedComponent },
      { path: 'observation-detail/:id', component: ObservationDetailComponent },
      { path: 'observation-delete/:id', component: ObservationDeleteComponent },
      { path: 'birds-index', component: BirdsIndexComponent },
      { path: 'birds-detail/:id', component: BirdsDetailComponent },
      { path: 'life-list', component: LifeListComponent },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
