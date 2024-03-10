﻿using AutoMapper;
using Vyapari.Data;
using Vyapari.Data.Entities;
using Vyapari.Infra;

namespace Vyapari.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Email and password are required.");

        var user = await _userRepository.GetByEmailAsync(email);

        // check if username exists
        if (user == null)
            throw new ArgumentException("User not found.");

        // check if password is correct
        if (!VerifyPasswordHash(password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
            throw new ArgumentException("Password is incorrect.");

        // authentication successful
         var userDto = _mapper.Map<UserDto>(user);
         return userDto;
    }

    public async Task<User> CreateAsync(User user, string password)
    {
        // validation
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        if (await _userRepository.GetByEmailAsync(user.Email) != null)
            throw new ArgumentException($"Email \"{user.Email}\" is already taken.");

        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = Convert.ToBase64String(passwordHash);
        user.PasswordSalt = Convert.ToBase64String(passwordSalt);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }



    // private helper methods
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }

        return true;
    }
}