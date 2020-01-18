import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService, Race } from 'src/app/common/services/api-service.client';

@Component({
  selector: 'race-detail-page',
  templateUrl: './race-detail-page.component.html',
  styleUrls: ['./race-detail-page.component.css']
})
export class RaceDetailPageComponent implements OnInit {

  raceLoading: boolean = true;
  race: Race;

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService) {
  }

  ngOnInit() {
    let raceId: number = +(this.route.snapshot.paramMap.get('raceId'));

    this.apiService.getRaceById(raceId)
      .subscribe(race => {
        this.raceLoading = false;
        this.race = race;
      })
  }

}
