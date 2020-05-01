import { AbstractControl, ValidatorFn } from '@angular/forms';

export function RestrictedNameValidator(nameRe: RegExp): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      const forbidden = nameRe.test(control.value);
      return forbidden ? {'restrictedName': {value: control.value}} : null;
    };
  }