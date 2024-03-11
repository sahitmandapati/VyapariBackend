﻿using Vyapari.Data.Entities;

namespace Vyapari.Data;
public interface IUserRepository
{

    Task<User> GetByEmailAsync(string email);

    Task<User> GetByIdAsync(int id);
    Task AddAsync(User user);
    
    Task SaveChangesAsync();
}


