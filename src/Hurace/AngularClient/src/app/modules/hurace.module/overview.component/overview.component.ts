import { Component, OnInit } from '@angular/core';
import { ApiService, Race, RaceFilter, RaceType, Season } from 'src/app/common/services/api-service.client';
import { Router } from '@angular/router';

@Component({
  selector: 'overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.css']
})
export class OverviewComponent implements OnInit {
  racesLoading: boolean = true;
  raceTypesLoading: boolean = true;
  seasonsLoading: boolean = true;
  races: Race[];
  raceTypes: RaceType[];
  seasons: Season[];
  selectedRaceTypes: RaceType[] = [];
  selectedSeasons: Season[] = [];

  constructor(
    private apiService: ApiService,
    private router: Router) {
  }

  ngOnInit() {
    this.updateRaces();

    this.apiService.getAllRaceTypes()
      .subscribe(raceTypes => {
        this.raceTypes = raceTypes;
        this.raceTypesLoading = false;
      });

    this.apiService.getAllSeasons()
      .subscribe(seasons => {
        this.seasons = seasons;
        this.seasonsLoading = false;
      })
  }

  onRaceTypeClicked(currentRaceType: RaceType): void {
    if (this.selectedRaceTypes.includes(currentRaceType)){
      this.selectedRaceTypes.splice(this.selectedRaceTypes.findIndex(rt => rt.id == currentRaceType.id), 1);
    } else {
      this.selectedRaceTypes.push(currentRaceType);
    }

    this.updateRaces();
  }

  onSeasonClicked(currentSeason: Season): void {
    if (this.selectedSeasons.includes(currentSeason)){
      this.selectedSeasons.splice(
        this.selectedSeasons.findIndex(rt => rt.id == currentSeason.id),
        1
      );
    } else {
      this.selectedSeasons.push(currentSeason);
    }

    this.updateRaces();
  }

  onRaceClicked(currentRace: Race): void{
    if (currentRace.overallRaceState.reference.id !== 4){
      let rankMode: string = currentRace.overallRaceState.reference.id == 3 ? 'live' : 'static';
      this.router.navigateByUrl(`overview/${currentRace.id}/${rankMode}`)
    }
  }

  private updateRaces(): void{
    this.races = [];
    this.racesLoading = true;

    let raceFilter: RaceFilter = {
      raceTypeIds: this.selectedRaceTypes.map(rt => rt.id),
      seasonIds: this.selectedSeasons.map(s => s.id)
    };
  
    this.apiService.getRacesByFilter(raceFilter)
      .subscribe(raceList => {
        this.races = raceList.sort((r1, r2) => {
          let d1 = new Date(r1.date);
          let d2 = new Date(r2.date);
          
          if (d1.getTime() === d2.getTime()){
            return r1.venue.reference.name.localeCompare(r2.venue.reference.name);
          } else {
            return d1.getTime() - d2.getTime();
          }
        });
        this.racesLoading = false;
      });
  }
}
