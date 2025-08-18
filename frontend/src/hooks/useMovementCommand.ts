import { useMutation } from '@tanstack/react-query'
import { apiService } from '../services/api'
import { toast } from 'react-hot-toast'

export function useMovementCommand() {
  const mutation = useMutation({
    mutationFn: (command: string) => apiService.parseMovementCommand(command),
    onSuccess: () => {
      toast.success('Command parsed successfully')
    },
    onError: (error) => {
      console.error('Command parsing failed:', error)
      toast.error('Failed to parse movement command')
    },
  })

  return {
    parseCommand: mutation.mutate,
    result: mutation.data,
    isLoading: mutation.isPending,
    error: mutation.error,
    reset: mutation.reset,
  }
}
