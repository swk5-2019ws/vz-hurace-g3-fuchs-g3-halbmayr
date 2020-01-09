import { ApiService, Race} from './common/services/api-service.client';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'AngularClient';
  races: Race[] = [];

  constructor(private converterClient: ApiService) {}

  ngOnInit(): void {
    this.converterClient.race_GetAllRaces()
      .subscribe(raceList => {
        this.races = raceList;
      });
  }
}
