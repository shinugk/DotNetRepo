import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserProfile } from '../Interfaces/profile.model';
import { Employer } from '../Interfaces/employer.model';
import { environment } from 'src/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) {}

  getMyProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${environment.apiBaseUrl}/api/user/me`);
  }

  getEmployersByUser(userId: number): Observable<Employer[]> {
    return this.http.get<Employer[]>(`${environment.apiBaseUrl}/api/user/${userId}`);
  }

   updateUser(id: number, payload: any): Observable<UserProfile> {
    return this.http.patch<UserProfile>(`${environment.apiBaseUrl}/api/user/${id}`, payload);
  }

  downloadResume() {
  return this.http.get(
    `${environment.apiBaseUrl}/api/user/me/resume/download`,
    { responseType: 'blob' }
  );
}

}
