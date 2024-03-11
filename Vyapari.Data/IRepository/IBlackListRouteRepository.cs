namespace Vyapari.Data;

public interface IBlackListRouteRepository
{
    Task<BlackListRoute> AddBlackListRoute(BlackListRoute route);
    // Add other necessary methods here
}
