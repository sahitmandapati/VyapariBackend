using Vyapari.Data;

namespace Vyapari.Service;

public class RouteService :IRouteService
{

    private readonly IRouteRepository _routeRepository;

    public RouteService(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<BlackListRoute> GetBlackListedRoute(string route)
    {
        return await _routeRepository.GetBlackListRouteAsync(route);
    }

    public bool IsBlackListed(string route)
    {
        return _routeRepository.GetBlackListRouteAsync(route).Result != null;
    }

    public bool IsWhiteListed(string route)
    {
        return _routeRepository.GetWhiteListRouteAsync(route).Result != null;
    }

}
