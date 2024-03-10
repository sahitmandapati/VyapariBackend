using Vyapari.Data.Entities;
using Vyapari.Infra;

namespace Vyapari.Service;
public interface IUserService
{

    Task<UserDto> AuthenticateAsync(string username, string password);
    Task<User> CreateAsync(User user, string password);
}
