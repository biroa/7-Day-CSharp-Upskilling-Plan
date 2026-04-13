using UserApiTest.Models;

namespace UserApiTest.Services;

public interface IUserService
{
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByEmail(string email);
    Task<User> CreateUser(CreateUserDto user);
    Task<List<User>> GetAllUsers();
    Task<User?> UpdateUser(int id, CreateUserDto user);
    Task<bool> DeleteUser(int id);
}