import { Injectable, OnDestroy } from '@angular/core';
import { Observable, Subject } from "rxjs";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  constructor(private httpClient: HttpClient) {
  }

  public sendEmail() {
    return this.httpClient.post("", {})
  }
}
