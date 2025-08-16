using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Abstractions;

public interface IForkliftService
{
    Task<List<Forklift>> GetAllForkliftsAsync();
    Task<List<Forklift>> ImportForkliftsFromCsvAsync(Stream csvStream);
    Task<List<Forklift>> ImportForkliftsFromJsonAsync(Stream jsonStream);
    Task SaveForkliftsAsync(List<Forklift> forklifts);
 }
