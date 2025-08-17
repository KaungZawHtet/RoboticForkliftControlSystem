 
export interface Forklift {
    id?: number;
    name: string;
    modelNumber: string;
    manufacturingDate: string;
}

export interface MovementCommand {
    action: string;
    value: number;
    description: string;
}

export interface MovementResult {
    isValid: boolean;
    commands: MovementCommand[];
    errors: string[];
}

export interface ApiError {
    message: string;
    status?: number;
}

export interface FileUploadResponse {
    success: boolean;
    data?: Forklift[];
    message?: string;
    count?: number;
}

// API Response types
export interface ApiResponse<T> {
    data: T;
    success: boolean;
    message?: string;
}

// Form types
export interface FileUploadForm {
    file: FileList | null;
}

export interface MovementCommandForm {
    command: string;
}
