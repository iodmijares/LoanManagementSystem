using LoanManagementSystem.Models.Users;

namespace LoanManagementSystem.Services;

public static class SessionManager
{
    public static User? CurrentUser { get; private set; }
    public static bool IsLoggedIn => CurrentUser != null;

    public static void Login(User user)
    {
        CurrentUser = user;
    }

    public static void Logout()
    {
        CurrentUser = null;
    }

    public static bool HasPermission(Permission permission)
    {
        return CurrentUser?.HasPermission(permission) ?? false;
    }

    public static string GetCurrentUsername()
    {
        return CurrentUser?.Username ?? "System";
    }

    public static int GetCurrentUserId()
    {
        return CurrentUser?.Id ?? 0;
    }
}
