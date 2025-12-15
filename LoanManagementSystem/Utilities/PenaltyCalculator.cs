namespace LoanManagementSystem.Utilities;

public class PenaltyCalculator
{
    public decimal CalculatePenalty(decimal overdueAmount, decimal penaltyRatePerDay, int daysLate, int gracePeriodDays)
    {
        if (daysLate <= gracePeriodDays) return 0;

        int penaltyDays = daysLate - gracePeriodDays;
        decimal penalty = overdueAmount * (penaltyRatePerDay / 100) * penaltyDays;

        return Math.Round(penalty, 2, MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTotalPenaltiesForLoan(IEnumerable<PenaltyScheduleInfo> overdueSchedules, decimal penaltyRatePerDay, int gracePeriodDays)
    {
        decimal totalPenalty = 0;

        foreach (var schedule in overdueSchedules)
        {
            int daysLate = (DateTime.Now - schedule.DueDate).Days;
            decimal overdueAmount = schedule.TotalDue - schedule.TotalPaid;
            totalPenalty += CalculatePenalty(overdueAmount, penaltyRatePerDay, daysLate, gracePeriodDays);
        }

        return totalPenalty;
    }
}

public class PenaltyScheduleInfo
{
    public DateTime DueDate { get; set; }
    public decimal TotalDue { get; set; }
    public decimal TotalPaid { get; set; }
}
