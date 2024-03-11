using Microsoft.EntityFrameworkCore;
using Vyapari.Data.Entities;

namespace Vyapari.Data;
public class UserRepository : IUserRepository
{
    private readonly VyapariDBContext _context;

    public UserRepository(VyapariDBContext context)
    {
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
