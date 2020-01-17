import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'input-search',
  templateUrl: './input-search.component.html',
  styleUrls: ['./input-search.component.css']
})
export class InputSearchComponent implements OnInit {

  searchText: string;

  @Input() placeholder: string;
  @Output() searchTextChanged = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  searchTextChangedEmitter():void{
    this.searchTextChanged.emit(this.searchText);
  }
}
