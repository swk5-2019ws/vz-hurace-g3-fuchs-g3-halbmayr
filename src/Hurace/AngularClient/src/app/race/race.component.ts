import { Component, OnInit } from '@angular/core';
import { ConverterClient, Race, RankedSkier } from '../converter.client';
import { ActivatedRoute } from '@angular/router';
import { interval, Subscription } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-race',
  templateUrl: './race.component.html',
  styleUrls: ['./race.component.css']
})
export class RaceComponent implements OnInit {

  race: Race;
  rankedSkiers: RankedSkier[] = [];
  loading: boolean;
  raceActive: boolean;
  sub: Subscription;

  constructor(private converterClient: ConverterClient, private route: ActivatedRoute) {
    this.loading = true;
  }

  ngOnInit() {
    const id = this.route.snapshot.params.id;
    if (id) {
      this.converterClient.getRaceById(id).subscribe(res => {
        this.race = res;

        if (this.race.overallRaceState.reference.id == 3){
          this.raceActive = true;

          this.sub = interval(5000).pipe(
            startWith(0),
            switchMap(() => this.converterClient.getRankedSkiersOfRace(id)))
            .subscribe(res => {
              this.rankedSkiers = res;
              this.loading = false;
            });
        } else {
          this.raceActive = false;
          this.converterClient.getRankedSkiersOfRace(id).subscribe(res => {
            this.rankedSkiers = res;
            this.loading = false;
          });
        }
      });
    }
  }

  ngOnDestroy(){
    if(this.raceActive){
      this.sub.unsubscribe();
    }
  }



  convertDate(date: any): Date {
    return new Date(date);
  }

}
