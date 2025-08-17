using Microsoft.AspNetCore.Mvc;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Constants;
using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForkliftsController : ControllerBase
    {
        private readonly IForkliftService _forkliftService;

        public ForkliftsController(IForkliftService forkliftService)
        {
            _forkliftService = forkliftService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Forklift>>> GetForklifts()
        {
            try
            {
                var forklifts = await _forkliftService.GetAllForkliftsAsync();
                return Ok(forklifts);
            }
            catch
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ForkliftMessages.InternalServerError
                );
            }
        }

        [HttpPost("import")]
        public async Task<ActionResult<List<Forklift>>> ImportForklifts(IFormFile file)
        {
            if (file is null || file.Length is 0)
            {
                return BadRequest(ForkliftMessages.NoFileUploaded);
            }

            try
            {
                List<Forklift> forklifts;
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                using var stream = file.OpenReadStream();

                forklifts = fileExtension switch
                {
                    FileTypes.Csv => await _forkliftService.ImportForkliftsFromCsvAsync(stream),
                    FileTypes.Json => await _forkliftService.ImportForkliftsFromJsonAsync(stream),
                    _ => throw new ArgumentException(ForkliftMessages.UnsupportedFileFormat),
                };

                if (forklifts.Any())
                {
                    await _forkliftService.SaveForkliftsAsync(forklifts);
                }

                return Ok(forklifts);
            }
            catch
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ForkliftMessages.ImportingError
                );
            }
        }
    }
}
