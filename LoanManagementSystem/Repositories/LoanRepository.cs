using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repositories;

public class LoanRepository : Repository<Loan>
{
    public LoanRepository(LoanDbContext context) : base(context) { }

    public async Task<Loan?> GetByLoanNumberAsync(string loanNumber)
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Include(l => l.PaymentSchedules)
            .Include(l => l.Payments)
            .FirstOrDefaultAsync(l => l.LoanNumber == loanNumber);
    }

    public async Task<IEnumerable<Loan>> GetByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(l => l.LoanProduct)
            .Include(l => l.PaymentSchedules)
            .Where(l => l.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Where(l => l.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Include(l => l.PaymentSchedules)
            .Where(l => l.Status == LoanStatus.Active &&
                       l.PaymentSchedules.Any(s => s.Status == PaymentStatus.Pending && s.DueDate < DateTime.Now))
            .ToListAsync();
    }

    public async Task<Loan?> GetWithSchedulesAsync(int id)
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Include(l => l.PaymentSchedules.OrderBy(s => s.InstallmentNumber))
            .Include(l => l.Payments.OrderByDescending(p => p.PaymentDate))
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Loan>> GetMaturedLoansAsync()
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Where(l => l.Status == LoanStatus.Active && l.MaturityDate <= DateTime.Now)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalOutstandingBalanceAsync()
    {
        return await _dbSet
            .Where(l => l.Status == LoanStatus.Active)
            .SumAsync(l => l.OutstandingPrincipal);
    }

    public async Task<IEnumerable<Loan>> SearchByCustomerNameAsync(string customerName)
    {
        var searchTerm = customerName.ToLower();
        return await _dbSet
            .Include(l => l.Customer)
            .Include(l => l.LoanProduct)
            .Include(l => l.PaymentSchedules)
            .Where(l => l.Status == LoanStatus.Active &&
                       (l.Customer!.FirstName.ToLower().Contains(searchTerm) ||
                        l.Customer.LastName.ToLower().Contains(searchTerm) ||
                        (l.Customer.FirstName + " " + l.Customer.LastName).ToLower().Contains(searchTerm)))
            .OrderBy(l => l.Customer!.LastName)
            .ThenBy(l => l.Customer!.FirstName)
            .ToListAsync();
    }
}
