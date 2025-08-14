using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> GetByEmailAsync(string email);
}