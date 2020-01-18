import { Component, OnInit } from '@angular/core';
import { Skier, ApiService } from 'src/app/common/services/api-service.client';
import { MatGridTileHeaderCssMatStyler, MatDialog } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';
import { SkierDetailDialog } from '../create-skier-dialog.component/skier-detail-dialog.component';

@Component({
  selector: 'adminstration',
  templateUrl: './adminstration.component.html',
  styleUrls: ['./adminstration.component.css']
})
export class AdminstrationComponent implements OnInit {

  skiersLoading: boolean;
  skiers: Skier[];
  allSkiers: Skier[];

  lastSearchText: string;

  constructor(
    private apiService: ApiService,
    private datePipe: DatePipe,
    private dialogRef: MatDialog) { }

  ngOnInit() {
    this.reloadSkiers();
  }

  searchTextChangedHandler(searchText: string): void{
    this.lastSearchText = searchText;
    this.skiers = this.filterSkiers(this.allSkiers, searchText);
  }

  createNewSkierPressedHandler(): void{
    const dialogRef = this.dialogRef.open(
      SkierDetailDialog,
      {
        width: '650px',
        data: null
      }
    );

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.reloadSkiers();
      }
    });
  }

  editSkierPressedHandler(selectedSkier: Skier): void{
    const dialogRef = this.dialogRef.open(
      SkierDetailDialog,
      {
        width: '650px',
        data: selectedSkier
      }
    );

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.reloadSkiers();
      }
    });
  }

  deleteSkierPressedHandler(skier: Skier): void{
    this.skiersLoading = true;

    this.apiService.deleteSkier(skier.id)
      .subscribe(_ => {
        this.reloadSkiers();
      });
  }

  private reloadSkiers(){
    this.skiersLoading = true;
    this.skiers = [];
    this.allSkiers = [];

    this.apiService.getAllSkiers()
      .subscribe(skiers => {
        this.skiersLoading = false;
        this.allSkiers = skiers;
        this.skiers = this.sortSkiers(skiers);

        if (this.lastSearchText && this.lastSearchText !== ""){
          this.skiers = this.filterSkiers(this.allSkiers, this.lastSearchText);
        }
      });
  }

  private sortSkiers(unsortedSkiers: Skier[]): Skier[]{
    return unsortedSkiers.sort((s1: Skier, s2: Skier) => {
      let lastNameComparison = s1.lastName.localeCompare(s2.lastName);
      return lastNameComparison !== 0
        ? lastNameComparison
        : s1.firstName.localeCompare(s2.firstName);
    });
  }

  private filterSkiers(allSkiers: Skier[], searchText: string): Skier[]{
    return allSkiers.filter((s) => this.skierContainsSearchText(s, searchText));
  }

  private skierContainsSearchText(skier: Skier, searchText: string): boolean{
    let lowerCaseSearchText = searchText.toLocaleLowerCase();

    return skier.country && skier.country.reference && skier.country.reference.name.toLocaleLowerCase().indexOf(lowerCaseSearchText) != -1 ||
      this.datePipe.transform(skier.dateOfBirth, 'dd.MM.yyyy').indexOf(lowerCaseSearchText) != -1 ||
      skier.firstName.toLocaleLowerCase().indexOf(lowerCaseSearchText) != -1 ||
      skier.lastName.toLocaleLowerCase().indexOf(lowerCaseSearchText) != -1 ||
      skier.sex && skier.sex.reference && skier.sex.reference.label.toLocaleLowerCase().indexOf(lowerCaseSearchText) != -1;
  }
}
