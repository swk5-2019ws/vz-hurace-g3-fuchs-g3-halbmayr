import { Skier } from './../converter.client';
import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'tr.app-skier-list-item',
  templateUrl: './skier-list-item.component.html',
  styleUrls: ['./skier-list-item.component.css']
})
export class SkierListItemComponent implements OnInit {

  date: Date;
  @Input() skier: Skier;

  constructor(public auth: AuthService) { }

  ngOnInit() {
    this.date = new Date(this.skier.dateOfBirth);
  }

}
