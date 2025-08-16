import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { apiService } from '../services/api';
import { toast } from 'react-hot-toast';
import { UI_MESSAGES } from '../config/constants';
import type { Forklift } from '../types';

export const QUERY_KEYS = {
    FORKLIFTS: ['forklifts'] as const,
};

export function useForkliftData() {
    const queryClient = useQueryClient();

    // Query to fetch forklifts
    const {
        data: forklifts = [],
        isLoading,
        isError,
        error,
        refetch,
    } = useQuery({
        queryKey: QUERY_KEYS.FORKLIFTS,
        queryFn: () => apiService.getForklifts(),
        staleTime: 5 * 60 * 1000, // 5 minutes
        gcTime: 10 * 60 * 1000, // 10 minutes (formerly cacheTime)
    });

    // Mutation for file upload
    const uploadMutation = useMutation({
        mutationFn: (file: File) => apiService.uploadFile(file),
        onSuccess: (newForklifts: Forklift[]) => {
            queryClient.invalidateQueries({ queryKey: QUERY_KEYS.FORKLIFTS });
            toast.success(
                `${UI_MESSAGES.SUCCESS.FILE_UPLOADED}: ${newForklifts.length} forklifts imported`,
            );
        },
        onError: (error) => {
            console.error('Upload failed:', error);
            toast.error(UI_MESSAGES.ERROR.FILE_UPLOAD_FAILED);
        },
    });

    return {
        forklifts,
        isLoading,
        isError,
        error,
        refetch,
        uploadFile: uploadMutation.mutate,
        isUploading: uploadMutation.isPending,
        uploadError: uploadMutation.error,
    };
}
