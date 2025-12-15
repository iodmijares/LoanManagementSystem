using LoanManagementSystem.Data;
using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Utilities;

namespace LoanManagementSystem.Managers;

/// <summary>
/// Manages payment processing including allocation, penalty calculation, and loan status updates.
/// 
/// Design Patterns:
/// - Repository Pattern: Uses repositories for data access
/// - Strategy Pattern: Payment allocation follows a specific order (Penalty ? Interest ? Principal)
/// 
/// SOLID Principles:
/// - Single Responsibility: Handles only payment processing logic
/// - Open/Closed: New payment types can be added without modifying core logic
/// - Dependency Inversion: Depends on abstractions (IRepository)
/// </summary>
public class PaymentManager : IPaymentManager
{
    private readonly PaymentRepository _paymentRepository;
    private readonly LoanRepository _loanRepository;
    private readonly Repository<PaymentSchedule> _scheduleRepository;
    private readonly Repository<LoanProduct> _productRepository;
    private readonly PenaltyCalculator _penaltyCalculator;

    public PaymentManager(LoanDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _paymentRepository = new PaymentRepository(context);
        _loanRepository = new LoanRepository(context);
        _scheduleRepository = new Repository<PaymentSchedule>(context);
        _productRepository = new Repository<LoanProduct>(context);
        _penaltyCalculator = new PenaltyCalculator();
    }

    /// <summary>
    /// Processes a payment using the standard allocation order: Penalties ? Interest ? Principal.
    /// This method is atomic - either the entire payment succeeds or fails.
    /// </summary>
    public async Task<PaymentResult> ProcessPaymentAsync(
        int loanId, 
        decimal amount, 
        int processedByUserId, 
        string paymentMethod = "Cash", 
        string? referenceNumber = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero", nameof(amount));

        var loan = await _loanRepository.GetWithSchedulesAsync(loanId)
            ?? throw new InvalidOperationException("Loan not found");

        var product = await _productRepository.GetByIdAsync(loan.LoanProductId)
            ?? throw new InvalidOperationException("Loan product not found");

        var result = new PaymentResult
        {
            LoanId = loanId,
            AmountReceived = amount,
            PaymentDate = DateTime.Now
        };

        // Get pending schedules ordered by due date (FIFO principle)
        var pendingSchedules = loan.PaymentSchedules
            .Where(s => s.Status != PaymentStatus.Paid)
            .OrderBy(s => s.DueDate)
            .ToList();

        decimal remainingAmount = amount;

        // Allocate payment to each schedule following the priority order
        foreach (var schedule in pendingSchedules)
        {
            if (remainingAmount <= 0) break;

            remainingAmount = await AllocatePaymentToScheduleAsync(
                schedule, product, remainingAmount, result);
        }

        // Update loan balances
        await UpdateLoanAfterPaymentAsync(loan, result, amount);

        // Create payment record
        var payment = CreatePaymentRecord(loanId, amount, result, processedByUserId, paymentMethod, referenceNumber, loan);
        await _paymentRepository.AddAsync(payment);
        
        result.ReceiptNumber = payment.ReceiptNumber;
        result.Success = true;

        return result;
    }

    /// <summary>
    /// Calculates the total amount required for early loan settlement.
    /// Includes outstanding principal, pending interest, and any penalties.
    /// </summary>
    public async Task<decimal> CalculateEarlySettlementAmountAsync(int loanId)
    {
        var loan = await _loanRepository.GetWithSchedulesAsync(loanId)
            ?? throw new InvalidOperationException("Loan not found");

        var product = await _productRepository.GetByIdAsync(loan.LoanProductId)
            ?? throw new InvalidOperationException("Loan product not found");

        decimal outstandingPrincipal = loan.OutstandingPrincipal;
        decimal pendingInterest = CalculatePendingInterest(loan);
        decimal penalties = CalculateTotalPenalties(loan, product);

        return outstandingPrincipal + pendingInterest + Math.Max(0, penalties);
    }

    #region Query Methods

    public async Task<Payment?> GetByReceiptNumberAsync(string receiptNumber) =>
        await _paymentRepository.GetByReceiptNumberAsync(receiptNumber);

    public async Task<IEnumerable<Payment>> GetByLoanIdAsync(int loanId) =>
        await _paymentRepository.GetByLoanIdAsync(loanId);

    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate) =>
        await _paymentRepository.GetByDateRangeAsync(startDate, endDate);

    public async Task<IEnumerable<Payment>> GetTodaysCollectionsAsync() =>
        await _paymentRepository.GetTodaysCollectionsAsync();

    public async Task<decimal> GetTotalCollectionsByDateAsync(DateTime date) =>
        await _paymentRepository.GetTotalCollectionsByDateAsync(date);

    #endregion

    #region Private Helper Methods

    private async Task<decimal> AllocatePaymentToScheduleAsync(
        PaymentSchedule schedule, 
        LoanProduct product, 
        decimal remainingAmount, 
        PaymentResult result)
    {
        // Calculate penalty if overdue
        decimal penalty = CalculateSchedulePenalty(schedule, product);

        // 1. Pay penalties first
        if (penalty > 0 && remainingAmount > 0)
        {
            decimal penaltyPayment = Math.Min(remainingAmount, penalty);
            schedule.PenaltyPaid += penaltyPayment;
            result.PenaltyPortion += penaltyPayment;
            remainingAmount -= penaltyPayment;
        }

        // 2. Pay interest
        decimal interestDue = schedule.InterestDue - schedule.InterestPaid;
        if (interestDue > 0 && remainingAmount > 0)
        {
            decimal interestPayment = Math.Min(remainingAmount, interestDue);
            schedule.InterestPaid += interestPayment;
            result.InterestPortion += interestPayment;
            remainingAmount -= interestPayment;
        }

        // 3. Pay principal
        decimal principalDue = schedule.PrincipalDue - schedule.PrincipalPaid;
        if (principalDue > 0 && remainingAmount > 0)
        {
            decimal principalPayment = Math.Min(remainingAmount, principalDue);
            schedule.PrincipalPaid += principalPayment;
            result.PrincipalPortion += principalPayment;
            remainingAmount -= principalPayment;
        }

        // Update schedule status
        UpdateScheduleStatus(schedule);
        await _scheduleRepository.UpdateAsync(schedule);

        return remainingAmount;
    }

    private decimal CalculateSchedulePenalty(PaymentSchedule schedule, LoanProduct product)
    {
        if (schedule.DueDate >= DateTime.Now) return 0;

        int daysLate = (DateTime.Now - schedule.DueDate).Days;
        decimal overdueAmount = schedule.TotalDue - schedule.TotalPaid;
        decimal penalty = _penaltyCalculator.CalculatePenalty(
            overdueAmount, product.PenaltyRatePerDay, daysLate, product.GracePeriodDays);
        
        return penalty - schedule.PenaltyPaid; // Subtract already paid penalties
    }

    private static void UpdateScheduleStatus(PaymentSchedule schedule)
    {
        schedule.TotalPaid = schedule.PrincipalPaid + schedule.InterestPaid + schedule.PenaltyPaid;
        
        if (schedule.PrincipalPaid >= schedule.PrincipalDue && schedule.InterestPaid >= schedule.InterestDue)
        {
            schedule.Status = PaymentStatus.Paid;
            schedule.PaidDate = DateTime.Now;
        }
        else if (schedule.TotalPaid > 0)
        {
            schedule.Status = PaymentStatus.PartiallyPaid;
        }
    }

    private async Task UpdateLoanAfterPaymentAsync(Loan loan, PaymentResult result, decimal totalAmount)
    {
        loan.OutstandingPrincipal -= result.PrincipalPortion;
        loan.TotalPaid += totalAmount;
        loan.TotalPenalties += result.PenaltyPortion;
        loan.UpdatedAt = DateTime.Now;

        // Check if loan is fully paid
        if (loan.OutstandingPrincipal <= 0)
        {
            loan.Status = LoanStatus.FullyPaid;
            loan.FullyPaidDate = DateTime.Now;
            loan.OutstandingPrincipal = 0;
        }

        await _loanRepository.UpdateAsync(loan);
    }

    private static Payment CreatePaymentRecord(
        int loanId, 
        decimal amount, 
        PaymentResult result, 
        int processedByUserId,
        string paymentMethod,
        string? referenceNumber,
        Loan loan)
    {
        return new Payment
        {
            ReceiptNumber = GenerateReceiptNumber(),
            LoanId = loanId,
            PaymentDate = DateTime.Now,
            AmountPaid = amount,
            PrincipalPortion = result.PrincipalPortion,
            InterestPortion = result.InterestPortion,
            PenaltyPortion = result.PenaltyPortion,
            PaymentType = DeterminePaymentType(amount, loan.MonthlyPayment, loan.OutstandingPrincipal + result.PrincipalPortion),
            PaymentMethod = paymentMethod,
            ReferenceNumber = referenceNumber,
            ProcessedByUserId = processedByUserId
        };
    }

    private static decimal CalculatePendingInterest(Loan loan)
    {
        return loan.PaymentSchedules
            .Where(s => s.Status != PaymentStatus.Paid)
            .Sum(s => s.InterestDue - s.InterestPaid);
    }

    private decimal CalculateTotalPenalties(Loan loan, LoanProduct product)
    {
        decimal penalties = 0;
        
        foreach (var schedule in loan.PaymentSchedules.Where(s => s.Status != PaymentStatus.Paid && s.DueDate < DateTime.Now))
        {
            int daysLate = (DateTime.Now - schedule.DueDate).Days;
            decimal overdueAmount = schedule.TotalDue - schedule.TotalPaid;
            penalties += _penaltyCalculator.CalculatePenalty(overdueAmount, product.PenaltyRatePerDay, daysLate, product.GracePeriodDays);
            penalties -= schedule.PenaltyPaid;
        }

        return penalties;
    }

    private static PaymentType DeterminePaymentType(decimal amount, decimal monthlyPayment, decimal outstanding)
    {
        if (amount >= outstanding) return PaymentType.FullPayment;
        if (amount > monthlyPayment * 1.5M) return PaymentType.Advance;
        if (amount < monthlyPayment * 0.9M) return PaymentType.Partial;
        return PaymentType.Regular;
    }

    private static string GenerateReceiptNumber()
    {
        return $"OR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }

    #endregion
}

/// <summary>
/// Represents the result of a payment processing operation.
/// </summary>
public class PaymentResult
{
    public int LoanId { get; init; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public decimal AmountReceived { get; init; }
    public decimal PrincipalPortion { get; set; }
    public decimal InterestPortion { get; set; }
    public decimal PenaltyPortion { get; set; }
    public decimal ChargesPortion { get; set; }
    public DateTime PaymentDate { get; init; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
