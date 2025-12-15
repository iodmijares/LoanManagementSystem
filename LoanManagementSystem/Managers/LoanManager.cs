using LoanManagementSystem.Data;
using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Utilities;

namespace LoanManagementSystem.Managers;

/// <summary>
/// Manages loan-related operations including computation, disbursement, and schedule generation.
/// 
/// Design Patterns:
/// - Repository Pattern: Uses LoanRepository for data access
/// - Factory Pattern: Uses InterestCalculatorFactory to create appropriate calculators
/// - Strategy Pattern: Different interest calculation strategies via IInterestCalculator
/// 
/// SOLID Principles:
/// - Single Responsibility: Handles only loan-related business logic
/// - Open/Closed: New interest calculation methods can be added without modifying this class
/// - Dependency Inversion: Depends on abstractions (IInterestCalculator, IRepository)
/// </summary>
public class LoanManager : ILoanManager
{
    private readonly LoanRepository _loanRepository;
    private readonly Repository<PaymentSchedule> _scheduleRepository;
    private readonly Repository<LoanProduct> _productRepository;
    private readonly LoanDbContext _context;

    public LoanManager(LoanDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _loanRepository = new LoanRepository(context);
        _scheduleRepository = new Repository<PaymentSchedule>(context);
        _productRepository = new Repository<LoanProduct>(context);
    }

    /// <summary>
    /// Computes loan details using the Strategy pattern for interest calculation.
    /// The appropriate calculator is selected based on the product's interest method.
    /// </summary>
    public LoanComputationResult ComputeLoan(LoanProduct product, decimal principal, int termMonths)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        if (principal <= 0)
            throw new ArgumentException("Principal must be greater than zero", nameof(principal));
        
        if (termMonths <= 0)
            throw new ArgumentException("Term must be greater than zero", nameof(termMonths));

        // Factory Pattern: Create the appropriate calculator based on interest method
        var calculator = InterestCalculatorFactory.Create(product.InterestMethod);

        decimal monthlyPayment = calculator.CalculateMonthlyPayment(principal, product.AnnualInterestRate, termMonths);
        decimal totalInterest = calculator.CalculateTotalInterest(principal, product.AnnualInterestRate, termMonths);
        decimal serviceCharge = Math.Round(principal * (product.ServiceChargePercent / 100), 2, MidpointRounding.AwayFromZero);
        decimal processingFee = product.ProcessingFeeFixed;
        decimal totalDeductions = serviceCharge + processingFee;
        decimal netProceeds = principal - totalDeductions;
        decimal totalPayable = principal + totalInterest;

        return new LoanComputationResult
        {
            Principal = principal,
            TermMonths = termMonths,
            AnnualInterestRate = product.AnnualInterestRate,
            InterestMethod = product.InterestMethod,
            MonthlyPayment = monthlyPayment,
            TotalInterest = totalInterest,
            TotalPayable = totalPayable,
            ServiceCharge = serviceCharge,
            ProcessingFee = processingFee,
            TotalDeductions = totalDeductions,
            NetProceeds = netProceeds
        };
    }

    /// <summary>
    /// Generates amortization schedule using the Strategy pattern.
    /// </summary>
    public List<AmortizationEntry> GenerateAmortizationSchedule(LoanProduct product, decimal principal, int termMonths, DateTime startDate)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        var calculator = InterestCalculatorFactory.Create(product.InterestMethod);
        return calculator.GenerateAmortizationSchedule(principal, product.AnnualInterestRate, termMonths, startDate);
    }

    /// <summary>
    /// Disburses a loan from an approved application.
    /// Creates the loan record and generates all payment schedules.
    /// </summary>
    public async Task<Loan> DisburseLoanAsync(LoanApplication application, int disbursedByUserId)
    {
        ArgumentNullException.ThrowIfNull(application);

        var product = await _productRepository.GetByIdAsync(application.LoanProductId)
            ?? throw new InvalidOperationException("Loan product not found");

        var computation = ComputeLoan(product, application.ApprovedAmount ?? application.RequestedAmount, application.RequestedTermMonths);

        var loan = CreateLoanFromComputation(application, computation, disbursedByUserId);
        var createdLoan = await _loanRepository.AddAsync(loan);

        // Generate and save payment schedules
        await CreatePaymentSchedulesAsync(createdLoan.Id, product, computation);

        return createdLoan;
    }

    #region Query Methods

    public async Task<Loan?> GetByIdAsync(int id) => await _loanRepository.GetByIdAsync(id);

    public async Task<Loan?> GetByLoanNumberAsync(string loanNumber) => await _loanRepository.GetByLoanNumberAsync(loanNumber);

    public async Task<Loan?> GetWithSchedulesAsync(int id) => await _loanRepository.GetWithSchedulesAsync(id);

    public async Task<IEnumerable<Loan>> GetAllAsync() => await _loanRepository.GetAllAsync();

    public async Task<IEnumerable<Loan>> GetByCustomerIdAsync(int customerId) => await _loanRepository.GetByCustomerIdAsync(customerId);

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status) => await _loanRepository.GetByStatusAsync(status);

    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync() => await _loanRepository.GetOverdueLoansAsync();

    public async Task<IEnumerable<Loan>> GetMaturedLoansAsync() => await _loanRepository.GetMaturedLoansAsync();

    public async Task<decimal> GetTotalOutstandingBalanceAsync() => await _loanRepository.GetTotalOutstandingBalanceAsync();

    public async Task<IEnumerable<Loan>> SearchByCustomerNameAsync(string customerName) => 
        await _loanRepository.SearchByCustomerNameAsync(customerName);

    #endregion

    public async Task UpdateLoanAsync(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        loan.UpdatedAt = DateTime.Now;
        await _loanRepository.UpdateAsync(loan);
    }

    #region Private Helper Methods

    private static Loan CreateLoanFromComputation(LoanApplication application, LoanComputationResult computation, int disbursedByUserId)
    {
        return new Loan
        {
            LoanNumber = GenerateLoanNumber(),
            LoanApplicationId = application.Id,
            CustomerId = application.CustomerId,
            LoanProductId = application.LoanProductId,
            PrincipalAmount = computation.Principal,
            AnnualInterestRate = computation.AnnualInterestRate,
            InterestMethod = computation.InterestMethod,
            TermMonths = computation.TermMonths,
            MonthlyPayment = computation.MonthlyPayment,
            TotalInterest = computation.TotalInterest,
            TotalPayable = computation.TotalPayable,
            ServiceCharge = computation.ServiceCharge,
            ProcessingFee = computation.ProcessingFee,
            OutstandingPrincipal = computation.Principal,
            DisbursementDate = DateTime.Now,
            FirstPaymentDueDate = DateTime.Now.AddMonths(1),
            MaturityDate = DateTime.Now.AddMonths(computation.TermMonths),
            Status = LoanStatus.Active,
            DisbursedByUserId = disbursedByUserId
        };
    }

    private async Task CreatePaymentSchedulesAsync(int loanId, LoanProduct product, LoanComputationResult computation)
    {
        var schedules = GenerateAmortizationSchedule(product, computation.Principal, computation.TermMonths, DateTime.Now);
        
        foreach (var entry in schedules)
        {
            var schedule = new PaymentSchedule
            {
                LoanId = loanId,
                InstallmentNumber = entry.Period,
                DueDate = entry.DueDate,
                PrincipalDue = entry.Principal,
                InterestDue = entry.Interest,
                TotalDue = entry.Payment,
                PrincipalBalance = entry.EndingBalance,
                Status = PaymentStatus.Pending
            };
            await _scheduleRepository.AddAsync(schedule);
        }
    }

    private static string GenerateLoanNumber()
    {
        return $"LN-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    #endregion
}

/// <summary>
/// Represents the result of a loan computation.
/// Immutable data transfer object (DTO) containing all calculated loan values.
/// </summary>
public class LoanComputationResult
{
    public decimal Principal { get; init; }
    public int TermMonths { get; init; }
    public decimal AnnualInterestRate { get; init; }
    public InterestCalculationMethod InterestMethod { get; init; }
    public decimal MonthlyPayment { get; init; }
    public decimal TotalInterest { get; init; }
    public decimal TotalPayable { get; init; }
    public decimal ServiceCharge { get; init; }
    public decimal ProcessingFee { get; init; }
    public decimal TotalDeductions { get; init; }
    public decimal NetProceeds { get; init; }
}
