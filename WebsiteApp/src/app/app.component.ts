import {Component, OnDestroy} from '@angular/core';
import { EmailService } from "./services/email.service";
import { Subject } from "rxjs";
import {takeUntil} from "rxjs/operators";
import {FormControl, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnDestroy {
  title = 'Ryan Ha';

  emailForm = new FormGroup({
    email: new FormControl('', [
      Validators.email,
      Validators.required
    ]),
    emailBody: new FormControl('', Validators.required)
  })

  private destroy$: Subject<void> = new Subject<void>()

  constructor(private emailService: EmailService) {
  }

  sendEmail() {
    this.emailService.sendEmail()
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        console.log('test');
      }, error => {
        console.log(error);
      })
  }

  onSubmit() {
    console.log(this.emailForm.controls["email"]?.value);
    console.log(this.emailForm.controls["emailBody"]?.value);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
  }
}
