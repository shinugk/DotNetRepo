import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment';

@Injectable({ providedIn: 'root' })
export class AiAssistantService {

    private apiUrl = `${environment.apiBaseUrl}/api/ai/employer-insight`;

    constructor(private http: HttpClient) { }

    askEmployerInsight(employer: any, question: string) {
        const payload = {
            companyName: employer.companyName,
            offeredRole: employer.offeredRole,
            interviewStatus: employer.interviewStatus,
            ctcOffered: employer.ctcOffered,
            location: employer.location,
            userQuestion: question
        };

        return this.http.post<any>(this.apiUrl, payload);
    }
}
