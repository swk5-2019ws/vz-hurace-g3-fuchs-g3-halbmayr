import { environment } from './../environments/environment.prod';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ApiService, HURACE_SERVICE_BASE_URL } from './common/services/api-service.client';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
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
