import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Email } from "../../model/Email";
import { Observable, Subject } from "rxjs";
import {finalize, tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  private sending: Subject<boolean> = new Subject();
  public sending$: Observable<boolean> = this.sending.asObservable();

  constructor(private httpClient: HttpClient) {
  }

  public sendEmail(email: Email) {
    this.sending.next(true);
    return this.httpClient.post("https://ryanha.dev/api/v1/Email", email)
      .pipe(finalize(() => this.sending.next(false)))
  }
}
