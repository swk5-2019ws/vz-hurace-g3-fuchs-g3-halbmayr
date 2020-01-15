import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OverviewComponent } from './modules/hurace.module/overview.component/overview.component';
import { AdminstrationComponent } from './modules/hurace.module/adminstration.component/adminstration.component';
import { AuthGuard } from './common/services/auth.guard';
import { AnalysationComponent } from './modules/hurace.module/analysation.component/analysation.component';
import { RankListComponent } from './modules/hurace.module/rank-list.component/rank-list.component';

const routes: Routes = [
  { path: 'overview', component: OverviewComponent },
  { path: 'overview/:raceId/:mode', component: RankListComponent },
  { path: 'analysis', component: AnalysationComponent },
  { path: 'administration', component: AdminstrationComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
