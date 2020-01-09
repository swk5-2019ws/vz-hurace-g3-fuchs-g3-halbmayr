import { Component, OnInit } from '@angular/core';
import { ApiService, Race } from 'src/app/common/services/api-service.client';

@Component({
  selector: 'race-list',
  templateUrl: './race-list.component.html',
  styleUrls: ['./race-list.component.css']
})
export class RaceListComponent implements OnInit {
  racesLoading: boolean;
  races: Race[] = [];

  constructor(private apiService: ApiService) {
    this.racesLoading = true;
  }

  ngOnInit() {
    this.apiService.race_GetAllRaces()
      .subscribe(raceList => {
        this.races = raceList;
        this.racesLoading = false;
      });
  }

}
