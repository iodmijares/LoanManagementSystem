using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Utilities;

public class CompoundInterestCalculator : IInterestCalculator
{
    public InterestCalculationMethod Method => InterestCalculationMethod.CompoundInterest;

    public decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths)
    {
        // Uses same formula as diminishing balance for monthly payments
        if (principal <= 0 || termMonths <= 0) return 0;
        if (annualRate == 0) return Math.Round(principal / termMonths, 2, MidpointRounding.AwayFromZero);

        decimal monthlyRate = annualRate / 100 / 12;
        double rate = (double)monthlyRate;
        double factor = Math.Pow(1 + rate, termMonths);
        decimal payment = principal * (decimal)(rate * factor / (factor - 1));

        return Math.Round(payment, 2, MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTotalInterest(decimal principal, decimal annualRate, int termMonths)
    {
        // Compound interest formula: A = P(1 + r/n)^(nt)
        decimal monthlyRate = annualRate / 100 / 12;
        decimal compoundedAmount = principal * (decimal)Math.Pow((double)(1 + monthlyRate), termMonths);
        return Math.Round(compoundedAmount - principal, 2, MidpointRounding.AwayFromZero);
    }

    public List<AmortizationEntry> GenerateAmortizationSchedule(decimal principal, decimal annualRate, int termMonths, DateTime startDate)
    {
        // Similar to diminishing balance for amortized payments
        var diminishingCalc = new DiminishingBalanceCalculator();
        return diminishingCalc.GenerateAmortizationSchedule(principal, annualRate, termMonths, startDate);
    }
}
