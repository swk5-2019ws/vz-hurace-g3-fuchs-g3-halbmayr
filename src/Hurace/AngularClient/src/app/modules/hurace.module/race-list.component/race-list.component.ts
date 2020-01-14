import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiService, Race, RaceFilter, RaceType, Season } from 'src/app/common/services/api-service.client';
import { MatSelectModule } from '@angular/material/select';
import { MatSelectionList } from '@angular/material';

@Component({
  selector: 'race-list',
  templateUrl: './race-list.component.html',
  styleUrls: ['./race-list.component.css']
})
export class RaceListComponent implements OnInit {
  racesLoading: boolean = true;
  raceTypesLoading: boolean = true;
  seasonsLoading: boolean = true;
  races: Race[];
  raceTypes: RaceType[];
  seasons: Season[];
  selectedRaceTypes: RaceType[] = [];
  selectedSeasons: Season[] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit() {
    this.updateRaces();

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

  private updateRaces(): void{
    this.races = [];
    this.racesLoading = true;

    let raceFilter: RaceFilter = {
      raceTypeIds: this.selectedRaceTypes.map(rt => rt.id),
      seasonIds: this.selectedSeasons.map(s => s.id)
    };
  
    this.apiService.race_GetRacesByFilter(raceFilter)
      .subscribe(raceList => {
        this.races = raceList;
        this.racesLoading = false;
      });
  }

  clickRaceType(raceType: RaceType) {
    if (this.selectedRaceTypes.includes(raceType)){
      this.selectedRaceTypes.splice(
        this.selectedRaceTypes.findIndex(rt => rt.id == raceType.id),
        1
      );
    } else {
      this.selectedRaceTypes.push(raceType);
    }

    this.updateRaces();
  }

  clickSeason(season: Season) {
    if (this.selectedSeasons.includes(season)){
      this.selectedSeasons.splice(
        this.selectedSeasons.findIndex(rt => rt.id == season.id),
        1
      );
    } else {
      this.selectedSeasons.push(season);
    }

    this.updateRaces();
  }
}
