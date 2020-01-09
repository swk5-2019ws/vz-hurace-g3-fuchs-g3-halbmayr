import { ApiService, Race} from './common/services/api-service.client';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'AngularClient';
  racesLoading: boolean;
  races: Race[] = [];

  constructor(private converterClient: ApiService) {
    this.racesLoading = true;
  }

  ngOnInit(): void {
    this.converterClient.race_GetAllRaces()
      .subscribe(raceList => {
        this.races = raceList;
        this.racesLoading = false;
      });
  }
}
