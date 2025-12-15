using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;

namespace LoanManagementSystem.Managers;

public class LoanProductManager
{
    private readonly Repository<LoanProduct> _productRepository;

    public LoanProductManager(LoanDbContext context)
    {
        _productRepository = new Repository<LoanProduct>(context);
    }

    public async Task<LoanProduct> CreateProductAsync(LoanProduct product)
    {
        product.CreatedAt = DateTime.Now;
        return await _productRepository.AddAsync(product);
    }

    public async Task<LoanProduct?> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<LoanProduct>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<IEnumerable<LoanProduct>> GetActiveProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Where(p => p.IsActive);
    }

    public async Task<IEnumerable<LoanProduct>> GetByLoanTypeAsync(LoanType loanType)
    {
        var products = await _productRepository.GetAllAsync();
        return products.Where(p => p.LoanType == loanType && p.IsActive);
    }

    public async Task UpdateProductAsync(LoanProduct product)
    {
        await _productRepository.UpdateAsync(product);
    }

    public async Task DeactivateProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Product not found");

        product.IsActive = false;
        await _productRepository.UpdateAsync(product);
    }

    public async Task ActivateProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Product not found");

        product.IsActive = true;
        await _productRepository.UpdateAsync(product);
    }
}
