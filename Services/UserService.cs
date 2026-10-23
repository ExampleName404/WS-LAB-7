using BankProductsAPI.Models;

namespace BankProductsAPI.Services;

public class UserService
{
    private readonly List<User> _users = new()
    {
        new User { Id = 1, UserName = "AdminUser", Password = "Admin123", Email = "admin@bank.test", Roles = new() { "Admin" } },
        new User { Id = 2, UserName = "ManagerUser", Password = "Manager123", Email = "manager@bank.test", Roles = new() { "Manager" } },
        new User { Id = 3, UserName = "ClientUser", Password = "Client123", Email = "client@bank.test", Roles = new() { "Client" } }
    };

    public User? Validate(string userName, string password)
    {
        return _users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
    }
}
