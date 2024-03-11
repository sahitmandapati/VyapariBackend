using Vyapari.Data;

namespace Vyapari.Service;

public class BlackListRouteService : IBlackListRouteService
{
    private readonly IBlackListRouteRepository _repository;

    public BlackListRouteService(IBlackListRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlackListRoute> AddBlackListRoute(BlackListRoute route)
    {
        return await _repository.AddBlackListRoute(route);
    }

    // Implement other necessary methods here
}
