using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class LoansControl : UserControl
{
    private readonly LoanDbContext _context;
    private readonly LoanManager _loanManager;
    private readonly LoanApplicationManager _applicationManager;
    private readonly SemaphoreSlim _loadLock = new(1, 1);
    private bool _isLoading = false;

    public LoansControl(LoanDbContext context)
    {
        _context = context;
        _loanManager = new LoanManager(context);
        _applicationManager = new LoanApplicationManager(context);
        InitializeComponent();
        ConfigureAccessByRole();
    }

    private void ConfigureAccessByRole()
    {
        var user = SessionManager.CurrentUser;
        if (user == null) return;

        // New Application button - only for Loan Officers and Admins
        btnNewApplication.Visible = user.HasPermission(Permission.CreateLoanApplication);
        btnNewApplication.Enabled = user.HasPermission(Permission.CreateLoanApplication);
    }

    private void InitializeComponent()
    {
        tabControl = new TabControl();
        tabApplications = new TabPage();
        tabActiveLoans = new TabPage();
        
        pnlAppTop = new Panel();
        btnNewApplication = new Button();
        btnRefreshApps = new Button();
        dgvApplications = new DataGridView();
        
        pnlLoanTop = new Panel();
        btnRefreshLoans = new Button();
        cboLoanStatus = new ComboBox();
        lblStatus = new Label();
        dgvLoans = new DataGridView();

        tabControl.SuspendLayout();
        tabApplications.SuspendLayout();
        tabActiveLoans.SuspendLayout();
        pnlAppTop.SuspendLayout();
        pnlLoanTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvApplications).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLoans).BeginInit();
        SuspendLayout();

        // tabControl
        tabControl.Controls.Add(tabApplications);
        tabControl.Controls.Add(tabActiveLoans);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Font = new Font("Segoe UI", 10F);
        tabControl.Location = new Point(0, 0);
        tabControl.Size = new Size(940, 600);

        // tabApplications
        tabApplications.Controls.Add(dgvApplications);
        tabApplications.Controls.Add(pnlAppTop);
        tabApplications.Location = new Point(4, 34);
        tabApplications.Padding = new Padding(3);
        tabApplications.Size = new Size(932, 562);
        tabApplications.Text = "Loan Applications";

        // pnlAppTop
        pnlAppTop.BackColor = Color.White;
        pnlAppTop.Controls.Add(btnNewApplication);
        pnlAppTop.Controls.Add(btnRefreshApps);
        pnlAppTop.Dock = DockStyle.Top;
        pnlAppTop.Size = new Size(926, 50);

        // btnRefreshApps
        btnRefreshApps.BackColor = Color.FromArgb(52, 152, 219);
        btnRefreshApps.FlatAppearance.BorderSize = 0;
        btnRefreshApps.FlatStyle = FlatStyle.Flat;
        btnRefreshApps.Font = new Font("Segoe UI", 10F);
        btnRefreshApps.ForeColor = Color.White;
        btnRefreshApps.Location = new Point(10, 8);
        btnRefreshApps.Size = new Size(100, 34);
        btnRefreshApps.Text = "Refresh";
        btnRefreshApps.Click += btnRefreshApps_Click;

        // btnNewApplication
        btnNewApplication.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNewApplication.BackColor = Color.FromArgb(46, 204, 113);
        btnNewApplication.FlatAppearance.BorderSize = 0;
        btnNewApplication.FlatStyle = FlatStyle.Flat;
        btnNewApplication.Font = new Font("Segoe UI", 10F);
        btnNewApplication.ForeColor = Color.White;
        btnNewApplication.Location = new Point(780, 8);
        btnNewApplication.Size = new Size(140, 34);
        btnNewApplication.Text = "+ New Application";
        btnNewApplication.Click += btnNewApplication_Click;

        // dgvApplications
        dgvApplications.AllowUserToAddRows = false;
        dgvApplications.AllowUserToDeleteRows = false;
        dgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvApplications.BackgroundColor = Color.White;
        dgvApplications.BorderStyle = BorderStyle.None;
        dgvApplications.Dock = DockStyle.Fill;
        dgvApplications.Location = new Point(3, 53);
        dgvApplications.ReadOnly = true;
        dgvApplications.RowHeadersVisible = false;
        dgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvApplications.Size = new Size(926, 506);
        dgvApplications.CellDoubleClick += dgvApplications_CellDoubleClick;

        // tabActiveLoans
        tabActiveLoans.Controls.Add(dgvLoans);
        tabActiveLoans.Controls.Add(pnlLoanTop);
        tabActiveLoans.Location = new Point(4, 34);
        tabActiveLoans.Padding = new Padding(3);
        tabActiveLoans.Size = new Size(932, 562);
        tabActiveLoans.Text = "Active Loans";

        // pnlLoanTop
        pnlLoanTop.BackColor = Color.White;
        pnlLoanTop.Controls.Add(btnRefreshLoans);
        pnlLoanTop.Controls.Add(cboLoanStatus);
        pnlLoanTop.Controls.Add(lblStatus);
        pnlLoanTop.Dock = DockStyle.Top;
        pnlLoanTop.Size = new Size(926, 50);

        // lblStatus
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(10, 14);
        lblStatus.Text = "Status:";

        // cboLoanStatus
        cboLoanStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoanStatus.Location = new Point(65, 10);
        cboLoanStatus.Size = new Size(150, 28);
        cboLoanStatus.Items.AddRange(new object[] { "All", "Active", "FullyPaid", "Defaulted" });
        cboLoanStatus.SelectedIndex = 1;
        cboLoanStatus.SelectedIndexChanged += cboLoanStatus_SelectedIndexChanged;

        // btnRefreshLoans
        btnRefreshLoans.BackColor = Color.FromArgb(52, 152, 219);
        btnRefreshLoans.FlatAppearance.BorderSize = 0;
        btnRefreshLoans.FlatStyle = FlatStyle.Flat;
        btnRefreshLoans.Font = new Font("Segoe UI", 10F);
        btnRefreshLoans.ForeColor = Color.White;
        btnRefreshLoans.Location = new Point(230, 8);
        btnRefreshLoans.Size = new Size(100, 34);
        btnRefreshLoans.Text = "Refresh";
        btnRefreshLoans.Click += btnRefreshLoans_Click;

        // dgvLoans
        dgvLoans.AllowUserToAddRows = false;
        dgvLoans.AllowUserToDeleteRows = false;
        dgvLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLoans.BackgroundColor = Color.White;
        dgvLoans.BorderStyle = BorderStyle.None;
        dgvLoans.Dock = DockStyle.Fill;
        dgvLoans.Location = new Point(3, 53);
        dgvLoans.ReadOnly = true;
        dgvLoans.RowHeadersVisible = false;
        dgvLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLoans.Size = new Size(926, 506);
        dgvLoans.CellDoubleClick += dgvLoans_CellDoubleClick;

        // LoansControl
        BackColor = Color.FromArgb(240, 240, 245);
        Controls.Add(tabControl);
        Size = new Size(940, 600);
        Load += LoansControl_Load;

        tabControl.ResumeLayout(false);
        tabApplications.ResumeLayout(false);
        tabActiveLoans.ResumeLayout(false);
        pnlAppTop.ResumeLayout(false);
        pnlLoanTop.ResumeLayout(false);
        pnlLoanTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvApplications).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvLoans).EndInit();
        ResumeLayout(false);
    }

    private TabControl tabControl;
    private TabPage tabApplications, tabActiveLoans;
    private Panel pnlAppTop, pnlLoanTop;
    private Button btnNewApplication, btnRefreshApps, btnRefreshLoans;
    private ComboBox cboLoanStatus;
    private Label lblStatus;
    private DataGridView dgvApplications, dgvLoans;

    private async void LoansControl_Load(object sender, EventArgs e)
    {
        await LoadAllDataAsync();
    }

    private async Task LoadAllDataAsync()
    {
        await _loadLock.WaitAsync();
        try
        {
            // Load applications
            var applications = await _applicationManager.GetAllAsync();
            var appData = applications.OrderByDescending(a => a.ApplicationDate).Select(a => new
            {
                a.Id,
                AppNo = a.ApplicationNumber,
                Customer = a.Customer?.FullName ?? "N/A",
                Product = a.LoanProduct?.ProductName ?? "N/A",
                Amount = a.RequestedAmount.ToString("N2"),
                Term = $"{a.RequestedTermMonths} mo",
                Monthly = a.ComputedMonthlyPayment.ToString("N2"),
                a.Status,
                Date = a.ApplicationDate.ToString("MM/dd/yyyy")
            }).ToList();

            dgvApplications.DataSource = appData;
            if (dgvApplications.Columns.Contains("Id"))
                dgvApplications.Columns["Id"].Visible = false;

            // Load loans
            var statusFilter = cboLoanStatus.SelectedItem?.ToString();
            IEnumerable<Loan> loans;

            if (statusFilter == "All")
                loans = await _loanManager.GetAllAsync();
            else if (Enum.TryParse<LoanStatus>(statusFilter, out var status))
                loans = await _loanManager.GetByStatusAsync(status);
            else
                loans = await _loanManager.GetByStatusAsync(LoanStatus.Active);

            var loanData = loans.Select(l => new
            {
                l.Id,
                LoanNo = l.LoanNumber,
                Customer = l.Customer?.FullName ?? "N/A",
                Principal = l.PrincipalAmount.ToString("N2"),
                Monthly = l.MonthlyPayment.ToString("N2"),
                Outstanding = l.OutstandingPrincipal.ToString("N2"),
                TotalPaid = l.TotalPaid.ToString("N2"),
                l.Status,
                Maturity = l.MaturityDate.ToString("MM/dd/yyyy")
            }).ToList();

            dgvLoans.DataSource = loanData;
            if (dgvLoans.Columns.Contains("Id"))
                dgvLoans.Columns["Id"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _loadLock.Release();
        }
    }

    private async Task LoadApplicationsOnlyAsync()
    {
        await _loadLock.WaitAsync();
        try
        {
            var applications = await _applicationManager.GetAllAsync();
            var appData = applications.OrderByDescending(a => a.ApplicationDate).Select(a => new
            {
                a.Id,
                AppNo = a.ApplicationNumber,
                Customer = a.Customer?.FullName ?? "N/A",
                Product = a.LoanProduct?.ProductName ?? "N/A",
                Amount = a.RequestedAmount.ToString("N2"),
                Term = $"{a.RequestedTermMonths} mo",
                Monthly = a.ComputedMonthlyPayment.ToString("N2"),
                a.Status,
                Date = a.ApplicationDate.ToString("MM/dd/yyyy")
            }).ToList();

            dgvApplications.DataSource = appData;
            if (dgvApplications.Columns.Contains("Id"))
                dgvApplications.Columns["Id"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading applications: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _loadLock.Release();
        }
    }

    private async Task LoadLoansOnlyAsync()
    {
        await _loadLock.WaitAsync();
        try
        {
            var statusFilter = cboLoanStatus.SelectedItem?.ToString();
            IEnumerable<Loan> loans;

            if (statusFilter == "All")
                loans = await _loanManager.GetAllAsync();
            else if (Enum.TryParse<LoanStatus>(statusFilter, out var status))
                loans = await _loanManager.GetByStatusAsync(status);
            else
                loans = await _loanManager.GetByStatusAsync(LoanStatus.Active);

            var loanData = loans.Select(l => new
            {
                l.Id,
                LoanNo = l.LoanNumber,
                Customer = l.Customer?.FullName ?? "N/A",
                Principal = l.PrincipalAmount.ToString("N2"),
                Monthly = l.MonthlyPayment.ToString("N2"),
                Outstanding = l.OutstandingPrincipal.ToString("N2"),
                TotalPaid = l.TotalPaid.ToString("N2"),
                l.Status,
                Maturity = l.MaturityDate.ToString("MM/dd/yyyy")
            }).ToList();

            dgvLoans.DataSource = loanData;
            if (dgvLoans.Columns.Contains("Id"))
                dgvLoans.Columns["Id"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading loans: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _loadLock.Release();
        }
    }

    private async void btnRefreshApps_Click(object sender, EventArgs e)
    {
        await LoadApplicationsOnlyAsync();
    }

    private async void btnRefreshLoans_Click(object sender, EventArgs e)
    {
        await LoadLoansOnlyAsync();
    }

    private async void cboLoanStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        await LoadLoansOnlyAsync();
    }

    private async void btnNewApplication_Click(object sender, EventArgs e)
    {
        using var form = new LoanApplicationForm(_context);
        if (form.ShowDialog() == DialogResult.OK)
        {
            await LoadApplicationsOnlyAsync();
        }
    }

    private async void dgvApplications_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            var id = (int)dgvApplications.Rows[e.RowIndex].Cells["Id"].Value;
            using var form = new ApplicationDetailForm(_context, id);
            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadAllDataAsync();
            }
        }
    }

    private void dgvLoans_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            var id = (int)dgvLoans.Rows[e.RowIndex].Cells["Id"].Value;
            using var form = new LoanDetailForm(_context, id);
            form.ShowDialog();
        }
    }
}
