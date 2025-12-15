namespace LoanManagementSystem.Interfaces;

using LoanManagementSystem.Models;

public interface ICustomerManager
{
    Task<Customer> RegisterCustomerAsync(Customer customer);
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByCustomerIdAsync(string customerId);
    Task<Customer?> GetWithLoansAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<IEnumerable<Customer>> SearchAsync(string searchTerm);
    Task<IEnumerable<Customer>> GetByClassificationAsync(CustomerClassification classification);
    Task UpdateCustomerAsync(Customer customer);
    Task UpdateCreditScoreAsync(int customerId);
    Task<bool> BlacklistCustomerAsync(int customerId);
    string GetCreditRating(decimal score);
    bool IsEligibleForLoan(decimal score, decimal requestedAmount);
}
