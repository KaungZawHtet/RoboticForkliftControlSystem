import React, { useRef } from 'react'
import { Upload, FileText, X } from 'lucide-react'
import { useForm, Controller } from 'react-hook-form'
import { Card, CardContent, CardHeader, CardTitle } from '../ui/Card'
import { Button } from '../ui/Button'
import { Alert } from '../ui/Alert'
import { formatFileSize } from '../../utils'
import type { FileUploadForm } from '../../types'
import { FILE_CONFIG, UI_MESSAGES } from '../../config/constants'

interface FileUploadProps {
  onFileUpload: (file: File) => void
  isUploading: boolean
  error?: string | null
}

export const FileUpload: React.FC<FileUploadProps> = ({ onFileUpload, isUploading, error }) => {
  const fileInputRef = useRef<HTMLInputElement>(null)

  const {
    control,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
  } = useForm<FileUploadForm>()

  const selectedFile = watch('file')?.[0]

  const onSubmit = (data: FileUploadForm) => {
    const file = data.file?.[0]
    if (!file) return

    const validation = validateFile(file)
    if (!validation.isValid) return

    onFileUpload(file)
  }

  function validateFile(file: File): {
    isValid: boolean
    error?: string
  } {
    const dot = file.name.lastIndexOf('.')
    const ext = dot >= 0 ? file.name.slice(dot).toLowerCase() : ''
    if (!FILE_CONFIG.ACCEPTED_TYPES.map((t) => t.toLowerCase()).includes(ext)) {
      return { isValid: false, error: UI_MESSAGES.ERROR.INVALID_FILE_TYPE }
    }
    if (file.size > FILE_CONFIG.MAX_SIZE_BYTES) {
      return { isValid: false, error: UI_MESSAGES.ERROR.FILE_TOO_LARGE }
    }
    return { isValid: true }
  }

  const handleFileSelect = (files: FileList | null) => {
    if (files && files[0]) {
      const validation = validateFile(files[0])
      if (validation.isValid) {
        setValue('file', files)
      }
    }
  }

  const clearFile = () => {
    setValue('file', null)
    if (fileInputRef.current) {
      fileInputRef.current.value = ''
    }
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center">
          <Upload className="mr-2 h-5 w-5" />
          Import Forklift Data
        </CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <Controller
              name="file"
              control={control}
              rules={{ required: 'Please select a file' }}
              render={({ field: { onChange, name } }) => (
                <div className="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center hover:border-gray-400 transition-colors">
                  <input
                    ref={fileInputRef}
                    type="file"
                    name={name}
                    accept=".csv,.json"
                    onChange={(e) => {
                      onChange(e.target.files)
                      handleFileSelect(e.target.files)
                    }}
                    className="hidden"
                    disabled={isUploading}
                  />

                  {selectedFile ? (
                    <div className="flex items-center justify-between bg-gray-50 rounded p-3">
                      <div className="flex items-center">
                        <FileText className="h-5 w-5 text-blue-500 mr-2" />
                        <div className="text-left">
                          <p className="text-sm font-medium text-gray-900">{selectedFile.name}</p>
                          <p className="text-xs text-gray-500">
                            {formatFileSize(selectedFile.size)}
                          </p>
                        </div>
                      </div>
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        onClick={clearFile}
                        className="text-red-500 hover:text-red-700"
                      >
                        <X className="h-4 w-4" />
                      </Button>
                    </div>
                  ) : (
                    <div>
                      <Upload className="mx-auto h-12 w-12 text-gray-400 mb-4" />
                      <p className="text-sm text-gray-600 mb-2">
                        <button
                          type="button"
                          onClick={() => fileInputRef.current?.click()}
                          className="text-blue-600 hover:text-blue-500 font-medium"
                        >
                          Click to upload
                        </button>{' '}
                        or drag and drop
                      </p>
                      <p className="text-xs text-gray-500">CSV or JSON files only (max 10MB)</p>
                    </div>
                  )}
                </div>
              )}
            />
            {errors.file && <p className="mt-1 text-sm text-red-600">{errors.file.message}</p>}
          </div>

          {error && (
            <Alert variant="error">
              <p>{error}</p>
            </Alert>
          )}

          <Button
            type="submit"
            disabled={!selectedFile || isUploading}
            isLoading={isUploading}
            className="w-full"
          >
            {isUploading ? 'Uploading...' : 'Upload File'}
          </Button>
        </form>

        <div className="mt-4 text-sm text-gray-600">
          <p className="font-medium mb-1">Expected file format:</p>
          <ul className="text-xs space-y-1">
            <li>
              • <strong>CSV:</strong> Name, ModelNumber, ManufacturingDate
            </li>
            <li>
              • <strong>JSON:</strong> Array of objects with name, modelNumber, manufacturingDate
            </li>
          </ul>
        </div>
      </CardContent>
    </Card>
  )
}
