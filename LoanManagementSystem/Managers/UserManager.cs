using LoanManagementSystem.Data;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Utilities;

namespace LoanManagementSystem.Managers;

public class UserManager
{
    private readonly UserRepository _userRepository;

    public UserManager(LoanDbContext context)
    {
        _userRepository = new UserRepository(context);
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        return await _userRepository.AuthenticateAsync(username, password);
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        if (await _userRepository.UsernameExistsAsync(user.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        user.PasswordHash = PasswordHelper.HashPassword(password);
        user.CreatedAt = DateTime.Now;

        return await _userRepository.AddAsync(user);
    }

    public async Task<Admin> CreateAdminAsync(string username, string password, string fullName, string email, string phone)
    {
        var admin = new Admin
        {
            Username = username,
            FullName = fullName,
            Email = email,
            Phone = phone
        };

        return (Admin)await CreateUserAsync(admin, password);
    }

    public async Task<LoanOfficer> CreateLoanOfficerAsync(string username, string password, string fullName, string email, string phone, decimal approvalLimit)
    {
        var officer = new LoanOfficer
        {
            Username = username,
            FullName = fullName,
            Email = email,
            Phone = phone,
            ApprovalLimit = approvalLimit
        };

        return (LoanOfficer)await CreateUserAsync(officer, password);
    }

    public async Task<Cashier> CreateCashierAsync(string username, string password, string fullName, string email, string phone, decimal dailyCashLimit)
    {
        var cashier = new Cashier
        {
            Username = username,
            FullName = fullName,
            Email = email,
            Phone = phone,
            DailyCashLimit = dailyCashLimit
        };

        return (Cashier)await CreateUserAsync(cashier, password);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
    {
        return await _userRepository.GetByRoleAsync(role);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _userRepository.GetActiveUsersAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
    }

    public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found");

        if (!PasswordHelper.VerifyPassword(currentPassword, user.PasswordHash))
        {
            throw new InvalidOperationException("Current password is incorrect");
        }

        user.PasswordHash = PasswordHelper.HashPassword(newPassword);
        await _userRepository.UpdateAsync(user);
    }

    public async Task ResetPasswordAsync(int userId, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found");

        user.PasswordHash = PasswordHelper.HashPassword(newPassword);
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeactivateUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found");

        user.IsActive = false;
        await _userRepository.UpdateAsync(user);
    }

    public async Task ActivateUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found");

        user.IsActive = true;
        await _userRepository.UpdateAsync(user);
    }
}
