namespace LoanManagementSystem.Models;

public class LoanApplication
{
    public int Id { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public int LoanProductId { get; set; }
    public int? CoMakerId { get; set; }
    public int ProcessedByUserId { get; set; }

    public decimal RequestedAmount { get; set; }
    public int RequestedTermMonths { get; set; }
    public string PurposeOfLoan { get; set; } = string.Empty;
    public DateTime DesiredReleaseDate { get; set; }

    // Collateral (if applicable)
    public string? CollateralType { get; set; }
    public string? CollateralDescription { get; set; }
    public decimal? CollateralValue { get; set; }

    // Computed Values (at application time)
    public decimal ComputedMonthlyPayment { get; set; }
    public decimal ComputedTotalInterest { get; set; }
    public decimal ComputedTotalPayable { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal ProcessingFee { get; set; }
    public decimal NetProceedsAmount { get; set; }

    // Status
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public string? RejectionReason { get; set; }
    public string? ApprovalConditions { get; set; }

    // Credit Assessment
    public decimal CreditScoreAtApplication { get; set; }
    public string? CreditAssessmentNotes { get; set; }

    // Approval Workflow
    public int? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public decimal? ApprovedAmount { get; set; }

    // Audit
    public DateTime ApplicationDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public virtual Customer? Customer { get; set; }
    public virtual LoanProduct? LoanProduct { get; set; }
    public virtual CoMaker? CoMaker { get; set; }
    public virtual Loan? Loan { get; set; }
}

public enum ApplicationStatus
{
    Pending,
    UnderReview,
    Approved,
    Rejected,
    Cancelled,
    Released
}
