namespace RoboticForkliftControlSystem.Api.Entities;

public class Forklift
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string ModelNumber { get; set; } = default!;
    public DateTime ManufacturingDate { get; set; }
}
