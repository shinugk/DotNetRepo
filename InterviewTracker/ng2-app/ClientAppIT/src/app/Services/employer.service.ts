import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment';
import { Employer } from '../Interfaces/employer.model';

@Injectable({
  providedIn: 'root',
})
export class EmployerService {
  constructor(private http: HttpClient) {}

  getEmployersForUser() {
    return this.http.get<any[]>(`${environment.apiBaseUrl}/api/employer`);
  }

  getEmployerById(id: number) {
  return this.http.get<Employer>(`${environment.apiBaseUrl}/api/employer/${id}`);
}

  updateEmployer(id: number, payload: any) {
    return this.http.patch(`${environment.apiBaseUrl}/api/employer/${id}`, payload);
  }

  addEmployer(payload: any) {
  return this.http.post(`${environment.apiBaseUrl}/api/employer`, payload);
}
}
