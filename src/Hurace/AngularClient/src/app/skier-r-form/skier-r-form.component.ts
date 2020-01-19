import { Skier, ConverterClient, Sex, Country } from './../converter.client';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SkierClass, AssociatedOfCountryClass, AssociatedOfSexClass } from '../shared/classes';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-skier-r-form',
  templateUrl: './skier-r-form.component.html',
  styleUrls: ['./skier-r-form.component.css']
})

export class SkierRFormComponent implements OnInit {

  skier: Skier = new SkierClass();
  sexes: Sex[] = [];
  countries: Country[] = [];
  errors: { [key: string]: string } = {};
  skierForm: FormGroup;
  submitted = false;

  constructor(private route: ActivatedRoute, private converterClient: ConverterClient, private formBuilder: FormBuilder) {

  }

  ngOnInit() {
    this.skierForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      year: ['', Validators.required],
      country: ['', Validators.required],
      sex: ['', Validators.required],
      image: ['', Validators.required]
    });

    this.skier.country = new AssociatedOfCountryClass();
    this.skier.sex = new AssociatedOfSexClass();
    const id = this.route.snapshot.params.id;
    if (id) {
      this.converterClient.getSkierById(id).subscribe(res => {
        this.skier = res;
      });
    }
    this.retrieveData();
  }

  get f() { return this.skierForm.controls; }

  submitForm() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.skierForm.invalid) {
        return;
    }

    this.converterClient.updateSkier(this.skier.id, this.skier);

    alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.skier, null, 4));
    // create skier
    /*this.bs.create(this.book).subscribe(res => {
      this.book = new Book();
      this.myForm.reset(this.book);
    });*/
  }

  /*updateErrorMessages() {
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
  }*/

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
        });
    } else {
        this.sexes = dataSexes;
        this.countries = dataCountries;
    }
  }

  changeCountry(e) {
    this.skier.country.reference = e.target.value;
  }

  changeSex(e) {
    this.skier.sex.reference = e.target.value;
  }
}
