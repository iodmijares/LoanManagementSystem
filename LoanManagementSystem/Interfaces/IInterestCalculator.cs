using LoanManagementSystem.Models;

namespace LoanManagementSystem.Interfaces;

public interface IInterestCalculator
{
    InterestCalculationMethod Method { get; }
    decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths);
    decimal CalculateTotalInterest(decimal principal, decimal annualRate, int termMonths);
    List<AmortizationEntry> GenerateAmortizationSchedule(decimal principal, decimal annualRate, int termMonths, DateTime startDate);
}

public class AmortizationEntry
{
    public int Period { get; set; }
    public DateTime DueDate { get; set; }
    public decimal BeginningBalance { get; set; }
    public decimal Payment { get; set; }
    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal EndingBalance { get; set; }
}
