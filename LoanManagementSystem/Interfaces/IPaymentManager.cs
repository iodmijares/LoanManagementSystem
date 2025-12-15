namespace LoanManagementSystem.Interfaces;

using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;


public interface IPaymentManager
{
    Task<PaymentResult> ProcessPaymentAsync(int loanId, decimal amount, int processedByUserId, string paymentMethod = "Cash", string? referenceNumber = null);
    Task<decimal> CalculateEarlySettlementAmountAsync(int loanId);
    Task<Payment?> GetByReceiptNumberAsync(string receiptNumber);
    Task<IEnumerable<Payment>> GetByLoanIdAsync(int loanId);
    Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Payment>> GetTodaysCollectionsAsync();
    Task<decimal> GetTotalCollectionsByDateAsync(DateTime date);
}
