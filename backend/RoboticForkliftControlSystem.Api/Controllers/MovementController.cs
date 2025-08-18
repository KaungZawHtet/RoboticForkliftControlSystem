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

    private readonly ILogger<MovementController> _logger;

    public MovementController(IMovementService movementService, ILogger<MovementController> logger)
    {
        _movementService = movementService;
        _logger = logger;
    }

    [HttpGet("parse")]
    public ActionResult<MovementResult> ParseMovementCommand([FromQuery] string command)
    {
        try
        {
            var result = _movementService.ParseMovementCommand(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, MovementMessages.ParsingErrorForLog, command);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                MovementMessages.ParsingError
            );
        }
    }
}
