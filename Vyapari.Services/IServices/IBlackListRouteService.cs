using Vyapari.Data;

namespace Vyapari.Service;

public interface IBlackListRouteService
{
    Task<BlackListRoute> AddBlackListRoute(BlackListRoute route);
    // Add other necessary methods here
}
