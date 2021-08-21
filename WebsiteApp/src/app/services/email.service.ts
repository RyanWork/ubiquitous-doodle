import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Email } from "../../model/Email";
import { Observable, Subject } from "rxjs";
import { tap } from "rxjs/operators";

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
    return this.httpClient.post("https://localhost:5001/api/v1/Email", email)
      .pipe(tap(() => this.sending.next(false)))
  }
}
