namespace Vyapari.Data;

public interface IRouteRepository
{
    Task<WhiteListRoute> GetWhiteListRouteAsync(string route);
    Task<BlackListRoute> GetBlackListRouteAsync(string route);

}
