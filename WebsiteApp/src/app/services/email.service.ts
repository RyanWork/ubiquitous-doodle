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
    return this.httpClient.post("https://localhost:5001/api/v1/Email", email)
  }
}
