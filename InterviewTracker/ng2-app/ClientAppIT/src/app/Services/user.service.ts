import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserProfile } from '../Interfaces/profile.model';
import { Employer } from '../Interfaces/employer.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = 'https://localhost:7257/api/user';

  constructor(private http: HttpClient) {}

  getMyProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.baseUrl}/me`);
  }

  getEmployersByUser(userId: number): Observable<Employer[]> {
    return this.http.get<Employer[]>(`${this.baseUrl}/user/${userId}`);
  }
}
