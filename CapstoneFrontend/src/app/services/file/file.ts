import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';

@Injectable({
    providedIn: 'root',
})
export class FileService {
    private http = inject(HttpClient);

    upload(file: File) {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post<{ filePath: string }>(`${API_BASE_URL}/Document/upload`, formData);
    }
}
