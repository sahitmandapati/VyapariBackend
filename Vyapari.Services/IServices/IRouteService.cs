using Vyapari.Data;

namespace Vyapari.Service;

public interface IRouteService
{

    bool IsWhiteListed(string route);
    bool IsBlackListed(string route);
    Task<BlackListRoute> GetBlackListedRoute(string route);
}

