import { ConverterClient, Skier } from './../converter.client';
import { Component, OnInit, EventEmitter } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-skier-list',
  templateUrl: './skier-list.component.html',
  styleUrls: ['./skier-list.component.css']
})
export class SkierListComponent implements OnInit {
  isLoading = false;
  skiers: Skier[] = [];
  foundSkiers: Skier[] = [];
  keyup = new EventEmitter<string>();
  constructor(private converterClient: ConverterClient, public auth: AuthService) { }

  ngOnInit() {
    this.retrieveData();
    /*this.converterClient.returns_all_skiers()
      .subscribe(skierList => {
        this.skiers = skierList;
        this.foundSkiers = skierList;
      });

    this.keyup.pipe(
    );*/
  }

  onEnter(value: string) {
    this.foundSkiers = this.skiers.filter((skier) => {
      const date: Date = new Date(skier.dateOfBirth);
      return skier.country.reference.name.toUpperCase().includes(value.toUpperCase()) ||
             skier.firstName.toUpperCase().includes(value.toUpperCase()) ||
             skier.lastName.toUpperCase().includes(value.toUpperCase()) ||
             date.toLocaleDateString().includes(value);
    });
  }

  retrieveData() {
    let data = JSON.parse(localStorage.getItem('skiers'));
    if (data === null){
        this.converterClient.getAllSkiers().subscribe(skiers => {
            localStorage.setItem('skiers', JSON.stringify(skiers));
            this.skiers = skiers;
            this.skiers.sort((a, b) => a.lastName.localeCompare(b.lastName));
            this.foundSkiers = skiers;
        }, error => {
            // handle errors
        });
    } else {
        this.skiers = data;
        this.foundSkiers = data;
        this.converterClient.getAllSkiers().subscribe(skiers => {
          localStorage.setItem('skiers', JSON.stringify(skiers));
          this.skiers = skiers;
          this.skiers.sort((a, b) => a.lastName.localeCompare(b.lastName));
          this.foundSkiers = skiers;
          }, error => {
          // handle errorss
        });
    }
  }
  getDate(skier: Skier): Date{
    return new Date(skier.dateOfBirth);
  }
}
