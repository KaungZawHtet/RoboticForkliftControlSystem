using System.Text.RegularExpressions;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Dtos;

namespace RoboticForkliftControlSystem.Api.Services;

public class MovementService : IMovementService
{
    public MovementResult ParseMovementCommand(string command)
    {
        var commands = new List<MovementCommand>();
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(command))
            return Invalid("Command cannot be empty");

        // Case-insensitive & compiled. Matches tokens like F10, R90, etc.
        var regex = new Regex(@"([FBLR])(\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var input = command.Trim();
        var matches = regex.Matches(input);

        if (matches.Count == 0)
            return Invalid("No valid commands found in input");

        var consumed = 0;

        foreach (Match m in matches)
        {
            consumed += m.Length;

            var action = char.ToUpperInvariant(m.Groups[1].Value[0]);
            if (!int.TryParse(m.Groups[2].Value, out var value))
            {
                errors.Add($"Invalid value for action {action}");
                continue;
            }

            // shared turn validation
            bool IsInvalidTurn(int v) => v < 0 || v > 360 || v % 90 != 0;

            string? description = action switch
            {
                'F' => $"Move Forward by {value} metres.",
                'B' => $"Move Backward by {value} metres.",
                'L' => IsInvalidTurn(value)
                    ? AddErr(
                        errors,
                        $"Left turn degrees must be between 0-360 and multiples of 90. Got: {value}"
                    )
                    : $"Turn Left by {value} degrees.",
                'R' => IsInvalidTurn(value)
                    ? AddErr(
                        errors,
                        $"Right turn degrees must be between 0-360 and multiples of 90. Got: {value}"
                    )
                    : $"Turn Right by {value} degrees.",
                _ => AddErr(errors, $"Unknown action '{action}'"),
            };

            if (description is not null)
                commands.Add(new MovementCommand(action.ToString(), value, description));
        }

        // Detect any junk between tokens (e.g., "F10X5")
        if (consumed != input.Length)
            errors.Add("Input contains unexpected characters or format between commands.");

        bool isValid = errors.Count == 0 && commands.Count > 0;
        if (!isValid && errors.Count == 0)
            errors.Add("No valid commands found in input");

        return new MovementResult(isValid, commands, errors);
    }

    private static string? AddErr(List<string> errors, string msg)
    {
        errors.Add(msg);
        return null;
    }

    private static MovementResult Invalid(string msg) =>
        new(false, new List<MovementCommand>(), new List<string> { msg });
}
