import { AbstractControl, ValidatorFn } from "@angular/forms"

// birds list contains objects
// if string is types, an object was not selected...
export function BirdsListValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'notBirdListObject': { value: control.value } }
      }
      return null  /* valid option selected */
    }
  }
  
  // for drop down list of strings
  // function autocompleteStringValidator(validOptions: Array<string>): ValidatorFn {
  //   return (control: AbstractControl): { [key: string]: any } | null => {
  //     if (validOptions.indexOf(control.value) !== -1) {
  //       return null  /* valid option selected */
  //     }
  //     return { 'invalidAutocompleteString': { value: control.value } }
  //   }
  // }