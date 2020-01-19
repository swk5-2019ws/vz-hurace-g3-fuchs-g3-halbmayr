import { Skier, ConverterClient, Sex, Country, AssociatedOfSex } from './../converter.client';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  registerForm: FormGroup;
  submitted = false;

  constructor(private converterClient: ConverterClient, private formBuilder: FormBuilder) {

  }

  ngOnInit() {
    this.skier.country = new AssociatedOfCountryClass();
    this.skier.sex = new AssociatedOfSexClass();
    this.myForm.statusChanges.subscribe(() => this.updateErrorMessages());
    this.retrieveData();

    this.registerForm = this.formBuilder.group({
      title: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      acceptTerms: [false, Validators.requiredTrue]
    });
  }

  get f() { return this.registerForm.controls; }

  submitForm() {
    /*this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
        return;
    }*/

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
