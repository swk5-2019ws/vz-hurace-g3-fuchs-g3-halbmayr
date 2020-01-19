import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { switchMap, startWith } from 'rxjs/operators';
import { RankedSkier, ApiService, Race } from 'src/app/common/services/api-service.client';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'rank-list',
  templateUrl: './rank-list.component.html',
  styleUrls: ['./rank-list.component.css']
})
export class RankListComponent implements OnInit {

  rankedSkiersLoading: boolean = true;
  rankedSkiers: RankedSkier[];

  raceLoading: boolean = true;
  race: Race;

  inLiveMode: boolean;
  intervalSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService) { }

  ngOnInit() {
    let mode = this.route.snapshot.paramMap.get('mode');

    let raceId: number = +(this.route.snapshot.paramMap.get('raceId'));
    this.apiService.getRaceById(raceId)
      .subscribe(race => {
        this.raceLoading = false;
        this.race = race;
      })

    let updateUiAction = (rankedSkiers: RankedSkier[]) => {
      this.rankedSkiersLoading = false;
      this.rankedSkiers = rankedSkiers;
      console.log("updated ranks");
    }

    if (mode === "static"){
      this.inLiveMode = false;

      this.apiService.getRankedSkiersOfRace(raceId)
        .subscribe(updateUiAction);
    } else if (mode === "live"){
      this.inLiveMode = true;

      this.intervalSubscription =
        interval(3000)
          .pipe(
            startWith(0),
            switchMap(() => this.apiService.getRankedSkiersOfRace(raceId))
          )
          .subscribe(updateUiAction)
    } else {
      throw Error(`unknown mode '${mode}' passed`);
    }
  }

  ngOnDestroy() {
    if (this.inLiveMode)
      this.intervalSubscription.unsubscribe();
  }

  generateSequence(length: number): number[]{
    return Array.from({length: length}, (v, i) => i + 1);
  }
}
