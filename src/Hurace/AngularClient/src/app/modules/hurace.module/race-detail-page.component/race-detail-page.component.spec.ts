import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RaceDetailPageComponent } from './race-detail-page.component';

describe('RaceDetailPageComponent', () => {
  let component: RaceDetailPageComponent;
  let fixture: ComponentFixture<RaceDetailPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RaceDetailPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RaceDetailPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
