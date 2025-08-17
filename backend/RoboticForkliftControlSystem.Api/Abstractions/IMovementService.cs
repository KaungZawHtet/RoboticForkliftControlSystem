using RoboticForkliftControlSystem.Api.Dtos;

namespace RoboticForkliftControlSystem.Api.Abstractions;

public interface IMovementService
{
    MovementResult ParseMovementCommand(string command);
}
