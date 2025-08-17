namespace RoboticForkliftControlSystem.Api.Dtos;

public record MovementCommand(string Action, int Value, string Description);

public record MovementResult(
    bool IsValid,
    IReadOnlyList<MovementCommand> Commands,
    IReadOnlyList<string> Errors
);
