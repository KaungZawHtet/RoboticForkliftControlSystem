// src/services/api.ts
import axios, {
    AxiosError,
    type AxiosInstance,
    type AxiosRequestConfig,
} from 'axios';
import { API_CONFIG, ENDPOINTS } from '../config/constants';
import type { Forklift, MovementResult, ApiError } from '../types';

class ApiService {
    private client: AxiosInstance;

    constructor() {
        this.client = axios.create({
            baseURL: API_CONFIG.BASE_URL,
            timeout: API_CONFIG.TIMEOUT,
            headers: {
                'Content-Type': 'application/json',
            },
        });

        this.setupInterceptors();
    }

    private setupInterceptors(): void {
        // Request interceptor
        this.client.interceptors.request.use(
            (config) => {
                console.log(
                    `🚀 API Request: ${config.method?.toUpperCase()} ${config.url}`,
                );
                return config;
            },
            (error) => {
                console.error('❌ Request Error:', error);
                return Promise.reject(error);
            },
        );

        // Response interceptor
        this.client.interceptors.response.use(
            (response) => {
                console.log(
                    `✅ API Response: ${response.status} ${response.config.url}`,
                );
                return response;
            },
            (error: AxiosError) => {
                console.error(
                    '❌ Response Error:',
                    error.response?.status,
                    error.message,
                );
                return Promise.reject(this.handleError(error));
            },
        );
    }

    private handleError(error: AxiosError): ApiError {
        if (error.response) {
            // Server responded with error status
            return {
                message: (error.response.data as string) || error.message,
                status: error.response.status,
            };
        } else if (error.request) {
            // Request was made but no response received
            return {
                message: 'Network error. Please check your connection.',
                status: 0,
            };
        } else {
            // Something else happened
            return {
                message: error.message || 'An unexpected error occurred',
            };
        }
    }

    // Generic request method with retry logic
    private async request<T>(
        config: AxiosRequestConfig,
        retries = API_CONFIG.RETRY_ATTEMPTS,
    ): Promise<T> {
        try {
            const response = await this.client.request<T>(config);
            return response.data;
        } catch (error) {
            if (retries > 0 && this.shouldRetry(error as AxiosError)) {
                console.log(`🔄 Retrying request... ${retries} attempts left`);
                await this.delay(1000); // Wait 1 second before retry
                return this.request<T>(config, retries - 1);
            }
            throw error;
        }
    }

    private shouldRetry(error: AxiosError): boolean {
        return (
            !error.response || // Network errors
            error.response.status >= 500 || // Server errors
            error.response.status === 408 // Timeout
        );
    }

    private delay(ms: number): Promise<void> {
        return new Promise((resolve) => setTimeout(resolve, ms));
    }

    // API Methods
    async getForklifts(): Promise<Forklift[]> {
        return this.request<Forklift[]>({
            method: 'GET',
            url: ENDPOINTS.FORKLIFTS,
        });
    }

    async uploadFile(file: File): Promise<Forklift[]> {
        const formData = new FormData();
        formData.append('file', file);

        return this.request<Forklift[]>({
            method: 'POST',
            url: ENDPOINTS.FORKLIFTS_IMPORT,
            data: formData,
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
    }

    async parseMovementCommand(command: string): Promise<MovementResult> {
        return this.request<MovementResult>({
            method: 'GET',
            url: `${ENDPOINTS.MOVEMENT_PARSE}?command=${encodeURIComponent(command)}`,
        });
    }
}

export const apiService = new ApiService();
