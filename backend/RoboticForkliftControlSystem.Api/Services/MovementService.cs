using System.Text.RegularExpressions;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Constants;
using RoboticForkliftControlSystem.Api.Dtos;

namespace RoboticForkliftControlSystem.Api.Services;

public class MovementService : IMovementService
{
    public MovementResult ParseMovementCommand(string command)
    {
        var commands = new List<MovementCommand>();
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(command))
            return Invalid(MovementMessages.CommandEmpty);

        var regex = new Regex(RegexPatterns.ValidCommand);

        var input = command.Trim();
        var matches = regex.Matches(input);

        if (matches.Count is 0)
            return Invalid(MovementMessages.NoValidCommands);

        var consumed = 0;

        foreach (Match match in matches)
        {
            consumed += match.Length;

            var action = match.Groups[1].Value[0];
            if (!int.TryParse(match.Groups[2].Value, out var value))
            {
                errors.Add(MovementMessages.InvalidValueForAction(action));
                continue;
            }

            string? description = action switch
            {
                MovementActions.Forward => MovementMessages.MoveForward(value),
                MovementActions.Backward => MovementMessages.MoveBackward(value),
                MovementActions.Left => IsInvalidTurn(value)
                    ? AddError(errors, MovementMessages.LeftTurnOutOfRange(value))
                    : MovementMessages.TurnLeft(value),
                MovementActions.Right => IsInvalidTurn(value)
                    ? AddError(errors, MovementMessages.RightTurnOutOfRange(value))
                    : MovementMessages.TurnRight(value),
                _ => AddError(errors, MovementMessages.UnknownAction(action)),
            };

            if (description is not null)
                commands.Add(new MovementCommand(action.ToString(), value, description));
        }

        if (consumed != input.Length)
            errors.Add(MovementMessages.UnexpectedChars);

        bool isValid = errors.Count is 0 && commands.Count > 0;
        if (!isValid && errors.Count is 0)
            errors.Add(MovementMessages.NoValidCommands);

        return new MovementResult(isValid, commands, errors);
    }

    private static string? AddError(List<string> errors, string msg)
    {
        errors.Add(msg);
        return null;
    }

    private static MovementResult Invalid(string msg) =>
        new(false, new List<MovementCommand>(), new List<string> { msg });

    private bool IsInvalidTurn(int degree) =>
        degree < TurnRules.MinDegrees
        || degree > TurnRules.MaxDegrees
        || (degree % TurnRules.StepDegrees) is not 0;
}
