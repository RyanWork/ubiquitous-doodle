import {TestBed, tick} from '@angular/core/testing';

import { EmailService } from './email.service';
import { HttpClientTestingModule, HttpTestingController } from "@angular/common/http/testing";
import { Email } from "../../model/Email";

describe('EmailService', () => {
  let httpTestingController: HttpTestingController;
  let emailService: EmailService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ]
    });
    httpTestingController = TestBed.inject(HttpTestingController);
    emailService = TestBed.inject(EmailService);
  });

  it('should be created', () => {
    expect(emailService).toBeTruthy();
  });

  it('should be able to send emails', () => {
    const fakeEmail: Email = CreateFakeEmail();

    emailService.sendEmail(fakeEmail).subscribe();

    const req = httpTestingController.expectOne({
      url: "https://localhost:5001/api/v1/Email"
    });
    expect(req.request.method).toEqual("POST");
    req.flush({});
    httpTestingController.verify();
  });

  it('should set sending$ to false after resolving send email', () => {
    const fakeEmail: Email = CreateFakeEmail();
    emailService.sendEmail(fakeEmail).subscribe();
    const req = httpTestingController.expectOne({
      url: "https://localhost:5001/api/v1/Email"
    });
    expect(req.request.method).toEqual("POST");

    emailService.sending$.subscribe(result => {
      expect(result).toBeFalse();
    });

    req.flush({});
    httpTestingController.verify();
  })
});

function CreateFakeEmail() {
  return {
    emailAddress: "fakeemail@ryanha.dev",
    emailBody: "Hello this is a test"
  }
}
