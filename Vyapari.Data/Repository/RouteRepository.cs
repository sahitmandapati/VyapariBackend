using Microsoft.EntityFrameworkCore;

namespace Vyapari.Data;

public class RouteRepository : IRouteRepository
{

    private readonly VyapariDBContext _context;

    public RouteRepository(VyapariDBContext context)
    {
        _context = context;
    }

    public async Task<WhiteListRoute> GetWhiteListRouteAsync(string route)
    {
        return await _context.WhiteListRoutes.FirstOrDefaultAsync(x => x.Route == route);
    }

    public async Task<BlackListRoute> GetBlackListRouteAsync(string route)
    {
        return await _context.BlackListRoutes.Include(x => x.AllowedRoles).FirstOrDefaultAsync(x => x.Route == route);
    }

}
