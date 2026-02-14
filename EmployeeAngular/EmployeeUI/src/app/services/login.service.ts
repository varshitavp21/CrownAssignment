import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { URLS } from '../constant';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  loginUser(data: any): Observable<any> {
    return this.http.post(
      `${URLS.ApiEndPoint}/${URLS.Login.login}`,
      data
    );
  }
}
