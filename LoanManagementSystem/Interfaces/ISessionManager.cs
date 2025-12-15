namespace LoanManagementSystem.Interfaces;

using LoanManagementSystem.Models.Users;


public interface ISessionManager
{
    User? CurrentUser { get; }
    bool IsLoggedIn { get; }
    void Login(User user);
    void Logout();
    bool HasPermission(Permission permission);
    string GetCurrentUsername();
    int GetCurrentUserId();
}
