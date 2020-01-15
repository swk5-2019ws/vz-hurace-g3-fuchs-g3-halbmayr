import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecordIconComponent } from './record-icon.component';

describe('RecordIconComponent', () => {
  let component: RecordIconComponent;
  let fixture: ComponentFixture<RecordIconComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecordIconComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecordIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
