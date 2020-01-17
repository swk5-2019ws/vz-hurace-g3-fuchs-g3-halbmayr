import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Skier } from 'src/app/common/services/api-service.client';

@Component({
  selector: 'skier-list',
  templateUrl: './skier-list.component.html',
  styleUrls: ['./skier-list.component.css']
})
export class SkierListComponent implements OnInit {

  @Input() skiers: Skier[];
  @Output() editSkierPressed = new EventEmitter<Skier>();
  @Output() deleteSkierPressed = new EventEmitter<Skier>();

  constructor() { }

  ngOnInit() {
  }

  editButtonPressed(clickedSkier: Skier): void{
    this.editSkierPressed.emit(clickedSkier);
  }

  deleteButtonPressed(clickedSkier: Skier): void{
    this.deleteSkierPressed.emit(clickedSkier);
  }
}
