namespace RoboticForkliftControlSystem.Api.Constants;

public class RegexPatterns
{
    public const string ValidCommand = @"([FBLR])(\d+)";
}

public static class TurnRules
{
    public const int MinDegrees = 0;
    public const int MaxDegrees = 360;
    public const int StepDegrees = 90;
}

public static class MovementActions
{
    public const char Forward = 'F';
    public const char Backward = 'B';
    public const char Left = 'L';
    public const char Right = 'R';
}
