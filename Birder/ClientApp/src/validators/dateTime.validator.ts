import { AbstractControl, ValidatorFn, Validators } from "@angular/forms";

export function DateValid(nameRe: RegExp): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      const forbidden = nameRe.test(control.value);
      return forbidden ? {'invalidDate': {value: control.value}} : null;
    };
}

//This is better as a async Validators
//like this

        //let date= '2011-10-05T14:48:00.000Z';
// const dateParsed = new Date(Date.parse(date))

// if(dateParsed.toISOString() === date && dateParsed.toUTCString() === new Date(d).toUTCString()){
//    return  date;
// } else {
//      throw new BadRequestException('Validation failed'); 
// }