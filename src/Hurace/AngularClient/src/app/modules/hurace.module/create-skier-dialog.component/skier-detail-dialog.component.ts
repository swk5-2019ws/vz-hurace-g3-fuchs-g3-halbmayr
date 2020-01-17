import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Skier } from 'src/app/common/services/api-service.client';

@Component({
  selector: 'skier-detail-dialog',
  templateUrl: './skier-detail-dialog.component.html',
  styleUrls: ['./skier-detail-dialog.component.css']
})
export class SkierDetailDialog implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<SkierDetailDialog>,
    @Inject(MAT_DIALOG_DATA) public skier: Skier
  ) { console.log(skier); }

  ngOnInit() {
  }

}
