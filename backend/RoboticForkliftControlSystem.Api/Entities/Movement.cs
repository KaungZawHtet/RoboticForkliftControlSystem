namespace RoboticForkliftControlSystem.Api.Entities;


 public class MovementCommand
{
    public string Action { get; set; }
    public int Value { get; set; }
    public string Description { get; set; }
}

public class MovementResult
{
    public bool IsValid { get; set; }
    public List<MovementCommand> Commands { get; set; } = new List<MovementCommand>();
    public List<string> Errors { get; set; } = new List<string>();
}
