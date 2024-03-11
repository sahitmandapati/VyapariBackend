namespace Vyapari.Data;

public class BlackListRouteRepository : IBlackListRouteRepository
{
    private readonly VyapariDBContext _context;

    public BlackListRouteRepository(VyapariDBContext context)
    {
        _context = context;
    }

    public async Task<BlackListRoute> AddBlackListRoute(BlackListRoute route)
    {
        _context.BlackListRoutes.Add(route);
        await _context.SaveChangesAsync();
        return route;
    }

    // Implement other necessary methods here
}
