namespace LoanManagementSystem.Interfaces;

using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;

public interface ILoanManager
{
    LoanComputationResult ComputeLoan(LoanProduct product, decimal principal, int termMonths);
    List<AmortizationEntry> GenerateAmortizationSchedule(LoanProduct product, decimal principal, int termMonths, DateTime startDate);
    Task<Loan> DisburseLoanAsync(LoanApplication application, int disbursedByUserId);
    Task<Loan?> GetByIdAsync(int id);
    Task<Loan?> GetByLoanNumberAsync(string loanNumber);
    Task<Loan?> GetWithSchedulesAsync(int id);
    Task<IEnumerable<Loan>> GetAllAsync();
    Task<IEnumerable<Loan>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<IEnumerable<Loan>> GetMaturedLoansAsync();
    Task<decimal> GetTotalOutstandingBalanceAsync();
    Task<IEnumerable<Loan>> SearchByCustomerNameAsync(string customerName);
    Task UpdateLoanAsync(Loan loan);
}
