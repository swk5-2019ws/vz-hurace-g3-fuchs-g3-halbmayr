import { Skier, ConverterClient } from './../converter.client';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SkierFormErrorMessages } from './skier-form-error-messages';

@Component({
  selector: 'app-skier-form',
  templateUrl: './skier-form.component.html',
  styleUrls: ['./skier-form.component.css']
})
export class SkierFormComponent implements OnInit {

  @ViewChild('myForm', {static: true}) myForm: NgForm;
  skier: Skier;
  errors: { [key: string]: string } = {};

  constructor(private converterClient: ConverterClient) {

  }

  ngOnInit() {
    this.myForm.statusChanges.subscribe(() => this.updateErrorMessages());
  }

  submitForm() {

    //create skier
    /*this.bs.create(this.book).subscribe(res => {
      this.book = new Book();
      this.myForm.reset(this.book);
    });*/
  }

  updateErrorMessages() {
    this.errors = {};
    for (const message of SkierFormErrorMessages) {
      const control = this.myForm.form.get(message.forControl);
      if (control &&
          control.dirty &&
          control.invalid &&
          control.errors[message.forValidator] &&
          !this.errors[message.forControl]) {
        this.errors[message.forControl] = message.text;
      }
    }
  }

}
