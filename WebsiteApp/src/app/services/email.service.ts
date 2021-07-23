import { Injectable} from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Email } from "../../model/Email";

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  constructor(private httpClient: HttpClient) {
  }

  public sendEmail(email: Email) {
    return this.httpClient.post("http://localhost:5000/api/v1/Email", email)
  }
}
