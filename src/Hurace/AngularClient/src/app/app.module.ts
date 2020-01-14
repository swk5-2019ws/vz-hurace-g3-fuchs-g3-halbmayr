import { environment } from './../environments/environment.prod';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ApiService, HURACE_SERVICE_BASE_URL } from './common/services/api-service.client';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient} from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppComponent } from './app.component';
import { SpinnerComponent } from './common/components/spinner.component/spinner.component';
import { NavbarComponent } from './common/components/navbar.component/navbar.component';
import { RaceListComponent } from './modules/hurace.module/race-list.component/race-list.component';
import { AppRoutingModule } from './app-routing.module';
import { AdminstrationComponent } from './modules/hurace.module/adminstration.component/adminstration.component';
import { AnalysationComponent } from './modules/hurace.module/analysation.component/analysation.component';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    SpinnerComponent,
    NavbarComponent,
    RaceListComponent,
    AdminstrationComponent,
    AnalysationComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
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
