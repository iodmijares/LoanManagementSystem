using LoanManagementSystem.Data;
using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;

namespace LoanManagementSystem.Managers;

/// <summary>
/// Manages loan application workflow including creation, approval, rejection, and release.
/// 
/// Design Patterns:
/// - Repository Pattern: Uses repositories for data access
/// - State Pattern: Application status transitions follow defined rules
/// 
/// SOLID Principles:
/// - Single Responsibility: Handles only loan application workflow
/// - Open/Closed: New application statuses can be added with minimal changes
/// - Dependency Inversion: Depends on abstractions (IRepository, ILoanManager)
/// </summary>
public class LoanApplicationManager : ILoanApplicationManager
{
    private readonly LoanApplicationRepository _applicationRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly Repository<LoanProduct> _productRepository;
    private readonly ILoanManager _loanManager;

    public LoanApplicationManager(LoanDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _applicationRepository = new LoanApplicationRepository(context);
        _customerRepository = new CustomerRepository(context);
        _productRepository = new Repository<LoanProduct>(context);
        _loanManager = new LoanManager(context);
    }

    /// <summary>
    /// Constructor supporting dependency injection for better testability.
    /// </summary>
    public LoanApplicationManager(
        LoanApplicationRepository applicationRepository,
        CustomerRepository customerRepository,
        Repository<LoanProduct> productRepository,
        ILoanManager loanManager)
    {
        _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _loanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
    }

    /// <summary>
    /// Creates a new loan application with computed values.
    /// Validates all business rules before creating the application.
    /// </summary>
    public async Task<LoanApplication> CreateApplicationAsync(LoanApplication application, int processedByUserId)
    {
        ArgumentNullException.ThrowIfNull(application);

        var customer = await _customerRepository.GetByIdAsync(application.CustomerId)
            ?? throw new InvalidOperationException("Customer not found");

        var product = await _productRepository.GetByIdAsync(application.LoanProductId)
            ?? throw new InvalidOperationException("Loan product not found");

        // Validate business rules
        ValidateLoanAmount(application.RequestedAmount, product);
        ValidateLoanTerm(application.RequestedTermMonths, product);

        // Compute loan details using LoanManager
        var computation = _loanManager.ComputeLoan(product, application.RequestedAmount, application.RequestedTermMonths);

        // Populate application details
        PopulateApplicationDetails(application, computation, customer, processedByUserId);

        return await _applicationRepository.AddAsync(application);
    }

    #region Status Transition Methods

    /// <summary>
    /// Approves an application. Only Pending or UnderReview applications can be approved.
    /// </summary>
    public async Task ApproveApplicationAsync(
        int applicationId, 
        int approvedByUserId, 
        decimal? approvedAmount = null, 
        string? conditions = null)
    {
        var application = await GetAndValidateApplicationForTransition(applicationId, 
            [ApplicationStatus.Pending, ApplicationStatus.UnderReview]);

        application.Status = ApplicationStatus.Approved;
        application.ApprovedByUserId = approvedByUserId;
        application.ApprovedAt = DateTime.Now;
        application.ApprovedAmount = approvedAmount ?? application.RequestedAmount;
        application.ApprovalConditions = conditions;
        application.UpdatedAt = DateTime.Now;

        await _applicationRepository.UpdateAsync(application);
    }

    /// <summary>
    /// Rejects an application with a reason. Only Pending or UnderReview applications can be rejected.
    /// </summary>
    public async Task RejectApplicationAsync(int applicationId, int rejectedByUserId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason is required", nameof(reason));

        var application = await GetAndValidateApplicationForTransition(applicationId,
            [ApplicationStatus.Pending, ApplicationStatus.UnderReview]);

        application.Status = ApplicationStatus.Rejected;
        application.ApprovedByUserId = rejectedByUserId;
        application.RejectionReason = reason;
        application.UpdatedAt = DateTime.Now;

        await _applicationRepository.UpdateAsync(application);
    }

    /// <summary>
    /// Releases an approved application, creating the actual loan.
    /// Only Approved applications can be released.
    /// </summary>
    public async Task<Loan> ReleaseApplicationAsync(int applicationId, int disbursedByUserId)
    {
        var application = await _applicationRepository.GetWithDetailsAsync(applicationId)
            ?? throw new InvalidOperationException("Application not found");

        if (application.Status != ApplicationStatus.Approved)
            throw new InvalidOperationException("Only approved applications can be released");

        // Disburse the loan using LoanManager
        var loan = await _loanManager.DisburseLoanAsync(application, disbursedByUserId);

        // Update application status
        application.Status = ApplicationStatus.Released;
        application.UpdatedAt = DateTime.Now;
        await _applicationRepository.UpdateAsync(application);

        return loan;
    }

    /// <summary>
    /// Moves an application to UnderReview status for additional evaluation.
    /// </summary>
    public async Task SetUnderReviewAsync(int applicationId)
    {
        var application = await GetAndValidateApplicationForTransition(applicationId,
            [ApplicationStatus.Pending]);

        application.Status = ApplicationStatus.UnderReview;
        application.UpdatedAt = DateTime.Now;

        await _applicationRepository.UpdateAsync(application);
    }

    /// <summary>
    /// Cancels an application. Released applications cannot be cancelled.
    /// </summary>
    public async Task CancelApplicationAsync(int applicationId)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId)
            ?? throw new InvalidOperationException("Application not found");

        if (application.Status == ApplicationStatus.Released)
            throw new InvalidOperationException("Released applications cannot be cancelled");

        application.Status = ApplicationStatus.Cancelled;
        application.UpdatedAt = DateTime.Now;

        await _applicationRepository.UpdateAsync(application);
    }

    #endregion

    #region Query Methods

    public async Task<LoanApplication?> GetByIdAsync(int id) => 
        await _applicationRepository.GetByIdAsync(id);

    public async Task<LoanApplication?> GetByApplicationNumberAsync(string applicationNumber) => 
        await _applicationRepository.GetByApplicationNumberAsync(applicationNumber);

    public async Task<LoanApplication?> GetWithDetailsAsync(int id) => 
        await _applicationRepository.GetWithDetailsAsync(id);

    public async Task<IEnumerable<LoanApplication>> GetAllAsync() => 
        await _applicationRepository.GetAllAsync();

    public async Task<IEnumerable<LoanApplication>> GetByStatusAsync(ApplicationStatus status) => 
        await _applicationRepository.GetByStatusAsync(status);

    public async Task<IEnumerable<LoanApplication>> GetByCustomerIdAsync(int customerId) => 
        await _applicationRepository.GetByCustomerIdAsync(customerId);

    public async Task<IEnumerable<LoanApplication>> GetPendingApprovalsAsync() => 
        await _applicationRepository.GetPendingApprovalsAsync();

    #endregion

    #region Private Helper Methods

    private static void ValidateLoanAmount(decimal amount, LoanProduct product)
    {
        if (amount < product.MinimumAmount || amount > product.MaximumAmount)
        {
            throw new InvalidOperationException(
                $"Loan amount must be between {product.MinimumAmount:C} and {product.MaximumAmount:C}");
        }
    }

    private static void ValidateLoanTerm(int term, LoanProduct product)
    {
        var availableTerms = product.GetAvailableTerms();
        if (!availableTerms.Contains(term))
        {
            throw new InvalidOperationException(
                $"Invalid loan term. Available terms: {string.Join(", ", availableTerms)} months");
        }
    }

    private static void PopulateApplicationDetails(
        LoanApplication application, 
        LoanComputationResult computation, 
        Customer customer, 
        int processedByUserId)
    {
        application.ApplicationNumber = GenerateApplicationNumber();
        application.ProcessedByUserId = processedByUserId;
        application.ComputedMonthlyPayment = computation.MonthlyPayment;
        application.ComputedTotalInterest = computation.TotalInterest;
        application.ComputedTotalPayable = computation.TotalPayable;
        application.ServiceCharge = computation.ServiceCharge;
        application.ProcessingFee = computation.ProcessingFee;
        application.NetProceedsAmount = computation.NetProceeds;
        application.CreditScoreAtApplication = customer.CreditScore;
        application.Status = ApplicationStatus.Pending;
        application.ApplicationDate = DateTime.Now;
    }

    private async Task<LoanApplication> GetAndValidateApplicationForTransition(
        int applicationId, 
        ApplicationStatus[] allowedStatuses)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId)
            ?? throw new InvalidOperationException("Application not found");

        if (!allowedStatuses.Contains(application.Status))
        {
            throw new InvalidOperationException(
                $"Application cannot be modified in its current status: {application.Status}");
        }

        return application;
    }

    private static string GenerateApplicationNumber()
    {
        return $"APP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }

    #endregion
}
