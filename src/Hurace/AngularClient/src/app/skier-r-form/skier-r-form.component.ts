import { Skier, ConverterClient, Sex, Country, AssociatedOfSex } from '../converter.client';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SkierFormErrorMessages } from './skier-r-form-error-messages';
import { SkierClass, AssociatedOfCountryClass, AssociatedOfSexClass } from '../shared/classes';
import { count } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-skier-r-form',
  templateUrl: './skier-r-form.component.html',
  styleUrls: ['./skier-r-form.component.css']
})
export class SkierRFormComponent implements OnInit {

  @ViewChild('myForm', {static: true}) myForm: NgForm;
  skier: Skier = new SkierClass();
  sexes: Sex[] = [];
  countries: Country[] = [];
  errors: { [key: string]: string } = {};

  constructor(private route: ActivatedRoute,
              private router: Router,
              private converterClient: ConverterClient) {

  }

  ngOnInit() {
    this.skier.country = new AssociatedOfCountryClass();
    this.skier.sex = new AssociatedOfSexClass();
    this.myForm.statusChanges.subscribe(() => this.updateErrorMessages());
    this.retrieveData();
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.converterClient.getSkierById(id).subscribe(res => {
        this.skier = res;
      });
    }
  }

  submitForm() {

    this.converterClient.createSkier(this.skier).subscribe(res => {
      this.skier = res;
      this.myForm.reset(this.skier);
    });

    alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.skier, null, 4));
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
