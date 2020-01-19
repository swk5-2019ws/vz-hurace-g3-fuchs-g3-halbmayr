import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Skier, Sex, ApiService, Country } from 'src/app/common/services/api-service.client';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { CustomValidators } from 'src/app/common/error-handling/custom-validators';

@Component({
  selector: 'skier-detail-dialog',
  templateUrl: './skier-detail-dialog.component.html',
  styleUrls: ['./skier-detail-dialog.component.css']
})
export class SkierDetailDialog implements OnInit {

  inCreateMode: boolean;
  uploading: boolean = false;

  loadingSexes: boolean = true;
  sexes: Sex[];

  loadingCountries: boolean = true;
  countries: Country[];

  skierForm: FormGroup;

  uploadError: string;

  constructor(
    private dialogRef: MatDialogRef<SkierDetailDialog>,
    @Inject(MAT_DIALOG_DATA) private skier: Skier,
    private apiService: ApiService
  ) {
    this.inCreateMode = !skier;
    if (this.inCreateMode){
      this.skier = {
        country: { reference: { name: null } },
        sex: { reference: { label: null } }
       };
    }

    this.skierForm = new FormGroup({
      'lastName': new FormControl(this.skier.lastName, Validators.required),
      'firstName': new FormControl(this.skier.firstName, Validators.required),
      'imageUrl': new FormControl(this.skier.imageUrl, CustomValidators.url),
      'sex': new FormControl(null, Validators.required),
      'country': new FormControl(null, Validators.required),
      'dateOfBirth': new FormControl(this.skier.dateOfBirth, Validators.required)
    });
  }

  get lastName() { return this.skierForm.get('lastName'); }
  get firstName() { return this.skierForm.get('firstName'); }
  get imageUrl() { return this.skierForm.get('imageUrl'); }
  get sex() { return this.skierForm.get('sex'); }
  get country() { return this.skierForm.get('country'); }
  get dateOfBirth() { return this.skierForm.get('dateOfBirth'); }

  ngOnInit() {
    this.apiService.getAllSexes()
      .subscribe(sexes => {
        this.loadingSexes = false;
        this.sexes = sexes.sort((s1, s2) => {
          return s1.label.localeCompare(s2.label);
        });
    
        if (!this.inCreateMode){
          this.sex.setValue(this.skier.sex.reference.label);
        }
      })

    this.apiService.getAllCountries()
      .subscribe(countries => {
        this.loadingCountries = false;
        this.countries = countries.sort((c1, c2) => {
          return c1.name.localeCompare(c2.name);
        });
    
        if (!this.inCreateMode){
          this.country.setValue(this.skier.country.reference.name);
        }
      });
  }

  generateUniqueImageUrl(): string{
    let lastNameStr = this.skier.lastName 
      ? this.skier.lastName
      : "<lastname>";

    let firstNameStr = this.skier.firstName
      ? this.skier.firstName
      : "<firstname>";
    
    return `https://robohash.org/${lastNameStr}-${firstNameStr}`
  }

  abortDialog(): void{
    console.log("aborted dialog");
    this.dialogRef.close(false);
  }

  createSkier(): void{
    if (this.skierForm.valid) {
      var newSkier = this.loadValuesFromForm();

      this.uploading = true;
      this.apiService.createSkier(newSkier)
        .subscribe(
          createdSkier => {
            this.uploading = false;
            console.log(createdSkier);
            this.dialogRef.close(true);
          },
          errorString => {
            this.uploading = false;
            this.uploadError = errorString;
          });
    }
  }

  editSkier(): void{
    if (this.skierForm.valid) {
      var alteredSkier = this.loadValuesFromForm();

      this.uploading = true;
      this.apiService.updateSkier(this.skier.id, alteredSkier)
        .subscribe(
          _ => {
            this.uploading = false;
            this.dialogRef.close(true);
          },
          errorString => {
            this.uploading = false;
            this.uploadError = errorString;
          });
    }
  }

  private loadValuesFromForm(): Skier{
    let enteredImageUrl = this.skierForm.controls.imageUrl.value;
    let imageUrl = enteredImageUrl
      ? enteredImageUrl
      : this.generateUniqueImageUrl();

    return {
      id: this.skier.id,
      lastName: this.skierForm.controls.lastName.value,
      firstName: this.skierForm.controls.firstName.value,
      dateOfBirth: this.skierForm.controls.dateOfBirth.value,
      imageUrl: imageUrl,
      sex: {
        foreignKey: this.sexes.filter(s => s.label === this.skierForm.controls.sex.value)[0].id
      },
      country: {
        foreignKey: this.countries.filter(c => c.name === this.skierForm.controls.country.value)[0].id
      },
    };
  }
}
