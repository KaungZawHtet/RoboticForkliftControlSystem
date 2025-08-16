using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Abstractions;

public interface IMovementService
{
    MovementResult ParseMovementCommand(string command);
}
