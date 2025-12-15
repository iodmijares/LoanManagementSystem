using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Utilities;

public class DiminishingBalanceCalculator : IInterestCalculator
{
    public InterestCalculationMethod Method => InterestCalculationMethod.DiminishingBalance;

    public decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths)
    {
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
        decimal monthlyPayment = CalculateMonthlyPayment(principal, annualRate, termMonths);
        decimal totalPayable = monthlyPayment * termMonths;
        return Math.Round(totalPayable - principal, 2, MidpointRounding.AwayFromZero);
    }

    public List<AmortizationEntry> GenerateAmortizationSchedule(decimal principal, decimal annualRate, int termMonths, DateTime startDate)
    {
        var schedule = new List<AmortizationEntry>();
        decimal monthlyPayment = CalculateMonthlyPayment(principal, annualRate, termMonths);
        decimal monthlyRate = annualRate / 100 / 12;
        decimal balance = principal;

        for (int i = 1; i <= termMonths; i++)
        {
            decimal interest = Math.Round(balance * monthlyRate, 2, MidpointRounding.AwayFromZero);
            decimal principalPayment = monthlyPayment - interest;

            // Adjust final payment for rounding
            if (i == termMonths)
            {
                principalPayment = balance;
                monthlyPayment = principalPayment + interest;
            }

            schedule.Add(new AmortizationEntry
            {
                Period = i,
                DueDate = startDate.AddMonths(i),
                BeginningBalance = balance,
                Payment = monthlyPayment,
                Principal = principalPayment,
                Interest = interest,
                EndingBalance = Math.Max(0, balance - principalPayment)
            });

            balance -= principalPayment;
        }

        return schedule;
    }
}
