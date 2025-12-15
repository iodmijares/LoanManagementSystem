namespace LoanManagementSystem.Interfaces;

using LoanManagementSystem.Models;


public interface ILoanApplicationManager
{
    Task<LoanApplication> CreateApplicationAsync(LoanApplication application, int processedByUserId);
    Task<LoanApplication?> GetByIdAsync(int id);
    Task<LoanApplication?> GetByApplicationNumberAsync(string applicationNumber);
    Task<LoanApplication?> GetWithDetailsAsync(int id);
    Task<IEnumerable<LoanApplication>> GetAllAsync();
    Task<IEnumerable<LoanApplication>> GetByStatusAsync(ApplicationStatus status);
    Task<IEnumerable<LoanApplication>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<LoanApplication>> GetPendingApprovalsAsync();
    Task ApproveApplicationAsync(int applicationId, int approvedByUserId, decimal? approvedAmount = null, string? conditions = null);
    Task RejectApplicationAsync(int applicationId, int rejectedByUserId, string reason);
    Task<Loan> ReleaseApplicationAsync(int applicationId, int disbursedByUserId);
    Task SetUnderReviewAsync(int applicationId);
    Task CancelApplicationAsync(int applicationId);
}
