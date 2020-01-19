import { Component, OnInit } from '@angular/core';
import { ConverterClient, Season, RaceType, Venue, Race } from '../converter.client';

@Component({
  selector: 'app-seasons',
  templateUrl: './seasons.component.html',
  styleUrls: ['./seasons.component.css']
})
export class SeasonsComponent implements OnInit {

  seasons: Season[] = [];
  raceTypes: RaceType[] = [];
  venues: Venue[] = [];
  races: Race[] = [];

  constructor(private converterClient: ConverterClient) {

  }

  ngOnInit() {

    this.converterClient.getAllSeasons().subscribe(res => {
      this.seasons = res;
      this.seasons = this.seasons.sort((a, b) => a.startDate < b.startDate ? 1 : -1);
    });

    this.converterClient.getAllRaceTypes().subscribe(res => {
      this.raceTypes = res;
    });

    this.converterClient.getAllRaces().subscribe(res => {
      this.races = res;
    });
  }

  convertDate(date: any): Date{
    return new Date(date);
  }

}
