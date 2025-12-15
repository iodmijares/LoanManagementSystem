using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repositories;

public class CustomerRepository : Repository<Customer>
{
    public CustomerRepository(LoanDbContext context) : base(context) { }

    public async Task<Customer?> GetByCustomerIdAsync(string customerId)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
    {
        return await _dbSet
            .Where(c => c.FirstName.Contains(searchTerm) ||
                       c.LastName.Contains(searchTerm) ||
                       c.CustomerId.Contains(searchTerm) ||
                       c.Email.Contains(searchTerm) ||
                       c.Phone.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetByClassificationAsync(CustomerClassification classification)
    {
        return await _dbSet.Where(c => c.Classification == classification).ToListAsync();
    }

    public async Task<Customer?> GetWithLoansAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Loans)
                .ThenInclude(l => l.PaymentSchedules)
            .Include(c => c.Loans)
                .ThenInclude(l => l.Payments)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
