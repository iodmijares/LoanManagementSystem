using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Managers;

public class ReportManager
{
    private readonly LoanDbContext _context;
    private readonly LoanRepository _loanRepository;
    private readonly PaymentRepository _paymentRepository;
    private readonly CustomerRepository _customerRepository;

    public ReportManager(LoanDbContext context)
    {
        _context = context;
        _loanRepository = new LoanRepository(context);
        _paymentRepository = new PaymentRepository(context);
        _customerRepository = new CustomerRepository(context);
    }

    // Dashboard Statistics
    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var stats = new DashboardStats
        {
            TotalActiveLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Active),
            TotalOutstandingBalance = await _context.Loans.Where(l => l.Status == LoanStatus.Active).SumAsync(l => l.OutstandingPrincipal),
            TodaysCollections = await _paymentRepository.GetTotalCollectionsByDateAsync(DateTime.Today),
            PendingApplications = await _context.LoanApplications.CountAsync(a => a.Status == ApplicationStatus.Pending || a.Status == ApplicationStatus.UnderReview),
            OverdueAccounts = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Active && l.PaymentSchedules.Any(s => s.Status == PaymentStatus.Pending && s.DueDate < DateTime.Now)),
            TotalCustomers = await _context.Customers.CountAsync(c => c.IsActive),
            TotalReleasedThisMonth = await _context.Loans.Where(l => l.DisbursementDate.Month == DateTime.Now.Month && l.DisbursementDate.Year == DateTime.Now.Year).SumAsync(l => l.PrincipalAmount),
            BlacklistedCustomers = await _context.Customers.CountAsync(c => c.Classification == CustomerClassification.Blacklisted),
            TotalFullyPaidLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.FullyPaid)
        };

        return stats;
    }

    // Extended Dashboard Statistics
    public async Task<ExtendedDashboardStats> GetExtendedDashboardStatsAsync()
    {
        var now = DateTime.Now;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfWeek = now.AddDays(-(int)now.DayOfWeek);

        var stats = new ExtendedDashboardStats
        {
            // Collections
            WeeklyCollections = await _context.Payments
                .Where(p => p.PaymentDate >= startOfWeek)
                .SumAsync(p => p.AmountPaid),
            MonthlyCollections = await _context.Payments
                .Where(p => p.PaymentDate >= startOfMonth)
                .SumAsync(p => p.AmountPaid),

            // Loan Status Breakdown
            ActiveLoansCount = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Active),
            FullyPaidLoansCount = await _context.Loans.CountAsync(l => l.Status == LoanStatus.FullyPaid),
            DefaultedLoansCount = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Defaulted),

            // Customer Classification
            RegularCustomers = await _context.Customers.CountAsync(c => c.Classification == CustomerClassification.Regular && c.IsActive),
            VipCustomers = await _context.Customers.CountAsync(c => c.Classification == CustomerClassification.VIP && c.IsActive),
            BlacklistedCustomers = await _context.Customers.CountAsync(c => c.Classification == CustomerClassification.Blacklisted),

            // Application Status
            PendingApplications = await _context.LoanApplications.CountAsync(a => a.Status == ApplicationStatus.Pending),
            ApprovedApplications = await _context.LoanApplications.CountAsync(a => a.Status == ApplicationStatus.Approved),
            RejectedApplications = await _context.LoanApplications.CountAsync(a => a.Status == ApplicationStatus.Rejected),
            ReleasedApplications = await _context.LoanApplications.CountAsync(a => a.Status == ApplicationStatus.Released),

            // This Month
            NewCustomersThisMonth = await _context.Customers
                .CountAsync(c => c.CreatedAt.Month == now.Month && c.CreatedAt.Year == now.Year),
            LoansReleasedThisMonth = await _context.Loans
                .CountAsync(l => l.DisbursementDate.Month == now.Month && l.DisbursementDate.Year == now.Year),
            AmountReleasedThisMonth = await _context.Loans
                .Where(l => l.DisbursementDate.Month == now.Month && l.DisbursementDate.Year == now.Year)
                .SumAsync(l => l.PrincipalAmount)
        };

        return stats;
    }

    // Monthly Collections for Chart (last 6 months)
    public async Task<List<MonthlyCollectionData>> GetMonthlyCollectionsAsync(int months = 6)
    {
        var result = new List<MonthlyCollectionData>();
        var now = DateTime.Now;

        for (int i = months - 1; i >= 0; i--)
        {
            var targetMonth = now.AddMonths(-i);
            var startOfMonth = new DateTime(targetMonth.Year, targetMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var total = await _context.Payments
                .Where(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth)
                .SumAsync(p => p.AmountPaid);

            result.Add(new MonthlyCollectionData
            {
                Month = targetMonth.ToString("MMM yyyy"),
                TotalCollections = total
            });
        }

        return result;
    }

    // Loan Product Distribution
    public async Task<List<LoanProductDistribution>> GetLoanProductDistributionAsync()
    {
        return await _context.Loans
            .Where(l => l.Status == LoanStatus.Active)
            .GroupBy(l => l.LoanProduct!.ProductName)
            .Select(g => new LoanProductDistribution
            {
                ProductName = g.Key,
                Count = g.Count(),
                TotalAmount = g.Sum(l => l.PrincipalAmount)
            })
            .ToListAsync();
    }

    // Customer Payment Behavior Analysis
    public async Task<CustomerPaymentBehavior> GetCustomerPaymentBehaviorAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.Loans)
                .ThenInclude(l => l.PaymentSchedules)
            .Include(c => c.Loans)
                .ThenInclude(l => l.Payments)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            return new CustomerPaymentBehavior { CustomerId = customerId };

        var allSchedules = customer.Loans.SelectMany(l => l.PaymentSchedules).ToList();
        var allPayments = customer.Loans.SelectMany(l => l.Payments).ToList();

        var paidSchedules = allSchedules.Where(s => s.Status == PaymentStatus.Paid).ToList();
        var latePayments = paidSchedules.Count(s => s.PaidDate.HasValue && s.PaidDate > s.DueDate);
        var onTimePayments = paidSchedules.Count(s => s.PaidDate.HasValue && s.PaidDate <= s.DueDate);
        var overdueSchedules = allSchedules.Count(s => s.Status == PaymentStatus.Pending && s.DueDate < DateTime.Now);

        return new CustomerPaymentBehavior
        {
            CustomerId = customerId,
            CustomerName = customer.FullName,
            TotalLoans = customer.Loans.Count,
            ActiveLoans = customer.Loans.Count(l => l.Status == LoanStatus.Active),
            FullyPaidLoans = customer.Loans.Count(l => l.Status == LoanStatus.FullyPaid),
            TotalPaymentsMade = allPayments.Count,
            OnTimePayments = onTimePayments,
            LatePayments = latePayments,
            CurrentOverdueCount = overdueSchedules,
            TotalAmountBorrowed = customer.Loans.Sum(l => l.PrincipalAmount),
            TotalAmountPaid = customer.Loans.Sum(l => l.TotalPaid),
            OutstandingBalance = customer.Loans.Where(l => l.Status == LoanStatus.Active).Sum(l => l.OutstandingPrincipal),
            PaymentComplianceRate = paidSchedules.Count > 0 ? (decimal)onTimePayments / paidSchedules.Count * 100 : 100,
            CreditScore = customer.CreditScore,
            Classification = customer.Classification,
            ShouldBeBlacklisted = ShouldCustomerBeBlacklisted(overdueSchedules, latePayments, paidSchedules.Count)
        };
    }

    // Check if customer should be blacklisted based on payment behavior
    private bool ShouldCustomerBeBlacklisted(int overdueCount, int latePayments, int totalPayments)
    {
        // Blacklist criteria:
        // 1. More than 3 currently overdue payments
        // 2. Late payment rate > 50% with at least 5 payments
        if (overdueCount > 3) return true;
        if (totalPayments >= 5 && (decimal)latePayments / totalPayments > 0.5m) return true;
        return false;
    }

    // Get customers eligible for blacklisting
    public async Task<List<CustomerPaymentBehavior>> GetCustomersEligibleForBlacklistAsync()
    {
        var customers = await _context.Customers
            .Include(c => c.Loans)
                .ThenInclude(l => l.PaymentSchedules)
            .Where(c => c.IsActive && c.Classification != CustomerClassification.Blacklisted)
            .ToListAsync();

        var result = new List<CustomerPaymentBehavior>();

        foreach (var customer in customers)
        {
            var behavior = await GetCustomerPaymentBehaviorAsync(customer.Id);
            if (behavior.ShouldBeBlacklisted)
            {
                result.Add(behavior);
            }
        }

        return result.OrderByDescending(c => c.CurrentOverdueCount).ToList();
    }

    // Loan Reports
    public async Task<IEnumerable<Loan>> GetActiveLoansReportAsync()
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Where(l => l.Status == LoanStatus.Active)
            .OrderBy(l => l.Customer!.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetReleasedLoansReportAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Where(l => l.DisbursementDate >= startDate && l.DisbursementDate <= endDate)
            .OrderByDescending(l => l.DisbursementDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetMaturedLoansReportAsync()
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Where(l => l.Status == LoanStatus.Active && l.MaturityDate <= DateTime.Now)
            .OrderBy(l => l.MaturityDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetFullyPaidLoansReportAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Where(l => l.Status == LoanStatus.FullyPaid && l.FullyPaidDate >= startDate && l.FullyPaidDate <= endDate)
            .OrderByDescending(l => l.FullyPaidDate)
            .ToListAsync();
    }

    // Payment Reports
    public async Task<IEnumerable<Payment>> GetDailyCollectionReportAsync(DateTime date)
    {
        return await _context.Payments
            .Include(p => p.Loan)
                .ThenInclude(l => l!.Customer)
            .Where(p => p.PaymentDate.Date == date.Date)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentHistoryByCustomerAsync(int customerId)
    {
        return await _context.Payments
            .Include(p => p.Loan)
            .Where(p => p.Loan!.CustomerId == customerId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<CollectionSummary> GetCollectionSummaryAsync(DateTime startDate, DateTime endDate)
    {
        var payments = await _context.Payments
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .ToListAsync();

        return new CollectionSummary
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalCollections = payments.Sum(p => p.AmountPaid),
            TotalPrincipalCollected = payments.Sum(p => p.PrincipalPortion),
            TotalInterestCollected = payments.Sum(p => p.InterestPortion),
            TotalPenaltiesCollected = payments.Sum(p => p.PenaltyPortion),
            TransactionCount = payments.Count
        };
    }

    // Overdue Reports
    public async Task<IEnumerable<OverdueAccountInfo>> GetOverdueAccountsReportAsync()
    {
        var overdueLoans = await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Include(l => l.PaymentSchedules)
            .Include(l => l.Payments)
            .Where(l => l.Status == LoanStatus.Active && l.PaymentSchedules.Any(s => s.Status == PaymentStatus.Pending && s.DueDate < DateTime.Now))
            .ToListAsync();

        var result = new List<OverdueAccountInfo>();

        foreach (var loan in overdueLoans)
        {
            var overdueSchedules = loan.PaymentSchedules.Where(s => s.Status == PaymentStatus.Pending && s.DueDate < DateTime.Now).ToList();
            var oldestOverdue = overdueSchedules.OrderBy(s => s.DueDate).First();

            result.Add(new OverdueAccountInfo
            {
                LoanId = loan.Id,
                CustomerId = loan.CustomerId,
                LoanNumber = loan.LoanNumber,
                CustomerName = loan.Customer?.FullName ?? "Unknown",
                TotalOverdueAmount = overdueSchedules.Sum(s => s.TotalDue - s.TotalPaid),
                DaysOverdue = (DateTime.Now - oldestOverdue.DueDate).Days,
                OutstandingBalance = loan.OutstandingPrincipal,
                LastPaymentDate = loan.Payments.OrderByDescending(p => p.PaymentDate).FirstOrDefault()?.PaymentDate,
                OverdueInstallments = overdueSchedules.Count
            });
        }

        return result.OrderByDescending(o => o.DaysOverdue);
    }

    // Financial Reports
    public async Task<FinancialSummary> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate)
    {
        var loans = await _context.Loans
            .Where(l => l.DisbursementDate >= startDate && l.DisbursementDate <= endDate)
            .ToListAsync();

        var payments = await _context.Payments
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .ToListAsync();

        return new FinancialSummary
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalDisbursed = loans.Sum(l => l.PrincipalAmount),
            TotalServiceChargeIncome = loans.Sum(l => l.ServiceCharge),
            TotalProcessingFeeIncome = loans.Sum(l => l.ProcessingFee),
            TotalInterestIncome = payments.Sum(p => p.InterestPortion),
            TotalPenaltyIncome = payments.Sum(p => p.PenaltyPortion),
            TotalPrincipalCollected = payments.Sum(p => p.PrincipalPortion),
            OutstandingPrincipalBalance = await _context.Loans.Where(l => l.Status == LoanStatus.Active).SumAsync(l => l.OutstandingPrincipal)
        };
    }

    // Customer Reports
    public async Task<IEnumerable<TopBorrowerInfo>> GetTopBorrowersAsync(int count = 10)
    {
        return await _context.Customers
            .Include(c => c.Loans)
            .Where(c => c.IsActive && c.Loans.Any())
            .Select(c => new TopBorrowerInfo
            {
                CustomerId = c.CustomerId,
                CustomerName = c.FullName,
                TotalLoansCount = c.Loans.Count,
                TotalBorrowed = c.Loans.Sum(l => l.PrincipalAmount),
                TotalPaid = c.Loans.Sum(l => l.TotalPaid),
                ActiveLoansCount = c.Loans.Count(l => l.Status == LoanStatus.Active),
                CreditScore = c.CreditScore
            })
            .OrderByDescending(c => c.TotalBorrowed)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetBlacklistedCustomersAsync()
    {
        return await _context.Customers
            .Include(c => c.Loans)
            .Where(c => c.Classification == CustomerClassification.Blacklisted)
            .ToListAsync();
    }
}

// Report DTOs
public class DashboardStats
{
    public int TotalActiveLoans { get; set; }
    public decimal TotalOutstandingBalance { get; set; }
    public decimal TodaysCollections { get; set; }
    public int PendingApplications { get; set; }
    public int OverdueAccounts { get; set; }
    public int TotalCustomers { get; set; }
    public decimal TotalReleasedThisMonth { get; set; }
    public int BlacklistedCustomers { get; set; }
    public int TotalFullyPaidLoans { get; set; }
}

public class ExtendedDashboardStats
{
    public decimal WeeklyCollections { get; set; }
    public decimal MonthlyCollections { get; set; }
    public int ActiveLoansCount { get; set; }
    public int FullyPaidLoansCount { get; set; }
    public int DefaultedLoansCount { get; set; }
    public int RegularCustomers { get; set; }
    public int VipCustomers { get; set; }
    public int BlacklistedCustomers { get; set; }
    public int PendingApplications { get; set; }
    public int ApprovedApplications { get; set; }
    public int RejectedApplications { get; set; }
    public int ReleasedApplications { get; set; }
    public int NewCustomersThisMonth { get; set; }
    public int LoansReleasedThisMonth { get; set; }
    public decimal AmountReleasedThisMonth { get; set; }
}

public class MonthlyCollectionData
{
    public string Month { get; set; } = string.Empty;
    public decimal TotalCollections { get; set; }
}

public class LoanProductDistribution
{
    public string ProductName { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CustomerPaymentBehavior
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int TotalLoans { get; set; }
    public int ActiveLoans { get; set; }
    public int FullyPaidLoans { get; set; }
    public int TotalPaymentsMade { get; set; }
    public int OnTimePayments { get; set; }
    public int LatePayments { get; set; }
    public int CurrentOverdueCount { get; set; }
    public decimal TotalAmountBorrowed { get; set; }
    public decimal TotalAmountPaid { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal PaymentComplianceRate { get; set; }
    public decimal CreditScore { get; set; }
    public CustomerClassification Classification { get; set; }
    public bool ShouldBeBlacklisted { get; set; }
}

public class CollectionSummary
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalCollections { get; set; }
    public decimal TotalPrincipalCollected { get; set; }
    public decimal TotalInterestCollected { get; set; }
    public decimal TotalPenaltiesCollected { get; set; }
    public int TransactionCount { get; set; }
}

public class OverdueAccountInfo
{
    public int LoanId { get; set; }
    public int CustomerId { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalOverdueAmount { get; set; }
    public int DaysOverdue { get; set; }
    public decimal OutstandingBalance { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public int OverdueInstallments { get; set; }
}

public class FinancialSummary
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalDisbursed { get; set; }
    public decimal TotalServiceChargeIncome { get; set; }
    public decimal TotalProcessingFeeIncome { get; set; }
    public decimal TotalInterestIncome { get; set; }
    public decimal TotalPenaltyIncome { get; set; }
    public decimal TotalPrincipalCollected { get; set; }
    public decimal OutstandingPrincipalBalance { get; set; }
    public decimal TotalIncome => TotalServiceChargeIncome + TotalProcessingFeeIncome + TotalInterestIncome + TotalPenaltyIncome;
}

public class TopBorrowerInfo
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public int TotalLoansCount { get; set; }
    public decimal TotalBorrowed { get; set; }
    public decimal TotalPaid { get; set; }
    public int ActiveLoansCount { get; set; }
    public decimal CreditScore { get; set; }
}
