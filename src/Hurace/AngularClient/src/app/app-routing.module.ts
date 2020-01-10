import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RaceListComponent } from './modules/hurace.module/race-list.component/race-list.component';
import { AdminstrationComponent } from './modules/hurace.module/adminstration.component/adminstration.component';
import { AuthGuard } from './common/services/auth.guard';

const routes: Routes = [
  { path: 'overview', component: RaceListComponent },
  { path: 'analysis', redirectTo: 'overview' },
  { path: 'administration', component: AdminstrationComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
