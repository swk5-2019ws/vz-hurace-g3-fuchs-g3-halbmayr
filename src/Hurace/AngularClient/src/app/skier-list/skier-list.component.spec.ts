import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkierListComponent } from './skier-list.component';

describe('SkierListComponent', () => {
  let component: SkierListComponent;
  let fixture: ComponentFixture<SkierListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkierListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkierListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
