using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using UserApiTest.Data;
using UserApiTest.Models;

namespace UserApiTest.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    // JSON dates deserialize as DateTimeKind.Unspecified; Npgsql cannot persist that to timestamptz.
    private static DateTime BirthDateForStore(DateTime birthDate) =>
        DateTime.SpecifyKind(birthDate.Date, DateTimeKind.Utc);

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserById(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail);
    }

    public async Task<User> CreateUser(CreateUserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true))
        {
            var errorMessages = string.Join("; ", validationResults.Select(result => result.ErrorMessage));
            throw new ValidationException($"Invalid create user payload: {errorMessages}");
        }

        var newUser = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email.Trim().ToLowerInvariant(),
            PhoneNumber = dto.PhoneNumber,
            BirthDate = BirthDateForStore(dto.BirthDate),
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return newUser;
    }   

    public async Task<List<User>> GetAllUsers()
    {
        return await _dbContext.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> UpdateUser(int id, CreateUserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true))
        {
            var errorMessages = string.Join("; ", validationResults.Select(result => result.ErrorMessage));
            throw new ValidationException($"Invalid update user payload: {errorMessages}");
        }

        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existingUser is null)
        {
            return null;
        }

        existingUser.FirstName = dto.FirstName;
        existingUser.LastName = dto.LastName;
        existingUser.Email = dto.Email.Trim().ToLowerInvariant();
        existingUser.PhoneNumber = dto.PhoneNumber;
        existingUser.BirthDate = BirthDateForStore(dto.BirthDate);
        existingUser.Address = dto.Address;
        existingUser.City = dto.City;
        existingUser.State = dto.State;
        existingUser.ZipCode = dto.ZipCode;

        await _dbContext.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return false;
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}