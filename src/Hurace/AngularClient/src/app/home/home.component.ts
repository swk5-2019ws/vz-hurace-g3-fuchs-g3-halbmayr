import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ConverterClient, Race} from '../converter.client';

@Component({
  selector: 'app-home',
  template: `
  <div clas="ui container two column grid">
    <div class="ui container column">
      <h1>Home</h1>
      <p>Hurace</p>
      <div class="card-header bg-info text-center text-white">
      <h1>Race Demo</h1>
    </div>
    <div class="card-body">
      <ul class="races">
        <li *ngFor="let race of races">
          <span class="badge">{{race.id}}</span> {{race.venue.reference.name}}
        </li>
      </ul>
    </div>
    </div>
  </div>
  `,
  styles: []
})
export class HomeComponent implements OnInit {
  races: Race[] = [];

  constructor(private router: Router,
              private route: ActivatedRoute,
              private converterClient: ConverterClient) { }

  ngOnInit(): void {
    this.converterClient.getAllRaces()
      .subscribe(raceList => {
        this.races = raceList;
      });
  }
}

