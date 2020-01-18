import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkierFormComponent } from './skier-form.component';

describe('SkierFormComponent', () => {
  let component: SkierFormComponent;
  let fixture: ComponentFixture<SkierFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkierFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkierFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
