using Microsoft.EntityFrameworkCore;
using MindScoreApi.Data;
using MindScoreApi.Models;

namespace MindScoreApi.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message, User? user)> RegisterAsync(string name, string email, string password, int age, string gender);
        Task<(bool success, string message, User? user)> LoginAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;

        public AuthService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool success, string message, User? user)> RegisterAsync(string name, string email, string password, int age, string gender)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (existingUser != null)
                {
                    return (false, "User already exists.", null);
                }

                var user = new User
                {
                    Name = name,
                    Email = email,
                    Password = password,
                    Age = age,
                    Gender = gender
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                return (true, "User registered successfully.", user);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred during registration: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, User? user)> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x =>
                        x.Email == email &&
                        x.Password == password);

                if (user == null)
                {
                    return (false, "Invalid email or password. User does not exist.", null);
                }

                return (true, "Login successful.", user);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred during login: {ex.Message}", null);
            }
        }
    }
}