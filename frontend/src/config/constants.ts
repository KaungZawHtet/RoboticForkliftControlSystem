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

export const QUERY_KEYS = {
  FORKLIFTS: ['forklifts'] as const,
}

export const REGEX_FOR_COMMEND = /^[FBLR]\d+([FBLR]\d+)*$/i;
