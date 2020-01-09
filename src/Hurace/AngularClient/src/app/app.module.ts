import { environment } from './../environments/environment.prod';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ConverterClient, HURACE_SERVICE_BASE_URL } from './converter.client';
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
    ConverterClient,
    {
    provide: HURACE_SERVICE_BASE_URL,
    useValue: environment.huraceServiceBaseUrl
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
