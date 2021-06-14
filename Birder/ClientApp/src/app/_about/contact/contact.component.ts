import { Component, ViewEncapsulation } from '@angular/core';
import { throwError } from 'rxjs';
import { ContactFormService } from '../contact-form.service';
import { ContactFormModel } from '../ContactFormModel';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ContactComponent {

  constructor() {}

}
