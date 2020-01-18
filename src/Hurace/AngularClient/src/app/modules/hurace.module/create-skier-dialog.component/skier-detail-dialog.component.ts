import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Skier, Sex, ApiService, Country } from 'src/app/common/services/api-service.client';
import { CustomMaterialErrorStateMatcher } from 'src/app/common/error-handling/custom-material-error-state-matcher';
import { FormControl, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/common/error-handling/custom-validators';

@Component({
  selector: 'skier-detail-dialog',
  templateUrl: './skier-detail-dialog.component.html',
  styleUrls: ['./skier-detail-dialog.component.css']
})
export class SkierDetailDialog implements OnInit {

  inCreateMode: boolean;

  loadingSexes: boolean = true;
  sexes: Sex[];

  loadingCountries: boolean = true;
  countries: Country[];

  allElementsFilled: boolean = true;
  lastNameFilled: boolean = true;
  imageUrlIsValid: boolean = true;
  imageUrlFormControl: FormControl = new FormControl('', [
    CustomValidators.url
  ]);

  errorStateMatcher = new CustomMaterialErrorStateMatcher();

  constructor(
    private dialogRef: MatDialogRef<SkierDetailDialog>,
    @Inject(MAT_DIALOG_DATA) private skier: Skier,
    private apiService: ApiService
  ) {
    this.inCreateMode = !skier;
    if (this.inCreateMode){
      this.skier = { country: {}, sex: {} };
    }
  }

  ngOnInit() {
    this.apiService.returns_all_sexes()
      .subscribe(sexes => {
        this.loadingSexes = false;
        this.sexes = sexes;
      })

      this.apiService.returns_all_countries()
        .subscribe(countries => {
          this.loadingCountries = false;
          this.countries = countries.sort((c1, c2) => {
            return c1.name.localeCompare(c2.name);
          });
        });
  }

  generateUniqueImageUrl(): string{
    return `https://robohash.org/${this.skier.lastName}-${this.skier.firstName}`
  }

  abortDialog(): void{
    console.log("aborted dialog");
    this.dialogRef.close(false);
  }

  createSkier(): void{
    this.errorStateMatcher.submitButtonPressed();

    let formValid = this.updateErrorStates();

    if (formValid) {
      console.log("created skier");
      console.log(this.skier);
      this.dialogRef.close(true);
    }
  }

  editSkier(): void{
    console.log("edited skier");
    console.log(this.skier);
    this.dialogRef.close(true);
  }

  private updateErrorStates(): boolean{
    this.allElementsFilled = false;
    this.lastNameFilled = false;
    this.imageUrlIsValid = false;

    return this.allElementsFilled &&
      this.lastNameFilled &&
      this.imageUrlIsValid
  }
}
