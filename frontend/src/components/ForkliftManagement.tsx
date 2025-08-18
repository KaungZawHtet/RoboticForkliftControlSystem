import React from 'react'
import { Truck } from 'lucide-react'
import { ForkliftTable } from './forklift/ForkliftTable'
import { FileUpload } from './forklift/FileUpload'
import { MovementCommandInput } from './movement/MovementCommandInput'
import { useForkliftData } from '../hooks/useForkliftData'
import { useMovementCommand } from '../hooks/useMovementCommand'
import { getErrorMessage } from '../utils'

export const ForkliftManagement: React.FC = () => {
  const {
    forklifts,
    isLoading: isLoadingForklifts,
    uploadFile,
    isUploading,
    uploadError,
  } = useForkliftData()

  const {
    parseCommand,
    result: movementResult,
    isLoading: isParsing,
    error: parseError,
  } = useMovementCommand()

  const handleFileUpload = (file: File) => {
    uploadFile(file)
  }

  const handleCommandSubmit = (command: string) => {
    parseCommand(command)
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="mb-8">
          <div className="bg-white shadow rounded-lg">
            <div className="px-6 py-4 border-b border-gray-200">
              <h1 className="text-3xl font-bold text-gray-900 flex items-center">
                <Truck className="mr-3 h-8 w-8 text-blue-600" />
                Forklift Fleet Management System
              </h1>
              <p className="mt-2 text-gray-600">
                Manage your forklift fleet and parse movement commands
              </p>
            </div>
          </div>
        </div>

        {/* Main Content */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-8">
          {/* File Upload Section */}
          <FileUpload
            onFileUpload={handleFileUpload}
            isUploading={isUploading}
            error={uploadError ? getErrorMessage(uploadError) : null}
          />

          {/* Movement Command Section */}
          <MovementCommandInput
            onCommandSubmit={handleCommandSubmit}
            isLoading={isParsing}
            result={movementResult}
            error={parseError ? getErrorMessage(parseError) : null}
          />
        </div>

        {/* Forklift Table */}
        <ForkliftTable forklifts={forklifts} isLoading={isLoadingForklifts} />
      </div>
    </div>
  )
}
