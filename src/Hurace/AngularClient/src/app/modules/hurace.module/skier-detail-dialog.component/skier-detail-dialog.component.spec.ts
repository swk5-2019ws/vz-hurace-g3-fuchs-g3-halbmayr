import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkierDetailDialog } from './skier-detail-dialog.component';

describe('CreateSkierDialogComponent', () => {
  let component: SkierDetailDialog;
  let fixture: ComponentFixture<SkierDetailDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkierDetailDialog ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkierDetailDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
