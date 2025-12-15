using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repositories;

public class LoanApplicationRepository : Repository<LoanApplication>
{
    public LoanApplicationRepository(LoanDbContext context) : base(context) { }

    public async Task<LoanApplication?> GetByApplicationNumberAsync(string applicationNumber)
    {
        return await _dbSet
            .Include(a => a.Customer)
            .Include(a => a.LoanProduct)
            .Include(a => a.CoMaker)
            .FirstOrDefaultAsync(a => a.ApplicationNumber == applicationNumber);
    }

    public async Task<IEnumerable<LoanApplication>> GetByStatusAsync(ApplicationStatus status)
    {
        return await _dbSet
            .Include(a => a.Customer)
            .Include(a => a.LoanProduct)
            .Where(a => a.Status == status)
            .OrderByDescending(a => a.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanApplication>> GetByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(a => a.LoanProduct)
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanApplication>> GetPendingApprovalsAsync()
    {
        return await _dbSet
            .Include(a => a.Customer)
            .Include(a => a.LoanProduct)
            .Where(a => a.Status == ApplicationStatus.Pending || a.Status == ApplicationStatus.UnderReview)
            .OrderBy(a => a.ApplicationDate)
            .ToListAsync();
    }

    public async Task<LoanApplication?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(a => a.Customer)
            .Include(a => a.LoanProduct)
            .Include(a => a.CoMaker)
            .Include(a => a.Loan)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
