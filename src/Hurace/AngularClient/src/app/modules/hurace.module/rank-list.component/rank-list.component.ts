import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { RankedSkier, ApiService } from 'src/app/common/services/api-service.client';

@Component({
  selector: 'rank-list',
  templateUrl: './rank-list.component.html',
  styleUrls: ['./rank-list.component.css']
})
export class RankListComponent implements OnInit {

  rankedSkiersLoading: boolean = true;
  rankedSkiers: RankedSkier[];

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService) { }

  ngOnInit() {
    // this.route.snapshot.paramMap.get('mode');

    let raceId: number = +(this.route.snapshot.paramMap.get('raceId'));
    this.apiService.returns_ranked_skiers_of_specific_race(raceId)
      .subscribe(rankedSkiers => {
        this.rankedSkiersLoading = false;
        this.rankedSkiers = rankedSkiers;
      });
  }

}
