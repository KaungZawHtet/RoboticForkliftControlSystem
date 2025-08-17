using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticForkliftControlSystem.Api.Constants;

public static class MovementMessages
{
    // Generic / validation
    public const string CommandEmpty = "Command cannot be empty.";
    public const string NoValidCommands = "No valid commands found in input.";
    public const string UnexpectedChars =
        "Input contains unexpected characters or format between commands.";

    // Parsing / action validation
    public static string InvalidValueForAction(char action) => $"Invalid value for action {action}";

    public static string UnknownAction(char action) => $"Unknown action '{action}'";

    // Turn rules
    public static string LeftTurnOutOfRange(int value) =>
        $"Left turn degrees must be between 0-360 and multiples of 90. Got: {value}";

    public static string RightTurnOutOfRange(int value) =>
        $"Right turn degrees must be between 0-360 and multiples of 90. Got: {value}";

    // Descriptions
    public static string MoveForward(int metres) => $"Move Forward by {metres} metres.";

    public static string MoveBackward(int metres) => $"Move Backward by {metres} metres.";

    public static string TurnLeft(int degrees) => $"Turn Left by {degrees} degrees.";

    public static string TurnRight(int degrees) => $"Turn Right by {degrees} degrees.";
}
