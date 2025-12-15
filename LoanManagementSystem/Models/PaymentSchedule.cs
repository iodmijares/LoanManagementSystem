namespace LoanManagementSystem.Models;

public class PaymentSchedule
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public int InstallmentNumber { get; set; }

    public DateTime DueDate { get; set; }
    public decimal PrincipalDue { get; set; }
    public decimal InterestDue { get; set; }
    public decimal TotalDue { get; set; }

    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal PenaltyPaid { get; set; }
    public decimal TotalPaid { get; set; }

    public decimal PrincipalBalance { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime? PaidDate { get; set; }

    public int DaysOverdue => Status == PaymentStatus.Pending && DateTime.Now > DueDate
        ? (DateTime.Now - DueDate).Days : 0;

    public virtual Loan? Loan { get; set; }
}

public enum PaymentStatus
{
    Pending,
    PartiallyPaid,
    Paid,
    Overdue
}
