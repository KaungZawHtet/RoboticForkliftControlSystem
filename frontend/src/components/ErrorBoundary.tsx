import { Component, type ErrorInfo, type ReactNode } from 'react';
import { AlertTriangle, RefreshCw } from 'lucide-react';
import { Button } from './ui/Button';
import { Card, CardContent } from './ui/Card';

interface Props {
  children: ReactNode;
}

interface State {
  hasError: boolean;
  error: Error | null;
  errorInfo: ErrorInfo | null;
}

export class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
    error: null,
    errorInfo: null,
  };

  public static getDerivedStateFromError(error: Error): State {
    return {
      hasError: true,
      error,
      errorInfo: null,
    };
  }

  public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    console.error('Uncaught error:', error, errorInfo);
    this.setState({
      error,
      errorInfo,
    });
  }

  private handleReset = () => {
    this.setState({
      hasError: false,
      error: null,
      errorInfo: null,
    });
  };

  public render() {
    if (this.state.hasError) {
      return (
        <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
          <Card className="max-w-lg w-full">
            <CardContent className="pt-6">
              <div className="text-center">
                <AlertTriangle className="mx-auto h-12 w-12 text-red-500 mb-4" />
                <h1 className="text-xl font-semibold text-gray-900 mb-2">
                  Something went wrong
                </h1>
                <p className="text-gray-600 mb-6">
                  We're sorry, but something went wrong. Please try refreshing the page.
                </p>
                
                {process.env.NODE_ENV === 'development' && this.state.error && (
                  <details className="text-left bg-gray-100 rounded p-4 mb-4">
                    <summary className="cursor-pointer font-medium">Error Details</summary>
                    <pre className="mt-2 text-sm text-red-600 overflow-auto">
                      {this.state.error.toString()}
                    </pre>
                  </details>
                )}
                
                <div className="flex flex-col sm:flex-row gap-3 justify-center">
                  <Button onClick={this.handleReset} className="flex items-center">
                    <RefreshCw className="mr-2 h-4 w-4" />
                    Try Again
                  </Button>
                  <Button 
                    variant="secondary" 
                    onClick={() => window.location.reload()}
                  >
                    Refresh Page
                  </Button>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>
      );
    }

    return this.props.children;
  }
}
