using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Utilities;

public class FlatRateCalculator : IInterestCalculator
{
    public InterestCalculationMethod Method => InterestCalculationMethod.FlatRate;

    public decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths)
    {
        if (principal <= 0 || termMonths <= 0) return 0;

        decimal totalInterest = CalculateTotalInterest(principal, annualRate, termMonths);
        decimal totalPayable = principal + totalInterest;

        return Math.Round(totalPayable / termMonths, 2, MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTotalInterest(decimal principal, decimal annualRate, int termMonths)
    {
        decimal termYears = termMonths / 12M;
        decimal totalInterest = principal * (annualRate / 100) * termYears;
        return Math.Round(totalInterest, 2, MidpointRounding.AwayFromZero);
    }

    public List<AmortizationEntry> GenerateAmortizationSchedule(decimal principal, decimal annualRate, int termMonths, DateTime startDate)
    {
        var schedule = new List<AmortizationEntry>();
        decimal monthlyPayment = CalculateMonthlyPayment(principal, annualRate, termMonths);
        decimal totalInterest = CalculateTotalInterest(principal, annualRate, termMonths);
        decimal monthlyInterest = Math.Round(totalInterest / termMonths, 2, MidpointRounding.AwayFromZero);
        decimal monthlyPrincipal = Math.Round(principal / termMonths, 2, MidpointRounding.AwayFromZero);
        decimal balance = principal;

        for (int i = 1; i <= termMonths; i++)
        {
            decimal principalPayment = i == termMonths ? balance : monthlyPrincipal;

            schedule.Add(new AmortizationEntry
            {
                Period = i,
                DueDate = startDate.AddMonths(i),
                BeginningBalance = balance,
                Payment = monthlyPayment,
                Principal = principalPayment,
                Interest = monthlyInterest,
                EndingBalance = Math.Max(0, balance - principalPayment)
            });

            balance -= principalPayment;
        }

        return schedule;
    }
}
