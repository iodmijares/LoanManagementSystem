using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repositories;

public class PaymentRepository : Repository<Payment>
{
    public PaymentRepository(LoanDbContext context) : base(context) { }

    public async Task<Payment?> GetByReceiptNumberAsync(string receiptNumber)
    {
        return await _dbSet
            .Include(p => p.Loan)
                .ThenInclude(l => l!.Customer)
            .FirstOrDefaultAsync(p => p.ReceiptNumber == receiptNumber);
    }

    public async Task<IEnumerable<Payment>> GetByLoanIdAsync(int loanId)
    {
        return await _dbSet
            .Where(p => p.LoanId == loanId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(p => p.Loan)
                .ThenInclude(l => l!.Customer)
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetTodaysCollectionsAsync()
    {
        var today = DateTime.Today;
        return await _dbSet
            .Include(p => p.Loan)
                .ThenInclude(l => l!.Customer)
            .Where(p => p.PaymentDate.Date == today)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalCollectionsByDateAsync(DateTime date)
    {
        return await _dbSet
            .Where(p => p.PaymentDate.Date == date.Date)
            .SumAsync(p => p.AmountPaid);
    }

    public async Task<IEnumerable<Payment>> GetByProcessedUserAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.Where(p => p.ProcessedByUserId == userId);

        if (startDate.HasValue)
            query = query.Where(p => p.PaymentDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.PaymentDate <= endDate.Value);

        return await query
            .Include(p => p.Loan)
                .ThenInclude(l => l!.Customer)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }
}
