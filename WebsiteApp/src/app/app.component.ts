import { Component, OnDestroy, OnInit } from '@angular/core';
import { EmailService } from "./services/email.service";
import { BehaviorSubject, Observable, Subject} from "rxjs";
import { takeUntil } from "rxjs/operators";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Title } from "@angular/platform-browser";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {

  sending$: Observable<boolean>;

  private invalid: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  invalid$ = this.invalid.asObservable();

  emailForm = new FormGroup({
    email: new FormControl('', [
      Validators.email,
      Validators.required
    ]),
    emailBody: new FormControl('', Validators.required)
  })

  private destroy$: Subject<void> = new Subject<void>()

  constructor(private emailService: EmailService, private snackBar: MatSnackBar, private titleService: Title) {
    this.sending$ = emailService.sending$;
    this.emailForm.statusChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => this.invalid.next(result == 'INVALID'));
  }

  ngOnInit(): void {
    this.titleService.setTitle('Ryan Ha');
  }

  sendEmail() {
    this.emailService.sendEmail({
      emailAddress: this.emailForm.controls['email']?.value,
      emailBody: this.emailForm.controls['emailBody']?.value
    })
    .pipe(takeUntil(this.destroy$))
    .subscribe(() => {
      this.snackBar.open('Email sent!');
    }, () => {
      this.snackBar.open('Something went wrong, please email me@ryanha.dev');
    })
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
