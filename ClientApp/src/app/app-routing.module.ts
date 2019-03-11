import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BirdsIndexComponent } from './birds-index/birds-index.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth-guard.service';
import { ReactFormExampleComponent } from './react-form-example/react-form-example.component';
import { LogoutComponent } from './logout/logout.component';
import { BirdsDetailComponent } from './birds-detail/birds-detail.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ObservationFeedComponent } from './observation-feed/observation-feed.component';
import { ObservationDetailComponent } from './observation-detail/observation-detail.component';
import { ObservationAddComponent } from './observation-add/observation-add.component';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'logout', component: LogoutComponent},
  { path: 'counter', component: CounterComponent },  //
  { path: 'observation-feed', component: ObservationFeedComponent, canActivate: [AuthGuard] },
  { path: 'observation-add', component: ObservationAddComponent, canActivate: [AuthGuard] },
  { path: 'observation-detail/:id', component: ObservationDetailComponent, canActivate: [AuthGuard] },
  { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard] },  //
  { path: 'birds-index', component: BirdsIndexComponent, canActivate: [AuthGuard] },
  { path: 'birds-detail/:id', component: BirdsDetailComponent, canActivate: [AuthGuard] },
  { path: 'react-form-example', component: ReactFormExampleComponent },
  { path: 'confirm-email', component: ConfirmEmailComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
