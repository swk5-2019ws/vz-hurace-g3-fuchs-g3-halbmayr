import { ConverterClient, Skier } from './../converter.client';
import { Component, OnInit, EventEmitter } from '@angular/core';
import { debounceTime, distinctUntilChanged, tap, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

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
  constructor(private converterClient: ConverterClient) { }

  ngOnInit() {
    this.converterClient.returns_all_skiers()
      .subscribe(skierList => {
        this.skiers = skierList;
        this.foundSkiers = skierList;
      });

    this.keyup.pipe(
    );
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
}
