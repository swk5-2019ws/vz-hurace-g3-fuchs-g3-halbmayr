import { Component, OnInit } from '@angular/core';
import { ApiService, Race, RaceFilter, RaceType, Season } from 'src/app/common/services/api-service.client';

@Component({
  selector: 'race-list',
  templateUrl: './race-list.component.html',
  styleUrls: ['./race-list.component.css']
})
export class RaceListComponent implements OnInit {
  racesLoading: boolean;
  races: Race[] = [];
  raceTypesLoading: boolean;
  raceTypes: RaceType[] = [];
  seasonsLoading: boolean;
  seasons: Season[] = [];

  constructor(private apiService: ApiService) {
    this.racesLoading = true;
    this.raceTypesLoading = true;
    this.seasonsLoading = true;
  }

  ngOnInit() {
    let raceFilter: RaceFilter = {
      raceTypeIds: null,
      seasonIds: null
    };
    this.apiService.race_GetRacesByFilter(raceFilter)
      .subscribe(raceList => {
        this.races = raceList;
        this.racesLoading = false;
      });

    this.apiService.returns_all_race_types()
      .subscribe(raceTypes => {
        this.raceTypes = raceTypes;
        this.raceTypesLoading = false;
      });

    this.apiService.returns_all_seasons()
      .subscribe(seasons => {
        this.seasons = seasons;
        this.seasonsLoading = false;
      })
  }

}
