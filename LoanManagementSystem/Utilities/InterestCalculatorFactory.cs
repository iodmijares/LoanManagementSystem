using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Utilities;

public static class InterestCalculatorFactory
{
    public static IInterestCalculator Create(InterestCalculationMethod method)
    {
        return method switch
        {
            InterestCalculationMethod.DiminishingBalance => new DiminishingBalanceCalculator(),
            InterestCalculationMethod.FlatRate => new FlatRateCalculator(),
            InterestCalculationMethod.AddOnRate => new AddOnRateCalculator(),
            InterestCalculationMethod.CompoundInterest => new CompoundInterestCalculator(),
            _ => throw new ArgumentException($"Unknown calculation method: {method}")
        };
    }
}
