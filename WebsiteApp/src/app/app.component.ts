import {Component, OnDestroy} from '@angular/core';
import { EmailService } from "./services/email.service";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material/snack-bar";

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

  constructor(private emailService: EmailService, private snackBar: MatSnackBar) {
  }

  sendEmail() {
    this.emailService.sendEmail({
      emailAddress: this.emailForm.controls['email']?.value,
      emailBody: this.emailForm.controls['emailBody']?.value
    })
      .pipe(takeUntil(this.destroy$))
      .subscribe((result) => {
        this.snackBar.open('Email sent!');
        console.log(result);
      }, error => {
        this.snackBar.open(`Error: ${error}`);
        console.log(error);
      })
  }

  ngOnDestroy(): void {
    this.destroy$.next();
  }
}
