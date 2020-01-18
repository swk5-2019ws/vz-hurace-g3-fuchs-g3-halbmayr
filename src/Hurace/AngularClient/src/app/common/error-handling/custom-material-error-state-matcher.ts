import { ErrorStateMatcher } from '@angular/material';
import { FormControl, NgForm, FormGroupDirective } from '@angular/forms';

export class CustomMaterialErrorStateMatcher implements ErrorStateMatcher{
    private isSubmitButtonPressed: boolean = false;

    isErrorState(
        control: FormControl,
        form: FormGroupDirective | NgForm
    ): boolean {
        return (control && control.invalid && (this.isSubmitButtonPressed || control.dirty));
    }

    submitButtonPressed(): void{
        this.isSubmitButtonPressed = true;
    }
}
