import { Component, OnInit } from '@angular/core';
import { Skier, ApiService } from 'src/app/common/services/api-service.client';
import { MatGridTileHeaderCssMatStyler } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-adminstration',
  templateUrl: './adminstration.component.html',
  styleUrls: ['./adminstration.component.css']
})
export class AdminstrationComponent implements OnInit {

  skiersLoading: boolean = true;
  skiers: Skier[];
  allSkiers: Skier[];

  constructor(
    private apiService: ApiService,
    private datePipe: DatePipe) { }

  ngOnInit() {
    this.reloadSkiers();
  }

  searchTextChangedHandler(searchText: string): void{
    this.skiers = this.filterSkiers(this.allSkiers, searchText);
  }

  createNewSkierPressedHandler(): void{
    console.log("create new");
  }

  editSkierPressedHandler(selectedSkier: Skier): void{
    console.log(selectedSkier);
  }

  deleteSkierPressedHandler(skier: Skier): void{
    console.log('delete this skier with lastname ' + skier.lastName);
    this.reloadSkiers();
  }

  private reloadSkiers(){
    this.apiService.returns_all_skiers()
      .subscribe(skiers => {
        this.skiersLoading = false;
        this.allSkiers = skiers;
        this.skiers = this.sortSkiers(skiers);
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
