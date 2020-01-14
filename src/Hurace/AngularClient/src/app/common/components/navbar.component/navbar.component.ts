import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private translate: TranslateService) { }

  ngOnInit() {
  }

  switchLanguageToGerman(): void{
    this.translate.use('de');
  }

  switchLanguageToEnglish(): void{
    this.translate.use('en');
  }
}
