namespace LoanManagementSystem.Models.Users;

/// <summary>
/// Admin Account - Full system access
/// - User management
/// - System configuration
/// - Override capabilities with audit
/// </summary>
public class Admin : User
{
    public Admin()
    {
        Role = UserRole.Admin;
    }

    // Full access to all features
    public override bool HasPermission(Permission permission) => true;

    public override string GetDashboardFormName() => "AdminDashboardForm";
}
