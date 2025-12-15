namespace LoanManagementSystem.Models;

public class CoMaker
{
    public int Id { get; set; }
    public int CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string IdType { get; set; } = string.Empty;
    public string IdNumber { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public string EmployerName { get; set; } = string.Empty;

    public virtual Customer? Customer { get; set; }
}
