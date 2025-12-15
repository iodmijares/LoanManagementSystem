using LoanManagementSystem.Interfaces;

namespace LoanManagementSystem.Utilities;

public class CreditScoreCalculator : ICreditScoreCalculator
{
    // Weights based on project requirements
    private const decimal PaymentHistoryWeight = 0.35M;
    private const decimal CreditUtilizationWeight = 0.30M;
    private const decimal CreditHistoryWeight = 0.15M;
    private const decimal IncomeStabilityWeight = 0.20M;

    public decimal CalculateCreditScore(CreditScoreInput input)
    {
        // Payment History Score (35%)
        decimal paymentScore = input.PaymentHistoryScore * PaymentHistoryWeight;

        // Credit Utilization (30%) - Lower is better, invert the score
        decimal utilizationScore = (100 - input.CreditUtilization) * CreditUtilizationWeight;

        // Credit History Length (15%)
        decimal historyScore = CalculateHistoryScore(input.CreditHistoryMonths) * CreditHistoryWeight;

        // Income Stability (20%)
        decimal incomeScore = CalculateIncomeStabilityScore(input.YearsEmployed, input.MonthlyIncome) * IncomeStabilityWeight;

        // Deductions for negative factors
        decimal deductions = input.LatePaymentCount * 2M + input.ExistingLoanCount * 1.5M;

        decimal totalScore = paymentScore + utilizationScore + historyScore + incomeScore - deductions;

        return Math.Round(Math.Max(0, Math.Min(100, totalScore)), 2, MidpointRounding.AwayFromZero);
    }

    public string GetCreditRating(decimal score)
    {
        return score switch
        {
            >= 90 => "Excellent",
            >= 80 => "Very Good",
            >= 70 => "Good",
            >= 60 => "Fair",
            >= 50 => "Poor",
            _ => "Very Poor"
        };
    }

    public bool IsEligibleForLoan(decimal score, decimal requestedAmount)
    {
        // Minimum score of 50 for any loan
        if (score < 50) return false;

        // Higher amounts require better scores
        return requestedAmount switch
        {
            > 500000 => score >= 80,
            > 200000 => score >= 70,
            > 100000 => score >= 60,
            _ => score >= 50
        };
    }

    private static decimal CalculateHistoryScore(int months)
    {
        return months switch
        {
            >= 60 => 100, // 5+ years
            >= 36 => 80,  // 3+ years
            >= 24 => 60,  // 2+ years
            >= 12 => 40,  // 1+ year
            _ => 20
        };
    }

    private static decimal CalculateIncomeStabilityScore(int yearsEmployed, decimal monthlyIncome)
    {
        decimal employmentScore = yearsEmployed switch
        {
            >= 5 => 50,
            >= 3 => 40,
            >= 1 => 30,
            _ => 20
        };

        decimal incomeScore = monthlyIncome switch
        {
            >= 100000 => 50,
            >= 50000 => 40,
            >= 30000 => 30,
            >= 20000 => 20,
            _ => 10
        };

        return employmentScore + incomeScore;
    }
}
