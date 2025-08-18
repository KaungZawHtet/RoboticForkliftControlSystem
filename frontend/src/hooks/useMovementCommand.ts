import { useMutation } from '@tanstack/react-query'
import { apiService } from '../services/api'
import { toast } from 'react-hot-toast'
import { UI_MESSAGES } from '../config/constants'

export function useMovementCommand() {
  const mutation = useMutation({
    mutationFn: (command: string) => apiService.parseMovementCommand(command),
    onSuccess: () => {
      toast.success(UI_MESSAGES.SUCCESS.COMMAND_PARSED)
    },
    onError: (error) => {
      console.error('Command parsing failed:', error)
      toast.error(UI_MESSAGES.ERROR.COMMAND_PARSE_FAILED)
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
