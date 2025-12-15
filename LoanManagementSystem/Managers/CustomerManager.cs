using LoanManagementSystem.Data;
using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Utilities;

namespace LoanManagementSystem.Managers;

/// <summary>
/// Manages customer-related operations including registration, credit scoring, and blacklisting.
/// 
/// Design Patterns:
/// - Repository Pattern: Uses CustomerRepository for data access
/// - Strategy Pattern: Uses ICreditScoreCalculator for flexible scoring algorithms
/// 
/// SOLID Principles:
/// - Single Responsibility: Handles only customer-related business logic
/// - Open/Closed: New credit scoring algorithms can be added without modifying this class
/// - Dependency Inversion: Depends on abstractions (ICreditScoreCalculator, IRepository)
/// </summary>
public class CustomerManager : ICustomerManager
{
    private readonly CustomerRepository _customerRepository;
    private readonly ICreditScoreCalculator _creditScoreCalculator;

    public CustomerManager(LoanDbContext context) 
        : this(new CustomerRepository(context), new CreditScoreCalculator())
    {
    }

    /// <summary>
    /// Constructor supporting dependency injection for better testability.
    /// </summary>
    public CustomerManager(CustomerRepository customerRepository, ICreditScoreCalculator creditScoreCalculator)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _creditScoreCalculator = creditScoreCalculator ?? throw new ArgumentNullException(nameof(creditScoreCalculator));
    }

    public async Task<Customer> RegisterCustomerAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        customer.CustomerId = GenerateCustomerId();
        customer.CreatedAt = DateTime.Now;

        // Calculate initial credit score using Strategy pattern
        var creditInput = CreateInitialCreditScoreInput(customer);
        customer.CreditScore = _creditScoreCalculator.CalculateCreditScore(creditInput);

        return await _customerRepository.AddAsync(customer);
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer?> GetByCustomerIdAsync(string customerId)
    {
        return await _customerRepository.GetByCustomerIdAsync(customerId);
    }

    public async Task<Customer?> GetWithLoansAsync(int id)
    {
        return await _customerRepository.GetWithLoansAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
    {
        return await _customerRepository.SearchAsync(searchTerm);
    }

    public async Task<IEnumerable<Customer>> GetByClassificationAsync(CustomerClassification classification)
    {
        return await _customerRepository.GetByClassificationAsync(classification);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        customer.UpdatedAt = DateTime.Now;
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task UpdateCreditScoreAsync(int customerId)
    {
        var customer = await _customerRepository.GetWithLoansAsync(customerId)
            ?? throw new InvalidOperationException("Customer not found");

        var creditInput = CalculateCreditInputFromHistory(customer);
        customer.CreditScore = _creditScoreCalculator.CalculateCreditScore(creditInput);
        customer.UpdatedAt = DateTime.Now;

        await _customerRepository.UpdateAsync(customer);
    }

    public async Task<bool> BlacklistCustomerAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId)
            ?? throw new InvalidOperationException("Customer not found");

        customer.Classification = CustomerClassification.Blacklisted;
        customer.UpdatedAt = DateTime.Now;

        await _customerRepository.UpdateAsync(customer);
        return true;
    }

    public string GetCreditRating(decimal score) => _creditScoreCalculator.GetCreditRating(score);

    public bool IsEligibleForLoan(decimal score, decimal requestedAmount) =>
        _creditScoreCalculator.IsEligibleForLoan(score, requestedAmount);

    #region Private Helper Methods

    private static CreditScoreInput CreateInitialCreditScoreInput(Customer customer)
    {
        return new CreditScoreInput
        {
            PaymentHistoryScore = 70, 
            CreditUtilization = 0,
            CreditHistoryMonths = 0,
            MonthlyIncome = customer.MonthlyIncome,
            YearsEmployed = customer.YearsEmployed,
            ExistingLoanCount = 0,
            LatePaymentCount = 0
        };
    }

    private static CreditScoreInput CalculateCreditInputFromHistory(Customer customer)
    {
        int latePayments = customer.Loans
            .SelectMany(l => l.PaymentSchedules)
            .Count(s => s.PaidDate.HasValue && s.PaidDate > s.DueDate);

        int onTimePayments = customer.Loans
            .SelectMany(l => l.PaymentSchedules)
            .Count(s => s.Status == PaymentStatus.Paid);

        decimal paymentHistoryScore = onTimePayments > 0
            ? (decimal)onTimePayments / (onTimePayments + latePayments) * 100
            : 70;

        decimal totalCreditLimit = customer.Loans.Sum(l => l.PrincipalAmount);
        decimal totalOutstanding = customer.Loans.Sum(l => l.OutstandingPrincipal);
        decimal creditUtilization = totalCreditLimit > 0
            ? (totalOutstanding / totalCreditLimit) * 100
            : 0;

        int creditHistoryMonths = customer.Loans.Any()
            ? (int)(DateTime.Now - customer.Loans.Min(l => l.DisbursementDate)).TotalDays / 30
            : 0;

        return new CreditScoreInput
        {
            PaymentHistoryScore = paymentHistoryScore,
            CreditUtilization = creditUtilization,
            CreditHistoryMonths = creditHistoryMonths,
            MonthlyIncome = customer.MonthlyIncome,
            YearsEmployed = customer.YearsEmployed,
            ExistingLoanCount = customer.Loans.Count(l => l.Status == LoanStatus.Active),
            LatePaymentCount = latePayments
        };
    }

    private static string GenerateCustomerId()
    {
        return $"CUST-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }

    #endregion
}
