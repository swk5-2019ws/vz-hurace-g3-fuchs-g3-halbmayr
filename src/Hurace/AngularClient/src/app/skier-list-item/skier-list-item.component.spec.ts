import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkierListItemComponent } from './skier-list-item.component';

describe('SkierListItemComponent', () => {
  let component: SkierListItemComponent;
  let fixture: ComponentFixture<SkierListItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkierListItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkierListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
