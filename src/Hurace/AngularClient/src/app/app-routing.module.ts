import { RaceComponent } from './race/race.component';
import { SeasonsComponent } from './seasons/seasons.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from './auth.guard';
import { SkierListComponent } from './skier-list/skier-list.component';
import { SkierFormComponent } from './skier-form/skier-form.component';
import { SkierRFormComponent } from './skier-r-form/skier-r-form.component';


const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'profile',
    component: ProfileComponent
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'skier',
    component: SkierListComponent,
  },
  {
    path: 'admin',
    component: SkierFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'adminR/:id',
    component: SkierRFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'seasons',
    component: SeasonsComponent
  },
  {
    path: 'race/:id',
    component: RaceComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
