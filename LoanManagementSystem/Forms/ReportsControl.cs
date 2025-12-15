using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;

namespace LoanManagementSystem.Forms;

public partial class ReportsControl : UserControl
{
    private readonly LoanDbContext _context;
    private readonly ReportManager _reportManager;

    public ReportsControl(LoanDbContext context)
    {
        _context = context;
        _reportManager = new ReportManager(context);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlFilters = new Panel();
        lblReportType = new Label();
        cboReportType = new ComboBox();
        lblDateFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblDateTo = new Label();
        dtpTo = new DateTimePicker();
        btnGenerate = new Button();
        
        dgvReport = new DataGridView();
        lblSummary = new Label();

        pnlFilters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
        SuspendLayout();

        // pnlFilters
        pnlFilters.BackColor = Color.White;
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Size = new Size(1050, 100);
        pnlFilters.Padding = new Padding(25, 20, 25, 20);

        // lblReportType
        lblReportType.AutoSize = true;
        lblReportType.Font = new Font("Segoe UI", 11F);
        lblReportType.Location = new Point(25, 35);
        lblReportType.Text = "Report Type:";

        // cboReportType
        cboReportType.Location = new Point(130, 30);
        cboReportType.Size = new Size(220, 35);
        cboReportType.Font = new Font("Segoe UI", 11F);
        cboReportType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboReportType.Items.AddRange(new object[] {
            "Active Loans",
            "Released Loans",
            "Fully Paid Loans",
            "Overdue Accounts",
            "Daily Collections",
            "Financial Summary",
            "Top Borrowers",
            "Blacklisted Customers"
        });
        cboReportType.SelectedIndex = 0;

        // lblDateFrom
        lblDateFrom.AutoSize = true;
        lblDateFrom.Font = new Font("Segoe UI", 11F);
        lblDateFrom.Location = new Point(380, 35);
        lblDateFrom.Text = "From:";

        // dtpFrom
        dtpFrom.Location = new Point(435, 30);
        dtpFrom.Size = new Size(150, 35);
        dtpFrom.Font = new Font("Segoe UI", 10F);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Value = DateTime.Today.AddMonths(-1);

        // lblDateTo
        lblDateTo.AutoSize = true;
        lblDateTo.Font = new Font("Segoe UI", 11F);
        lblDateTo.Location = new Point(605, 35);
        lblDateTo.Text = "To:";

        // dtpTo
        dtpTo.Location = new Point(640, 30);
        dtpTo.Size = new Size(150, 35);
        dtpTo.Font = new Font("Segoe UI", 10F);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Value = DateTime.Today;

        // btnGenerate
        btnGenerate.BackColor = Color.FromArgb(52, 152, 219);
        btnGenerate.FlatAppearance.BorderSize = 0;
        btnGenerate.FlatStyle = FlatStyle.Flat;
        btnGenerate.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnGenerate.ForeColor = Color.White;
        btnGenerate.Location = new Point(820, 25);
        btnGenerate.Size = new Size(150, 45);
        btnGenerate.Text = "Generate Report";
        btnGenerate.Click += btnGenerate_Click;

        pnlFilters.Controls.AddRange(new Control[] {
            lblReportType, cboReportType, lblDateFrom, dtpFrom,
            lblDateTo, dtpTo, btnGenerate
        });

        // lblSummary
        lblSummary.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(30, 60, 114);
        lblSummary.Location = new Point(25, 115);
        lblSummary.Size = new Size(1000, 35);
        lblSummary.Text = "";

        // dgvReport
        dgvReport.Location = new Point(25, 160);
        dgvReport.Size = new Size(1000, 520);
        dgvReport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.AllowUserToDeleteRows = false;
        dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReport.BackgroundColor = Color.White;
        dgvReport.BorderStyle = BorderStyle.None;
        dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvReport.ColumnHeadersHeight = 45;
        dgvReport.GridColor = Color.FromArgb(230, 230, 230);
        dgvReport.ReadOnly = true;
        dgvReport.RowHeadersVisible = false;
        dgvReport.RowTemplate.Height = 40;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // ReportsControl
        BackColor = Color.FromArgb(240, 240, 245);
        Controls.Add(pnlFilters);
        Controls.Add(lblSummary);
        Controls.Add(dgvReport);
        Size = new Size(1050, 700);
        Load += ReportsControl_Load;

        pnlFilters.ResumeLayout(false);
        pnlFilters.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlFilters;
    private Label lblReportType, lblDateFrom, lblDateTo, lblSummary;
    private ComboBox cboReportType;
    private DateTimePicker dtpFrom, dtpTo;
    private Button btnGenerate;
    private DataGridView dgvReport;

    private void ReportsControl_Load(object sender, EventArgs e)
    {
        StyleDataGridView(dgvReport);
    }

    private void StyleDataGridView(DataGridView dgv)
    {
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
    }

    private async void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            btnGenerate.Enabled = false;
            btnGenerate.Text = "Loading...";

            string reportType = cboReportType.SelectedItem?.ToString() ?? "";
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);

            switch (reportType)
            {
                case "Active Loans":
                    await LoadActiveLoansReport();
                    break;
                case "Released Loans":
                    await LoadReleasedLoansReport(from, to);
                    break;
                case "Fully Paid Loans":
                    await LoadFullyPaidLoansReport(from, to);
                    break;
                case "Overdue Accounts":
                    await LoadOverdueAccountsReport();
                    break;
                case "Daily Collections":
                    await LoadDailyCollectionsReport(from, to);
                    break;
                case "Financial Summary":
                    await LoadFinancialSummaryReport(from, to);
                    break;
                case "Top Borrowers":
                    await LoadTopBorrowersReport();
                    break;
                case "Blacklisted Customers":
                    await LoadBlacklistedCustomersReport();
                    break;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnGenerate.Enabled = true;
            btnGenerate.Text = "Generate Report";
        }
    }

    private async Task LoadActiveLoansReport()
    {
        var loans = await _reportManager.GetActiveLoansReportAsync();
        var data = loans.Select(l => new
        {
            LoanNo = l.LoanNumber,
            Customer = l.Customer?.FullName,
            Product = l.LoanProduct?.ProductName,
            Principal = l.PrincipalAmount.ToString("N2"),
            Monthly = l.MonthlyPayment.ToString("N2"),
            Outstanding = l.OutstandingPrincipal.ToString("N2"),
            Maturity = l.MaturityDate.ToString("MM/dd/yyyy")
        }).ToList();

        dgvReport.DataSource = data;
        lblSummary.Text = $"Active Loans Report - Total: {loans.Count()} loans | Outstanding: P {loans.Sum(l => l.OutstandingPrincipal):N2}";
    }

    private async Task LoadReleasedLoansReport(DateTime from, DateTime to)
    {
        var loans = await _reportManager.GetReleasedLoansReportAsync(from, to);
        var data = loans.Select(l => new
        {
            LoanNo = l.LoanNumber,
            Customer = l.Customer?.FullName,
            Product = l.LoanProduct?.ProductName,
            Principal = l.PrincipalAmount.ToString("N2"),
            Released = l.DisbursementDate.ToString("MM/dd/yyyy")
        }).ToList();

        dgvReport.DataSource = data;
        lblSummary.Text = $"Released Loans ({from:MM/dd/yyyy} - {to:MM/dd/yyyy}) - Total: {loans.Count()} loans | Amount: P {loans.Sum(l => l.PrincipalAmount):N2}";
    }

    private async Task LoadFullyPaidLoansReport(DateTime from, DateTime to)
    {
        var loans = await _reportManager.GetFullyPaidLoansReportAsync(from, to);
        var data = loans.Select(l => new
        {
            LoanNo = l.LoanNumber,
            Customer = l.Customer?.FullName,
            Principal = l.PrincipalAmount.ToString("N2"),
            TotalPaid = l.TotalPaid.ToString("N2"),
            PaidDate = l.FullyPaidDate?.ToString("MM/dd/yyyy")
        }).ToList();

        dgvReport.DataSource = data;
        lblSummary.Text = $"Fully Paid Loans ({from:MM/dd/yyyy} - {to:MM/dd/yyyy}) - Total: {loans.Count()} loans";
    }

    private async Task LoadOverdueAccountsReport()
    {
        var accounts = await _reportManager.GetOverdueAccountsReportAsync();
        var data = accounts.Select(a => new
        {
            LoanNo = a.LoanNumber,
            Customer = a.CustomerName,
            Overdue = a.TotalOverdueAmount.ToString("N2"),
            Days = a.DaysOverdue,
            Outstanding = a.OutstandingBalance.ToString("N2"),
            LastPayment = a.LastPaymentDate?.ToString("MM/dd/yyyy") ?? "Never"
        }).ToList();

        dgvReport.DataSource = data;
        lblSummary.Text = $"Overdue Accounts - Total: {accounts.Count()} accounts | Overdue Amount: P {accounts.Sum(a => a.TotalOverdueAmount):N2}";
    }

    private async Task LoadDailyCollectionsReport(DateTime from, DateTime to)
    {
        var summary = await _reportManager.GetCollectionSummaryAsync(from, to);
        
        var data = new List<object>
        {
            new { Description = "Total Collections", Amount = summary.TotalCollections.ToString("N2") },
            new { Description = "Principal Collected", Amount = summary.TotalPrincipalCollected.ToString("N2") },
            new { Description = "Interest Collected", Amount = summary.TotalInterestCollected.ToString("N2") },
            new { Description = "Penalties Collected", Amount = summary.TotalPenaltiesCollected.ToString("N2") },
            new { Description = "Transaction Count", Amount = summary.TransactionCount.ToString() }
        };

        dgvReport.DataSource = data;
        lblSummary.Text = $"Collection Summary ({from:MM/dd/yyyy} - {to:MM/dd/yyyy}) - Total: P {summary.TotalCollections:N2}";
    }

    private async Task LoadFinancialSummaryReport(DateTime from, DateTime to)
    {
        var summary = await _reportManager.GetFinancialSummaryAsync(from, to);

        var data = new List<object>
        {
            new { Description = "Total Disbursed", Amount = summary.TotalDisbursed.ToString("N2") },
            new { Description = "Service Charge Income", Amount = summary.TotalServiceChargeIncome.ToString("N2") },
            new { Description = "Processing Fee Income", Amount = summary.TotalProcessingFeeIncome.ToString("N2") },
            new { Description = "Interest Income", Amount = summary.TotalInterestIncome.ToString("N2") },
            new { Description = "Penalty Income", Amount = summary.TotalPenaltyIncome.ToString("N2") },
            new { Description = "Total Income", Amount = summary.TotalIncome.ToString("N2") },
            new { Description = "Outstanding Principal", Amount = summary.OutstandingPrincipalBalance.ToString("N2") }
        };

        dgvReport.DataSource = data;
        lblSummary.Text = $"Financial Summary ({from:MM/dd/yyyy} - {to:MM/dd/yyyy}) - Total Income: P {summary.TotalIncome:N2}";
    }

    private async Task LoadTopBorrowersReport()
    {
        var borrowers = await _reportManager.GetTopBorrowersAsync(20);
        var data = borrowers.Select(b => new
        {
            b.CustomerId,
            Name = b.CustomerName,
            Loans = b.TotalLoansCount,
            TotalBorrowed = b.TotalBorrowed.ToString("N2"),
            TotalPaid = b.TotalPaid.ToString("N2"),
            ActiveLoans = b.ActiveLoansCount,
            CreditScore = b.CreditScore.ToString("N2")
        }).ToList();

        dgvReport.DataSource = data;
        lblSummary.Text = $"Top Borrowers - Showing top {borrowers.Count()} customers by total borrowed amount";
    }

    private async Task LoadBlacklistedCustomersReport()
    {
        var customers = await _reportManager.GetBlacklistedCustomersAsync();
        var data = customers.Select(c => new
        {
            c.CustomerId,
            Name = c.FullName,
            c.Phone,
            c.Email,
            CreditScore = c.CreditScore.ToString("N2")
        }).ToList();

        dgvReport.DataSource = data;
        lblSummary.Text = $"Blacklisted Customers - Total: {customers.Count()}";
    }
}
