import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BirdsListComponent } from './birds-list/birds-list.component';
import { ObservationsFeedComponent } from './observations-feed/observations-feed.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth-guard.service';
import { ReactFormExampleComponent } from './react-form-example/react-form-example.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard] },
  { path: 'birds-list', component: BirdsListComponent },
  { path: 'observations-feed', component: ObservationsFeedComponent },
  { path: 'login', component: LoginComponent},
  { path: 'react-form-example', component: ReactFormExampleComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
