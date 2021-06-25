import { Component, ViewEncapsulation } from '@angular/core';
import { throwError } from 'rxjs';
import { ContactFormService } from '../contact-form.service';
import { ContactFormModel } from '../ContactFormModel';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ContactFormComponent {
  model: ContactFormModel;
  requesting = false;
  submitted = false;
  public errorObject = null;

  constructor(private service: ContactFormService) {
    this.model = new ContactFormModel('', '', '');
  }

  onReset(): void {
    this.model = new ContactFormModel('', '', '');
  }

  onSubmit(): void {
    this.requesting = true;

    this.service.postMessage(this.model)
      .subscribe(_ => {
        this.submitted = true;
        this.requesting = false;
      },
        (error: any) => {
          this.errorObject = error
          this.requesting = false;
          return throwError(error);
        });
  }
}
