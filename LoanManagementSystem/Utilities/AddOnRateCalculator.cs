using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Utilities;

public class AddOnRateCalculator : IInterestCalculator
{
    public InterestCalculationMethod Method => InterestCalculationMethod.AddOnRate;

    public decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths)
    {
        decimal monthlyRate = annualRate / 100 / 12;
        decimal totalInterest = principal * monthlyRate * termMonths;
        decimal totalPayable = principal + totalInterest;
        return Math.Round(totalPayable / termMonths, 2, MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTotalInterest(decimal principal, decimal annualRate, int termMonths)
    {
        decimal monthlyRate = annualRate / 100 / 12;
        return Math.Round(principal * monthlyRate * termMonths, 2, MidpointRounding.AwayFromZero);
    }

    public List<AmortizationEntry> GenerateAmortizationSchedule(decimal principal, decimal annualRate, int termMonths, DateTime startDate)
    {
        var schedule = new List<AmortizationEntry>();
        decimal monthlyPayment = CalculateMonthlyPayment(principal, annualRate, termMonths);
        decimal monthlyInterest = Math.Round(CalculateTotalInterest(principal, annualRate, termMonths) / termMonths, 2);
        decimal monthlyPrincipal = Math.Round(principal / termMonths, 2);
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
