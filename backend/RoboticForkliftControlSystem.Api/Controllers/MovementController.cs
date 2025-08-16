using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovementController : ControllerBase
{
    private readonly IMovementService _movementService;

    public MovementController(IMovementService movementService)
    {
        _movementService = movementService;
    }

    [HttpPost("parse")]
    public ActionResult<MovementResult> ParseMovementCommand([FromBody] string command)
    {
        try
        {
            var result = _movementService.ParseMovementCommand(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error parsing movement command: {ex.Message}");
        }
    }
}
