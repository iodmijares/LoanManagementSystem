namespace LoanManagementSystem.Models;

public class Payment
{
    public int Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public int LoanId { get; set; }
    public int? PaymentScheduleId { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public decimal AmountPaid { get; set; }

    // Allocation breakdown
    public decimal PrincipalPortion { get; set; }
    public decimal InterestPortion { get; set; }
    public decimal PenaltyPortion { get; set; }
    public decimal ChargesPortion { get; set; }

    public PaymentType PaymentType { get; set; }
    public string PaymentMethod { get; set; } = "Cash";
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }

    // Processing
    public int ProcessedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation
    public virtual Loan? Loan { get; set; }
    public virtual PaymentSchedule? PaymentSchedule { get; set; }
}

public enum PaymentType
{
    Regular,
    Partial,
    Advance,
    FullPayment,
    EarlySettlement
}
