import { useCallback } from 'react';
import { FILE_CONFIG, UI_MESSAGES } from '../config/constants';

export function useFileValidation() {
    const validateFile = useCallback(
        (file: File): { isValid: boolean; error?: string } => {
            // Check file type
            const fileExtension =
                '.' + file.name.split('.').pop()?.toLowerCase() ;
            if (!FILE_CONFIG.ACCEPTED_TYPES.includes(fileExtension)) {
                return {
                    isValid: false,
                    error: UI_MESSAGES.ERROR.INVALID_FILE_TYPE,
                };
            }

            // Check file size
            if (file.size > FILE_CONFIG.MAX_SIZE_BYTES) {
                return {
                    isValid: false,
                    error: UI_MESSAGES.ERROR.FILE_TOO_LARGE,
                };
            }

            return { isValid: true };
        },
        [],
    );

    return { validateFile };
}
