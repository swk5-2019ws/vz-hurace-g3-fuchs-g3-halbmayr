import { ConverterClient, Race} from './converter.client';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'AngularClient';
  races: Race[] = [];

  constructor(private converterClient: ConverterClient) {}

  ngOnInit(): void {
    this.converterClient.getAllRaces()
      .subscribe(raceList => {
        this.races = raceList;
      });
  }
}
