import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AnalysationComponent } from './analysation.component';

describe('AnalysationComponent', () => {
  let component: AnalysationComponent;
  let fixture: ComponentFixture<AnalysationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AnalysationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AnalysationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
