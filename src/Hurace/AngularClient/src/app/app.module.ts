import { environment } from './../environments/environment.prod';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ApiService, HURACE_SERVICE_BASE_URL } from './common/services/api-service.client';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { SpinnerComponent } from './common/components/spinner.component/spinner.component';
import { NavbarComponent } from './common/components/navbar.component/navbar.component';
import { RaceListComponent } from './modules/hurace.module/race-list.component/race-list.component';
import { AppRoutingModule } from './app-routing.module';
import { AdminstrationComponent } from './modules/hurace.module/adminstration.component/adminstration.component';

@NgModule({
  declarations: [
    AppComponent,
    SpinnerComponent,
    NavbarComponent,
    RaceListComponent,
    AdminstrationComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [
    ApiService,
    {
      provide: HURACE_SERVICE_BASE_URL,
      useValue: environment.huraceServiceBaseUrl
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
