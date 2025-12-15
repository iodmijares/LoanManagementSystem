namespace LoanManagementSystem.Models.Users;

/// <summary>
/// Loan Officer Account
/// - Process loan applications
/// - Evaluate creditworthiness
/// - Approve/reject loans (within authority limit)
/// - Monitor assigned portfolios
/// - View customers and reports
/// </summary>
public class LoanOfficer : User
{
    public decimal ApprovalLimit { get; set; } = 100000M;
    public int AssignedPortfolioCount { get; set; }

    private readonly Permission _permissions =
        Permission.ViewDashboard |
        Permission.ViewLoans |
        Permission.ViewCustomers |
        Permission.ViewReports |
        Permission.ViewProducts |
        Permission.CreateLoanApplication |
        Permission.ApproveLoan |
        Permission.RejectLoan |
        Permission.ManageCustomers; // Can register and manage customers

    public LoanOfficer()
    {
        Role = UserRole.LoanOfficer;
    }

    public override bool HasPermission(Permission permission) => _permissions.HasFlag(permission);

    public override string GetDashboardFormName() => "OfficerDashboardForm";

    /// <summary>
    /// Check if the loan officer can approve loans up to a certain amount
    /// </summary>
    public bool CanApproveLoan(decimal amount) => amount <= ApprovalLimit;
}
