namespace LoanManagementSystem.Models.Users;

/// <summary>
/// Borrower/Customer Account (Optional)
/// - Submit loan application
/// - View loan status
/// - View payment history
/// </summary>
public class Borrower : User
{
    public int? CustomerId { get; set; } // Links to Customer record

    private readonly Permission _permissions =
        Permission.ViewDashboard |
        Permission.ViewOwnLoansOnly |     // Can only see their own loans
        Permission.CreateLoanApplication; // Can submit loan applications

    public Borrower()
    {
        Role = UserRole.Borrower;
    }

    public override bool HasPermission(Permission permission) => _permissions.HasFlag(permission);

    public override string GetDashboardFormName() => "BorrowerDashboardForm";
}
