using Microsoft.AspNetCore.Mvc;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Constants;
using RoboticForkliftControlSystem.Api.Dtos;

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

    [HttpGet("parse")]
    public ActionResult<MovementResult> ParseMovementCommand([FromQuery] string command)
    {
        try
        {
            var result = _movementService.ParseMovementCommand(command);
            return Ok(result);
        }
        catch
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                MovementMessages.ParsingError
            );
        }
    }
}
