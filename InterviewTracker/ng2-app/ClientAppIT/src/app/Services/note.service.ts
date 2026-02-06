import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment';

export interface Note {
    id: number;
    title?: string;
    content: string;
    createdAt: string;
}

@Injectable({
    providedIn: 'root'
})
export class NoteService {


    constructor(private http: HttpClient) { }

    // GET: all notes for logged-in user
    getNotes(): Observable<Note[]> {
        return this.http.get<Note[]>(`${environment.apiBaseUrl}/api/note`);
    }

    // POST: create note
    createNote(note: Partial<Note>): Observable<Note> {
        return this.http.post<Note>(`${environment.apiBaseUrl}/api/note`, note);
    }

    // PUT: update note
    updateNote(id: number, note: Partial<Note>): Observable<Note> {
        return this.http.put<Note>(`${environment.apiBaseUrl}/api/note/${id}`, note);
    }

    // DELETE: delete note
    deleteNote(id: number): Observable<void> {
        return this.http.delete<void>(`${environment.apiBaseUrl}/api/note/${id}`);
    }
}
