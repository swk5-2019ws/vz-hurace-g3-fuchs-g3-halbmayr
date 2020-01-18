import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';


export class CustomValidators {
    private static urlPattern: string = "(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?"
    private static urlRegex = new RegExp(CustomValidators.urlPattern);

    static url(control: AbstractControl): ValidationErrors | null {
        if (control.value && control.value !== ""){
            const urlMatches = CustomValidators.urlRegex.test(control.value);
            return !urlMatches ? {'url': {value: control.value}} : null;
        } else {
            return null;
        }
    }
}