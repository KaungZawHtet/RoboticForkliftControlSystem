using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Abstractions;

public interface IMovementService
{
    MovementResult ParseMovementCommand(string command);
}
