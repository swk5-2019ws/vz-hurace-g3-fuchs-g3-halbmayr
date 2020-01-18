import { Skier, ConverterClient, Sex, Country } from './../converter.client';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SkierFormErrorMessages } from './skier-form-error-messages';
import { SkierClass } from '../shared/classes';

@Component({
  selector: 'app-skier-form',
  templateUrl: './skier-form.component.html',
  styleUrls: ['./skier-form.component.css']
})
export class SkierFormComponent implements OnInit {

  @ViewChild('myForm', {static: true}) myForm: NgForm;
  skier: Skier = new SkierClass();
  sexes: Sex[] = [];
  countries: Country[] = [];
  errors: { [key: string]: string } = {};

  constructor(private converterClient: ConverterClient) {

  }

  ngOnInit() {
    this.myForm.statusChanges.subscribe(() => this.updateErrorMessages());
  }

  submitForm() {

    this.converterClient.creates_a_new_skier(this.skier).subscribe(res => {
      this.skier = new SkierClass();
      this.myForm.reset(this.skier);
    });
    // create skier
    /*this.bs.create(this.book).subscribe(res => {
      this.book = new Book();
      this.myForm.reset(this.book);
    });*/
  }

  updateErrorMessages() {
    this.errors = {};
    for (const message of SkierFormErrorMessages) {
      const control = this.myForm.form.get(message.forControl);
      if (control &&
          control.dirty &&
          control.invalid &&
          control.errors[message.forValidator] &&
          !this.errors[message.forControl]) {
        this.errors[message.forControl] = message.text;
      }
    }
  }

  // retrieveData() {
  //   let data = JSON.parse(localStorage.getItem('skiers'));
  //   if (data === null){
  //       this.converterClient.returns_all_skiers().subscribe(skiers => {
  //           localStorage.setItem('skiers', JSON.stringify(skiers));
  //           this.skiers = skiers;
  //           this.foundSkiers = skiers;
  //       }, error => {
  //           // handle errors
  //       });
  //   } else {
  //       this.skiers = data;
  //       this.foundSkiers = data;
  //   }
  // }

}
