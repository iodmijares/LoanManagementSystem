namespace LoanManagementSystem.Interfaces;

public interface ICreditScoreCalculator
{
    decimal CalculateCreditScore(CreditScoreInput input);
    string GetCreditRating(decimal score);
    bool IsEligibleForLoan(decimal score, decimal requestedAmount);
}

public class CreditScoreInput
{
    public decimal PaymentHistoryScore { get; set; }
    public decimal CreditUtilization { get; set; }
    public int CreditHistoryMonths { get; set; }
    public decimal MonthlyIncome { get; set; }
    public int YearsEmployed { get; set; }
    public int ExistingLoanCount { get; set; }
    public int LatePaymentCount { get; set; }
}
