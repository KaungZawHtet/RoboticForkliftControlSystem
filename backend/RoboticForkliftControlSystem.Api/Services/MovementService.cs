using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Services;


public class MovementService : IMovementService
{
    public MovementResult ParseMovementCommand(string command)
    {
        var result = new MovementResult { IsValid = true };

        if (string.IsNullOrWhiteSpace(command))
        {
            result.IsValid = false;
            result.Errors.Add("Command cannot be empty");
            return result;
        }

        var pattern = @"([FBLR])(\d+)";
        var matches = Regex.Matches(command.ToUpper(), pattern);

        foreach (Match match in matches)
        {
            var action = match.Groups[1].Value;
            if (!int.TryParse(match.Groups[2].Value, out int value))
            {
                result.IsValid = false;
                result.Errors.Add($"Invalid value for action {action}");
                continue;
            }

            var movementCommand = new MovementCommand { Action = action, Value = value };

            switch (action)
            {
                case "F":
                    movementCommand.Description = $"Move Forward by {value} metres.";
                    break;
                case "B":
                    movementCommand.Description = $"Move Backward by {value} metres.";
                    break;
                case "L":
                    if (value < 0 || value > 360 || value % 90 != 0)
                    {
                        result.IsValid = false;
                        result.Errors.Add(
                            $"Left turn degrees must be between 0-360 and multiples of 90. Got: {value}"
                        );
                        continue;
                    }
                    movementCommand.Description = $"Turn Left by {value} degrees.";
                    break;
                case "R":
                    if (value < 0 || value > 360 || value % 90 != 0)
                    {
                        result.IsValid = false;
                        result.Errors.Add(
                            $"Right turn degrees must be between 0-360 and multiples of 90. Got: {value}"
                        );
                        continue;
                    }
                    movementCommand.Description = $"Turn Right by {value} degrees.";
                    break;
            }

            result.Commands.Add(movementCommand);
        }

        if (result.Commands.Count == 0)
        {
            result.IsValid = false;
            result.Errors.Add("No valid commands found in input");
        }

        return result;
    }
}
