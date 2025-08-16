using Microsoft.AspNetCore.Mvc;
using RoboticForkliftControlSystem.Api.Abstractions;
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("import")]
        public async Task<ActionResult<List<Forklift>>> ImportForklifts(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            try
            {
                List<Forklift> forklifts;
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                using var stream = file.OpenReadStream();

                if (fileExtension == ".csv")
                {
                    forklifts = await _forkliftService.ImportForkliftsFromCsvAsync(stream);
                }
                else if (fileExtension == ".json")
                {
                    forklifts = await _forkliftService.ImportForkliftsFromJsonAsync(stream);
                }
                else
                {
                    return BadRequest("Unsupported file format. Please upload a CSV or JSON file.");
                }

                if (forklifts.Any())
                {
                    await _forkliftService.SaveForkliftsAsync(forklifts);
                }

                return Ok(forklifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error importing forklifts: {ex.Message}");
            }
        }
    }
}
