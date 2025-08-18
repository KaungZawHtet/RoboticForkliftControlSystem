import React from 'react'
import { Send, HelpCircle } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { Card, CardContent, CardHeader, CardTitle } from '../ui/Card'
import { Button } from '../ui/Button'
import { Alert } from '../ui/Alert'
import type { MovementCommandForm, MovementResult } from '../../types'

interface MovementCommandInputProps {
  onCommandSubmit: (command: string) => void
  isLoading: boolean
  result: MovementResult | undefined
  error?: string | null
}

export const MovementCommandInput: React.FC<MovementCommandInputProps> = ({
  onCommandSubmit,
  isLoading,
  result,
  error,
}) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<MovementCommandForm>()

  const onSubmit = (data: MovementCommandForm) => {
    onCommandSubmit(data.command.trim().toUpperCase())
  }

  const exampleCommands = [
    { command: 'F10R90L90B5', description: 'Forward 10m, Right 90°, Left 90°, Backward 5m' },
    { command: 'F5R180B3', description: 'Forward 5m, Right 180°, Backward 3m' },
    { command: 'L270F15', description: 'Left 270°, Forward 15m' },
  ]

  const handleExampleClick = (command: string) => {
    onCommandSubmit(command)
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center">
          <Send className="mr-2 h-5 w-5" />
          Movement Command Parser
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        {/* Command Format Help */}
        <Alert variant="info">
          <HelpCircle className="h-4 w-4" />
          <div>
            <p className="font-medium mb-2">Command Format:</p>
            <ul className="text-sm space-y-1">
              <li>
                <strong>F[number]</strong> - Move Forward (metres)
              </li>
              <li>
                <strong>B[number]</strong> - Move Backward (metres)
              </li>
              <li>
                <strong>L[degrees]</strong> - Turn Left (90, 180, 270, 360)
              </li>
              <li>
                <strong>R[degrees]</strong> - Turn Right (90, 180, 270, 360)
              </li>
            </ul>
          </div>
        </Alert>

        {/* Command Input Form */}
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <div className="flex space-x-2">
              <input
                {...register('command', {
                  required: 'Please enter a movement command',
                  pattern: {
                    value: /^[FBLR]\d+([FBLR]\d+)*$/i,
                    message: 'Invalid command format',
                  },
                })}
                type="text"
                placeholder="Enter command (e.g., F10R90L90B5)"
                className="flex-1 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                disabled={isLoading}
              />
              <Button type="submit" isLoading={isLoading} disabled={isLoading}>
                Parse
              </Button>
            </div>
            {errors.command && (
              <p className="mt-1 text-sm text-red-600">{errors.command.message}</p>
            )}
          </div>
        </form>

        {/* Example Commands */}
        <div>
          <p className="text-sm font-medium text-gray-700 mb-2">Example Commands:</p>
          <div className="space-y-2">
            {exampleCommands.map((example, index) => (
              <div key={index} className="flex items-center justify-between bg-gray-50 rounded p-2">
                <div>
                  <code className="text-sm font-mono text-blue-600">{example.command}</code>
                  <p className="text-xs text-gray-600">{example.description}</p>
                </div>
                <Button
                  type="button"
                  variant="ghost"
                  size="sm"
                  onClick={() => handleExampleClick(example.command)}
                  disabled={isLoading}
                >
                  Try
                </Button>
              </div>
            ))}
          </div>
        </div>

        {/* Error Display */}
        {error && (
          <Alert variant="error">
            <p>{error}</p>
          </Alert>
        )}

        {/* Results Display */}
        {result && (
          <div className="border rounded-lg p-4">
            <h3 className="text-md font-semibold mb-3 flex items-center">
              Command Result:
              <span
                className={`ml-2 px-2 py-1 rounded text-sm ${
                  result.isValid ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                }`}
              >
                {result.isValid ? 'Valid' : 'Invalid'}
              </span>
            </h3>

            {result.isValid && result.commands.length > 0 && (
              <div className="mb-3">
                <h4 className="text-sm font-medium text-gray-700 mb-2">Actions:</h4>
                <div className="space-y-1">
                  {result.commands.map((cmd, index) => (
                    <div
                      key={index}
                      className="text-sm text-gray-600 bg-green-50 p-2 rounded flex items-center"
                    >
                      <span className="w-6 h-6 bg-green-200 text-green-800 rounded-full flex items-center justify-center text-xs font-medium mr-2">
                        {index + 1}
                      </span>
                      {cmd.description}
                    </div>
                  ))}
                </div>
              </div>
            )}

            {result.errors.length > 0 && (
              <div>
                <h4 className="text-sm font-medium text-red-700 mb-2">Errors:</h4>
                <div className="space-y-1">
                  {result.errors.map((error, index) => (
                    <div key={index} className="text-sm text-red-600 bg-red-50 p-2 rounded">
                      {error}
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>
        )}
      </CardContent>
    </Card>
  )
}
