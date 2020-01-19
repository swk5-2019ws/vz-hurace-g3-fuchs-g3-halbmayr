import { Skier, ConverterClient, Sex, Country, AssociatedOfSex } from './../converter.client';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SkierFormErrorMessages } from './skier-form-error-messages';
import { SkierClass, AssociatedOfCountryClass, AssociatedOfSexClass } from '../shared/classes';
import { count } from 'rxjs/operators';

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
    this.skier.country = new AssociatedOfCountryClass();
    this.skier.sex = new AssociatedOfSexClass();
    this.myForm.statusChanges.subscribe(() => this.updateErrorMessages());
    this.retrieveData();
  }

  submitForm() {

    alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.skier, null, 4));
    /*this.converterClient.createSkier(this.skier).subscribe(res => {
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

  retrieveData() {
    let dataSexes = JSON.parse(localStorage.getItem('sexes'));
    let dataCountries = JSON.parse(localStorage.getItem('countries'));
    if (dataSexes === null || dataCountries === null) {
        this.converterClient.getAllSexes().subscribe(sexes => {
            localStorage.setItem('sexes', JSON.stringify(sexes));
            this.sexes = sexes;
        }, error => {
            // handle errors
        });
        this.converterClient.getAllCountries().subscribe(countries => {
          localStorage.setItem('countries', JSON.stringify(countries));
          this.countries = countries;
        })
    } else {
        this.sexes = dataSexes;
        this.countries = dataCountries;
    }
  }

}
