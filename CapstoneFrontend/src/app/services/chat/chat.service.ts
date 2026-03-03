import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../config/api';

export interface ChatRequest {
    userMessage: string;
    requestAudio: boolean;
}

export interface ChatResponse {
    response: string;
    audioBase64?: string;
    imageBase64?: string;
}

@Injectable({
    providedIn: 'root'
})
export class ChatService {
    private http = inject(HttpClient);
    private apiUrl = `${API_BASE_URL}/ChatBot`;

    askBot(message: string, requestAudio: boolean = false): Observable<ChatResponse> {
        return this.http.post<ChatResponse>(`${this.apiUrl}/ask`, {
            userMessage: message,
            requestAudio: requestAudio
        });
    }
}
