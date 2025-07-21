using AutoMapper;
using Common.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.API.Services.Services;

internal class UserService(ILogger<UserService> logger, SocializerDbContext dbContext, UserManager<ApplicationUser> userManager, IMapper mapper) : IUserService
{
    public async Task<OperationResult<UserDto>> GetUserAsync(Guid userId)
    {
        var user = await dbContext.Users
            .SingleAsync(x => x.Id == userId);

        return OperationResult<UserDto>.Success(mapper.Map<UserDto>(user));
    }

    public async Task<OperationResult<CreateUserDto>> CreateUserAsync(CreateUserDto newUserDto)
    {
        // Duplicate validation in Frontend
        newUserDto.Email = newUserDto.Email.ToLowerInvariant();

        var validationResult = newUserDto.Validate();
        if (!validationResult.IsSuccess)
            return validationResult;

        // Validate username and email uniqueness
        if (dbContext.Users.Any(x => x.Email == newUserDto.Email))
            return OperationResult<CreateUserDto>.Failure($"User with email '{newUserDto.Email}' already exists.");

        if (dbContext.Users.Any(x => x.Username == newUserDto.Username))
            return OperationResult<CreateUserDto>.Failure($"User with name '{newUserDto.Username}' already exists.");

        // Add user to identity

        logger.LogInformation("Creating new user in Identity with username: {Username} and email: {Email}", newUserDto.Username, newUserDto.Email);

        var applicationUser = new ApplicationUser
        {
            UserName = newUserDto.Username,
            Email = newUserDto.Email
        };

        var identityResult = await userManager.CreateAsync(applicationUser, newUserDto.Password);
        newUserDto.Password = string.Empty; // Clear password for security reasons

        if (!identityResult.Succeeded)
        {
            return OperationResult<CreateUserDto>.Failure(identityResult.Errors.Select(x => x.Description));
        }

        // Add user to Users table
        // TODO: In case of failure identity should be removed in transaction manner

        logger.LogInformation("Adding new user to Users table with username: {Username} and email: {Email}", newUserDto.Username, newUserDto.Email);

        var newUser = mapper.Map<User>(newUserDto);
        newUser.Id = new Guid(applicationUser.Id); // Set the same ID as in Identity

        var addedUser = await dbContext.Users.AddAsync(newUser);
        await dbContext.SaveChangesAsync();

        return OperationResult<CreateUserDto>.Success(newUserDto);
    }
}
