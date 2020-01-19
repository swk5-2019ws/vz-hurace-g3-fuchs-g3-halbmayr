import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkierRFormComponent } from './skier-r-form.component';

describe('SkierRFormComponent', () => {
  let component: SkierRFormComponent;
  let fixture: ComponentFixture<SkierRFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkierRFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkierRFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
