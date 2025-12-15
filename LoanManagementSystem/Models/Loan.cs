namespace LoanManagementSystem.Models;

public class Loan
{
    public int Id { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public int LoanApplicationId { get; set; }
    public int CustomerId { get; set; }
    public int LoanProductId { get; set; }

    // Loan Details
    public decimal PrincipalAmount { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public InterestCalculationMethod InterestMethod { get; set; }
    public int TermMonths { get; set; }

    // Computed Values
    public decimal MonthlyPayment { get; set; }
    public decimal TotalInterest { get; set; }
    public decimal TotalPayable { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal ProcessingFee { get; set; }

    // Balance Tracking
    public decimal OutstandingPrincipal { get; set; }
    public decimal OutstandingInterest { get; set; }
    public decimal TotalPenalties { get; set; }
    public decimal TotalPaid { get; set; }

    // Dates
    public DateTime DisbursementDate { get; set; }
    public DateTime FirstPaymentDueDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public DateTime? FullyPaidDate { get; set; }

    // Status
    public LoanStatus Status { get; set; } = LoanStatus.Active;

    // Disbursement
    public int? DisbursedByUserId { get; set; }
    public string? DisbursementReference { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public virtual Customer? Customer { get; set; }
    public virtual LoanProduct? LoanProduct { get; set; }
    public virtual LoanApplication? LoanApplication { get; set; }
    public virtual List<PaymentSchedule> PaymentSchedules { get; set; } = [];
    public virtual List<Payment> Payments { get; set; } = [];
}

public enum LoanStatus
{
    Active,
    FullyPaid,
    Defaulted,
    Restructured,
    WrittenOff
}
