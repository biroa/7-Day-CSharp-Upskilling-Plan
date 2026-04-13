using System.ComponentModel.DataAnnotations;
using UserApiTest.Models;

namespace UserApiTest.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new List<User>();
    private int _nextId = 1;

    public Task<User?> GetUserById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> GetUserByEmail(string email)
    {
        var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<User> CreateUser(CreateUserDto dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true))
        {
            var errorMessages = string.Join("; ", validationResults.Select(result => result.ErrorMessage));
            throw new ValidationException($"Invalid create user payload: {errorMessages}");
        }

        var newUser = new User
        {
            Id = _nextId++,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
        };

        _users.Add(newUser);
        return Task.FromResult(newUser);
    }   

    public Task<List<User>> GetAllUsers()
    {
        return Task.FromResult(_users);
    }

    public Task<User?> UpdateUser(int id, CreateUserDto dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true))
        {
            var errorMessages = string.Join("; ", validationResults.Select(result => result.ErrorMessage));
            throw new ValidationException($"Invalid update user payload: {errorMessages}");
        }

        var existingUser = _users.FirstOrDefault(u => u.Id == id);
        if (existingUser is null)
        {
            return Task.FromResult<User?>(null);
        }

        existingUser.FirstName = dto.FirstName;
        existingUser.LastName = dto.LastName;
        existingUser.Email = dto.Email;
        existingUser.PhoneNumber = dto.PhoneNumber;
        existingUser.BirthDate = dto.BirthDate;
        existingUser.Address = dto.Address;
        existingUser.City = dto.City;
        existingUser.State = dto.State;
        existingUser.ZipCode = dto.ZipCode;

        return Task.FromResult<User?>(existingUser);
    }

    public Task<bool> DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            return Task.FromResult(false);
        }

        _users.Remove(user);
        return Task.FromResult(true);
    }

}