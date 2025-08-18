export const API_CONFIG = {
  BASE_URL: import.meta.env.VITE_API_URL,
  TIMEOUT: 10000,
  RETRY_ATTEMPTS: 3 as number,
} as const

export const ENDPOINTS = {
  FORKLIFTS: '/forklifts',
  FORKLIFTS_IMPORT: '/forklifts/import',
  MOVEMENT_PARSE: '/movement/parse',
} as const

export const FILE_CONFIG: {
  ACCEPTED_TYPES: string[]
  MAX_SIZE_MB: number
  MAX_SIZE_BYTES: number
} = {
  ACCEPTED_TYPES: ['.csv', '.json'],
  MAX_SIZE_MB: 10,
  MAX_SIZE_BYTES: 10 * 1024 * 1024, // 10MB
}

export const MOVEMENT_COMMANDS = {
  FORWARD: 'F',
  BACKWARD: 'B',
  LEFT: 'L',
  RIGHT: 'R',
} as const

export const UI_MESSAGES = {
  SUCCESS: {
    FILE_UPLOADED: 'File uploaded successfully',
    COMMAND_PARSED: 'Command parsed successfully',
  },
  ERROR: {
    FILE_UPLOAD_FAILED: 'Failed to upload file',
    INVALID_FILE_TYPE: 'Invalid file type. Please upload CSV or JSON files only.',
    FILE_TOO_LARGE: 'File size exceeds 10MB limit',
    NETWORK_ERROR: 'Network error. Please check your connection.',
    COMMAND_PARSE_FAILED: 'Failed to parse movement command',
  },
  LOADING: {
    UPLOADING: 'Uploading file...',
    LOADING_FORKLIFTS: 'Loading forklifts...',
    PARSING_COMMAND: 'Parsing command...',
  },
} as const

export const QUERY_KEYS = {
  FORKLIFTS: ['forklifts'] as const,
}
