import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';
import { AppComponent } from './app.component';
import { HttpClientTestingModule } from "@angular/common/http/testing";
import SpyObj = jasmine.SpyObj;
import { EmailService } from "./services/email.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Overlay } from "@angular/cdk/overlay";
import { RouterTestingModule } from "@angular/router/testing";
import { of, throwError } from "rxjs";
import { delay } from "rxjs/operators";
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from "@angular/material/form-field";
import { CircleLinkComponent } from "./circle-link/circle-link.component";

describe('AppComponent', () => {
  let emailServiceSpy: SpyObj<EmailService>;
  let snackBarSpy: SpyObj<MatSnackBar>;
  let sut: AppComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatCardModule,
        MatFormFieldModule
      ],
      declarations: [
        AppComponent,
        CircleLinkComponent
      ],
      providers: [
        MatSnackBar,
        Overlay
      ]
    });

    emailServiceSpy = TestBed.inject(EmailService) as SpyObj<EmailService>;
    snackBarSpy = TestBed.inject(MatSnackBar) as SpyObj<MatSnackBar>;
    sut = TestBed.createComponent(AppComponent).componentInstance;
  });

  it('should create the app', () => {
    expect(sut).toBeTruthy();
  });

  it('should call email service to send an email', () => {
    spyOn(emailServiceSpy, 'sendEmail').and.callThrough();

    sut.sendEmail();

    expect(emailServiceSpy.sendEmail).toHaveBeenCalled();
  });

  it('should open snack bar with \'Email sent!\' when email is successfully sent', fakeAsync(() => {
    spyOn(emailServiceSpy, 'sendEmail').and.returnValue(of({}).pipe(delay(1)));
    spyOn(snackBarSpy, 'open');

    sut.sendEmail()

    tick(1);
    expect(emailServiceSpy.sendEmail).toHaveBeenCalled();
    expect(snackBarSpy.open).toHaveBeenCalledOnceWith('Email sent!')
  }));

  it('should open snack bar with \'Something went wrong, please email me@ryanha.dev\' when email fails to send', fakeAsync(() => {
    spyOn(emailServiceSpy, 'sendEmail').and.returnValue(throwError('some fake error'));
    spyOn(snackBarSpy, 'open');

    sut.sendEmail()

    tick(1);
    expect(emailServiceSpy.sendEmail).toHaveBeenCalled();
    expect(snackBarSpy.open).toHaveBeenCalledOnceWith('Something went wrong, please email me@ryanha.dev')
  }))
});
