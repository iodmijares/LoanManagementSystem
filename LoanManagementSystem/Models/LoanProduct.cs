namespace LoanManagementSystem.Models;

public class LoanProduct
{
    public int Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LoanType LoanType { get; set; }

    // Interest Configuration
    public decimal AnnualInterestRate { get; set; }
    public InterestCalculationMethod InterestMethod { get; set; }
    public bool IsVariableRate { get; set; }

    // Fees
    public decimal ServiceChargePercent { get; set; }
    public decimal ProcessingFeeFixed { get; set; }

    // Limits
    public decimal MinimumAmount { get; set; }
    public decimal MaximumAmount { get; set; }
    public string AvailableTermsMonths { get; set; } = "6,12,18,24";

    // Penalty Configuration
    public decimal PenaltyRatePerDay { get; set; }
    public int GracePeriodDays { get; set; } = 3;

    // Requirements
    public bool RequiresCollateral { get; set; }
    public bool RequiresCoMaker { get; set; }
    public string RequiredDocuments { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int[] GetAvailableTerms() => 
        AvailableTermsMonths.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(int.Parse)
                           .ToArray();
}

public enum LoanType
{
    Personal,
    Emergency,
    Salary,
    Business,
    Housing
}

public enum InterestCalculationMethod
{
    DiminishingBalance,
    FlatRate,
    AddOnRate,
    CompoundInterest
}
