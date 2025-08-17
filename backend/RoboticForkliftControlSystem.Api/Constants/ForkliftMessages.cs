using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticForkliftControlSystem.Api.Constants;

public class ForkliftMessages
{
    // Generic
    public const string InternalServerError = "An unexpected error occurred.";

    // Import
    public const string NoFileUploaded = "No file uploaded.";
    public const string UnsupportedFileFormat =
        "Unsupported file format. Please upload a CSV or JSON file.";
    public const string ImportingError = "Error importing forklifts.";
}
