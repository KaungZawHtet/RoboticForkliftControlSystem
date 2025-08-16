using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Data;
using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Services;

public class ForkliftService : IForkliftService
{
    private readonly AppDbContext _context;

    public ForkliftService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Forklift>> GetAllForkliftsAsync()
    {
        return await _context.Forklifts.ToListAsync();
    }

    public async Task<List<Forklift>> ImportForkliftsFromCsvAsync(Stream csvStream)
    {
        var forklifts = new List<Forklift>();
        using var reader = new StreamReader(csvStream);

        // Skip header
        await reader.ReadLineAsync();

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            var parts = line.Split(',');
            if (parts.Length >= 3)
            {
                if (DateTime.TryParse(parts[2], out DateTime manufacturingDate))
                {
                    forklifts.Add(
                        new Forklift
                        {
                            Name = parts[0].Trim('"'),
                            ModelNumber = parts[1].Trim('"'),
                            ManufacturingDate = manufacturingDate,
                        }
                    );
                }
            }
        }

        return forklifts;
    }

    public async Task<List<Forklift>> ImportForkliftsFromJsonAsync(Stream jsonStream)
    {
        var jsonString = await new StreamReader(jsonStream).ReadToEndAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var forklifts = JsonSerializer.Deserialize<List<Forklift>>(jsonString, options);
        return forklifts ?? new List<Forklift>();
    }

    public async Task SaveForkliftsAsync(List<Forklift> forklifts)
    {
        _context.Forklifts.AddRange(forklifts);
        await _context.SaveChangesAsync();
    }
}
