namespace LoanManagementSystem.Models.Users;

/// <summary>
/// Cashier/Teller Account
/// - Process payments
/// - Release loan proceeds
/// - Generate receipts
/// - Daily cash report
/// </summary>
public class Cashier : User
{
    public decimal DailyCashLimit { get; set; } = 500000M;
    public decimal CurrentDayTotal { get; set; }

    private readonly Permission _permissions =
        Permission.ViewDashboard |
        Permission.ViewLoans |       // Can view loans to process payments
        Permission.ViewCustomers |   // Can view customer info
        Permission.ProcessPayment |
        Permission.GenerateReceipt |
        Permission.ViewDailyCollections |
        Permission.ReleaseLoan;      // Can release approved loans (disbursement)

    public Cashier()
    {
        Role = UserRole.Cashier;
    }

    public override bool HasPermission(Permission permission) => _permissions.HasFlag(permission);

    public override string GetDashboardFormName() => "CashierDashboardForm";

    /// <summary>
    /// Check if cashier has exceeded daily limit
    /// </summary>
    public bool CanProcessAmount(decimal amount) => (CurrentDayTotal + amount) <= DailyCashLimit;

    /// <summary>
    /// Add to daily total after processing a transaction
    /// </summary>
    public void AddToDailyTotal(decimal amount) => CurrentDayTotal += amount;

    /// <summary>
    /// Reset daily total (called at end of day)
    /// </summary>
    public void ResetDailyTotal() => CurrentDayTotal = 0;
}
