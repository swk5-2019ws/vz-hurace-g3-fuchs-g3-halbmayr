import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ConverterClient, Race} from '../converter.client';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
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

