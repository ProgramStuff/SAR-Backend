using SAR_API.Domains;

namespace SAR_API.Repositories;

public class IncidentRepository
{
    Task<int> AddIncident(NewIncidentRequest request)
    {
        // Add incident to database
        // Placeholder return value
        return Task.FromResult(1);
    }
}