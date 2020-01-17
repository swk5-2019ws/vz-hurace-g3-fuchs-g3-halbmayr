import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Skier } from 'src/app/common/services/api-service.client';

@Component({
  selector: 'skier-list',
  templateUrl: './skier-list.component.html',
  styleUrls: ['./skier-list.component.css']
})
export class SkierListComponent implements OnInit {

  @Input() skiers: Skier[];
  @Output() skierSelected = new EventEmitter<Skier>();

  constructor() { }

  ngOnInit() {
  }

  skierListItemClicked(clickedSkier: Skier): void{
    this.skierSelected.emit(clickedSkier);
  }

}
