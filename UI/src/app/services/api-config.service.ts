import { Injectable } from '@angular/core';
import { API_CONFIG } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class ApiConfigService {
  private readonly apiBaseUrl: string;

  constructor() {
    this.apiBaseUrl = API_CONFIG.baseUrl;
  }

  getBaseUrl(): string {
    return this.apiBaseUrl;
  }
}
