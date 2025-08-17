using System.Globalization;
using System.Text.Json;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Data;
using RoboticForkliftControlSystem.Api.Entities;
using RoboticForkliftControlSystem.Api.Utilities;

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
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // Register the mapping
        csv.Context.RegisterClassMap<ForkliftCsvMap>();

        // Read records
        var records = csv.GetRecords<Forklift>().ToList();

        return await Task.FromResult(records);
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
