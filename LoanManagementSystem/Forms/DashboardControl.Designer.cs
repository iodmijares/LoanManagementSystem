namespace LoanManagementSystem.Forms;

partial class DashboardControl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
        pnlStats = new FlowLayoutPanel();
        pnlActiveLoans = new Panel();
        lblActiveLoansValue = new Label();
        lblActiveLoansTitle = new Label();
        pnlOutstanding = new Panel();
        lblOutstandingValue = new Label();
        lblOutstandingTitle = new Label();
        pnlTodayCollections = new Panel();
        lblTodayCollectionsValue = new Label();
        lblTodayCollectionsTitle = new Label();
        pnlOverdue = new Panel();
        lblOverdueValue = new Label();
        lblOverdueTitle = new Label();
        pnlPendingApprovals = new Panel();
        lblPendingValue = new Label();
        lblPendingTitle = new Label();
        pnlCustomers = new Panel();
        lblCustomersValue = new Label();
        lblCustomersTitle = new Label();
        pnlBlacklisted = new Panel();
        lblBlacklistedValue = new Label();
        lblBlacklistedTitle = new Label();

        // Charts Panel
        pnlCharts = new Panel();
        pnlCollectionsChart = new Panel();
        lblCollectionsChartTitle = new Label();
        pnlLoanStatusChart = new Panel();
        lblLoanStatusTitle = new Label();
        pnlCustomerClassChart = new Panel();
        lblCustomerClassTitle = new Label();

        // Quick Stats Panel
        pnlQuickStats = new Panel();
        lblQuickStatsTitle = new Label();
        lblWeeklyCollections = new Label();
        lblMonthlyCollections = new Label();
        lblNewCustomers = new Label();
        lblLoansThisMonth = new Label();

        pnlRecentSection = new Panel();
        lblRecentApplications = new Label();
        dgvRecentApplications = new DataGridView();
        pnlOverdueSection = new Panel();
        lblOverdueAccounts = new Label();
        dgvOverdueAccounts = new DataGridView();
        btnRefresh = new Button();

        pnlStats.SuspendLayout();
        pnlActiveLoans.SuspendLayout();
        pnlOutstanding.SuspendLayout();
        pnlTodayCollections.SuspendLayout();
        pnlOverdue.SuspendLayout();
        pnlPendingApprovals.SuspendLayout();
        pnlCustomers.SuspendLayout();
        pnlBlacklisted.SuspendLayout();
        pnlCharts.SuspendLayout();
        pnlQuickStats.SuspendLayout();
        pnlRecentSection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecentApplications).BeginInit();
        pnlOverdueSection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvOverdueAccounts).BeginInit();
        SuspendLayout();

        // 
        // pnlStats
        // 
        pnlStats.Controls.Add(pnlActiveLoans);
        pnlStats.Controls.Add(pnlOutstanding);
        pnlStats.Controls.Add(pnlTodayCollections);
        pnlStats.Controls.Add(pnlOverdue);
        pnlStats.Controls.Add(pnlPendingApprovals);
        pnlStats.Controls.Add(pnlCustomers);
        pnlStats.Controls.Add(pnlBlacklisted);
        pnlStats.Dock = DockStyle.Top;
        pnlStats.Location = new Point(0, 0);
        pnlStats.Name = "pnlStats";
        pnlStats.Padding = new Padding(5, 5, 5, 10);
        pnlStats.Size = new Size(1200, 115);
        pnlStats.TabIndex = 0;

        // 
        // pnlActiveLoans
        // 
        pnlActiveLoans.BackColor = Color.FromArgb(52, 152, 219);
        pnlActiveLoans.Controls.Add(lblActiveLoansValue);
        pnlActiveLoans.Controls.Add(lblActiveLoansTitle);
        pnlActiveLoans.Location = new Point(8, 8);
        pnlActiveLoans.Margin = new Padding(3);
        pnlActiveLoans.Name = "pnlActiveLoans";
        pnlActiveLoans.Size = new Size(140, 90);
        pnlActiveLoans.TabIndex = 0;

        lblActiveLoansValue.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
        lblActiveLoansValue.ForeColor = Color.White;
        lblActiveLoansValue.Location = new Point(5, 40);
        lblActiveLoansValue.Size = new Size(130, 45);
        lblActiveLoansValue.Text = "0";
        lblActiveLoansValue.TextAlign = ContentAlignment.MiddleCenter;

        lblActiveLoansTitle.Font = new Font("Segoe UI", 9F);
        lblActiveLoansTitle.ForeColor = Color.White;
        lblActiveLoansTitle.Location = new Point(5, 8);
        lblActiveLoansTitle.Size = new Size(130, 30);
        lblActiveLoansTitle.Text = "Active Loans";
        lblActiveLoansTitle.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // pnlOutstanding
        // 
        pnlOutstanding.BackColor = Color.FromArgb(46, 204, 113);
        pnlOutstanding.Controls.Add(lblOutstandingValue);
        pnlOutstanding.Controls.Add(lblOutstandingTitle);
        pnlOutstanding.Location = new Point(154, 8);
        pnlOutstanding.Margin = new Padding(3);
        pnlOutstanding.Size = new Size(140, 90);

        lblOutstandingValue.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        lblOutstandingValue.ForeColor = Color.White;
        lblOutstandingValue.Location = new Point(5, 40);
        lblOutstandingValue.Size = new Size(130, 45);
        lblOutstandingValue.Text = "P 0.00";
        lblOutstandingValue.TextAlign = ContentAlignment.MiddleCenter;

        lblOutstandingTitle.Font = new Font("Segoe UI", 9F);
        lblOutstandingTitle.ForeColor = Color.White;
        lblOutstandingTitle.Location = new Point(5, 8);
        lblOutstandingTitle.Size = new Size(130, 30);
        lblOutstandingTitle.Text = "Outstanding";
        lblOutstandingTitle.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // pnlTodayCollections
        // 
        pnlTodayCollections.BackColor = Color.FromArgb(155, 89, 182);
        pnlTodayCollections.Controls.Add(lblTodayCollectionsValue);
        pnlTodayCollections.Controls.Add(lblTodayCollectionsTitle);
        pnlTodayCollections.Location = new Point(300, 8);
        pnlTodayCollections.Margin = new Padding(3);
        pnlTodayCollections.Size = new Size(140, 90);

        lblTodayCollectionsValue.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        lblTodayCollectionsValue.ForeColor = Color.White;
        lblTodayCollectionsValue.Location = new Point(5, 40);
        lblTodayCollectionsValue.Size = new Size(130, 45);
        lblTodayCollectionsValue.Text = "P 0.00";
        lblTodayCollectionsValue.TextAlign = ContentAlignment.MiddleCenter;

        lblTodayCollectionsTitle.Font = new Font("Segoe UI", 9F);
        lblTodayCollectionsTitle.ForeColor = Color.White;
        lblTodayCollectionsTitle.Location = new Point(5, 8);
        lblTodayCollectionsTitle.Size = new Size(130, 30);
        lblTodayCollectionsTitle.Text = "Today's Collections";
        lblTodayCollectionsTitle.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // pnlOverdue
        // 
        pnlOverdue.BackColor = Color.FromArgb(231, 76, 60);
        pnlOverdue.Controls.Add(lblOverdueValue);
        pnlOverdue.Controls.Add(lblOverdueTitle);
        pnlOverdue.Location = new Point(446, 8);
        pnlOverdue.Margin = new Padding(3);
        pnlOverdue.Size = new Size(140, 90);

        lblOverdueValue.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
        lblOverdueValue.ForeColor = Color.White;
        lblOverdueValue.Location = new Point(5, 40);
        lblOverdueValue.Size = new Size(130, 45);
        lblOverdueValue.Text = "0";
        lblOverdueValue.TextAlign = ContentAlignment.MiddleCenter;

        lblOverdueTitle.Font = new Font("Segoe UI", 9F);
        lblOverdueTitle.ForeColor = Color.White;
        lblOverdueTitle.Location = new Point(5, 8);
        lblOverdueTitle.Size = new Size(130, 30);
        lblOverdueTitle.Text = "Overdue";
        lblOverdueTitle.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // pnlPendingApprovals
        // 
        pnlPendingApprovals.BackColor = Color.FromArgb(241, 196, 15);
        pnlPendingApprovals.Controls.Add(lblPendingValue);
        pnlPendingApprovals.Controls.Add(lblPendingTitle);
        pnlPendingApprovals.Location = new Point(592, 8);
        pnlPendingApprovals.Margin = new Padding(3);
        pnlPendingApprovals.Size = new Size(140, 90);

        lblPendingValue.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
        lblPendingValue.ForeColor = Color.White;
        lblPendingValue.Location = new Point(5, 40);
        lblPendingValue.Size = new Size(130, 45);
        lblPendingValue.Text = "0";
        lblPendingValue.TextAlign = ContentAlignment.MiddleCenter;

        lblPendingTitle.Font = new Font("Segoe UI", 9F);
        lblPendingTitle.ForeColor = Color.White;
        lblPendingTitle.Location = new Point(5, 8);
        lblPendingTitle.Size = new Size(130, 30);
        lblPendingTitle.Text = "Pending";
        lblPendingTitle.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // pnlCustomers
        // 
        pnlCustomers.BackColor = Color.FromArgb(52, 73, 94);
        pnlCustomers.Controls.Add(lblCustomersValue);
        pnlCustomers.Controls.Add(lblCustomersTitle);
        pnlCustomers.Location = new Point(738, 8);
        pnlCustomers.Margin = new Padding(3);
        pnlCustomers.Size = new Size(140, 90);

        lblCustomersValue.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
        lblCustomersValue.ForeColor = Color.White;
        lblCustomersValue.Location = new Point(5, 40);
        lblCustomersValue.Size = new Size(130, 45);
        lblCustomersValue.Text = "0";
        lblCustomersValue.TextAlign = ContentAlignment.MiddleCenter;

        lblCustomersTitle.Font = new Font("Segoe UI", 9F);
        lblCustomersTitle.ForeColor = Color.White;
        lblCustomersTitle.Location = new Point(5, 8);
        lblCustomersTitle.Size = new Size(130, 30);
        lblCustomersTitle.Text = "Customers";
        lblCustomersTitle.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // pnlBlacklisted
        // 
        pnlBlacklisted.BackColor = Color.FromArgb(44, 62, 80);
        pnlBlacklisted.Controls.Add(lblBlacklistedValue);
        pnlBlacklisted.Controls.Add(lblBlacklistedTitle);
        pnlBlacklisted.Location = new Point(884, 8);
        pnlBlacklisted.Margin = new Padding(3);
        pnlBlacklisted.Size = new Size(140, 90);

        lblBlacklistedValue.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
        lblBlacklistedValue.ForeColor = Color.FromArgb(231, 76, 60);
        lblBlacklistedValue.Location = new Point(5, 40);
        lblBlacklistedValue.Size = new Size(130, 45);
        lblBlacklistedValue.Text = "0";
        lblBlacklistedValue.TextAlign = ContentAlignment.MiddleCenter;

        lblBlacklistedTitle.Font = new Font("Segoe UI", 9F);
        lblBlacklistedTitle.ForeColor = Color.White;
        lblBlacklistedTitle.Location = new Point(5, 8);
        lblBlacklistedTitle.Size = new Size(130, 30);
        lblBlacklistedTitle.Text = "Blacklisted";
        lblBlacklistedTitle.TextAlign = ContentAlignment.MiddleCenter;

        // 
        // pnlCharts - Container for visual charts
        // 
        pnlCharts.BackColor = Color.White;
        pnlCharts.Controls.Add(pnlCollectionsChart);
        pnlCharts.Controls.Add(pnlLoanStatusChart);
        pnlCharts.Controls.Add(pnlCustomerClassChart);
        pnlCharts.Controls.Add(pnlQuickStats);
        pnlCharts.Dock = DockStyle.Top;
        pnlCharts.Location = new Point(0, 115);
        pnlCharts.Name = "pnlCharts";
        pnlCharts.Padding = new Padding(10);
        pnlCharts.Size = new Size(1200, 180);
        pnlCharts.TabIndex = 3;

        // 
        // pnlCollectionsChart - Monthly Collections Bar Chart
        // 
        pnlCollectionsChart.BackColor = Color.FromArgb(248, 249, 250);
        pnlCollectionsChart.BorderStyle = BorderStyle.FixedSingle;
        pnlCollectionsChart.Controls.Add(lblCollectionsChartTitle);
        pnlCollectionsChart.Location = new Point(15, 15);
        pnlCollectionsChart.Size = new Size(380, 150);

        lblCollectionsChartTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblCollectionsChartTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblCollectionsChartTitle.Location = new Point(10, 8);
        lblCollectionsChartTitle.Size = new Size(200, 25);
        lblCollectionsChartTitle.Text = "Monthly Collections";

        // 
        // pnlLoanStatusChart - Loan Status Pie Chart
        // 
        pnlLoanStatusChart.BackColor = Color.FromArgb(248, 249, 250);
        pnlLoanStatusChart.BorderStyle = BorderStyle.FixedSingle;
        pnlLoanStatusChart.Controls.Add(lblLoanStatusTitle);
        pnlLoanStatusChart.Location = new Point(405, 15);
        pnlLoanStatusChart.Size = new Size(250, 150);

        lblLoanStatusTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblLoanStatusTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblLoanStatusTitle.Location = new Point(10, 8);
        lblLoanStatusTitle.Size = new Size(150, 25);
        lblLoanStatusTitle.Text = "Loan Status";

        // 
        // pnlCustomerClassChart - Customer Classification
        // 
        pnlCustomerClassChart.BackColor = Color.FromArgb(248, 249, 250);
        pnlCustomerClassChart.BorderStyle = BorderStyle.FixedSingle;
        pnlCustomerClassChart.Controls.Add(lblCustomerClassTitle);
        pnlCustomerClassChart.Location = new Point(665, 15);
        pnlCustomerClassChart.Size = new Size(250, 150);

        lblCustomerClassTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblCustomerClassTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblCustomerClassTitle.Location = new Point(10, 8);
        lblCustomerClassTitle.Size = new Size(230, 25);
        lblCustomerClassTitle.Text = "Customer Classification";

        // 
        // pnlQuickStats - Quick Statistics Summary
        // 
        pnlQuickStats.BackColor = Color.FromArgb(248, 249, 250);
        pnlQuickStats.BorderStyle = BorderStyle.FixedSingle;
        pnlQuickStats.Controls.Add(lblQuickStatsTitle);
        pnlQuickStats.Controls.Add(lblWeeklyCollections);
        pnlQuickStats.Controls.Add(lblMonthlyCollections);
        pnlQuickStats.Controls.Add(lblNewCustomers);
        pnlQuickStats.Controls.Add(lblLoansThisMonth);
        pnlQuickStats.Location = new Point(925, 15);
        pnlQuickStats.Size = new Size(250, 150);

        lblQuickStatsTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblQuickStatsTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblQuickStatsTitle.Location = new Point(10, 8);
        lblQuickStatsTitle.Size = new Size(150, 25);
        lblQuickStatsTitle.Text = "Quick Stats";

        lblWeeklyCollections.Font = new Font("Segoe UI", 9F);
        lblWeeklyCollections.Location = new Point(10, 40);
        lblWeeklyCollections.Size = new Size(230, 22);
        lblWeeklyCollections.Text = "Weekly: P 0.00";

        lblMonthlyCollections.Font = new Font("Segoe UI", 9F);
        lblMonthlyCollections.Location = new Point(10, 65);
        lblMonthlyCollections.Size = new Size(230, 22);
        lblMonthlyCollections.Text = "Monthly: P 0.00";

        lblNewCustomers.Font = new Font("Segoe UI", 9F);
        lblNewCustomers.Location = new Point(10, 90);
        lblNewCustomers.Size = new Size(230, 22);
        lblNewCustomers.Text = "New Customers: 0";

        lblLoansThisMonth.Font = new Font("Segoe UI", 9F);
        lblLoansThisMonth.Location = new Point(10, 115);
        lblLoansThisMonth.Size = new Size(230, 22);
        lblLoansThisMonth.Text = "Loans This Month: 0";

        // 
        // pnlRecentSection
        // 
        pnlRecentSection.BackColor = Color.White;
        pnlRecentSection.Controls.Add(dgvRecentApplications);
        pnlRecentSection.Controls.Add(lblRecentApplications);
        pnlRecentSection.Location = new Point(0, 295);
        pnlRecentSection.Name = "pnlRecentSection";
        pnlRecentSection.Padding = new Padding(15);
        pnlRecentSection.Size = new Size(590, 370);
        pnlRecentSection.TabIndex = 1;

        lblRecentApplications.AutoSize = true;
        lblRecentApplications.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        lblRecentApplications.ForeColor = Color.FromArgb(30, 60, 114);
        lblRecentApplications.Location = new Point(15, 15);
        lblRecentApplications.Text = "Recent Applications";

        dgvRecentApplications.AllowUserToAddRows = false;
        dgvRecentApplications.AllowUserToDeleteRows = false;
        dgvRecentApplications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvRecentApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvRecentApplications.BackgroundColor = Color.White;
        dgvRecentApplications.BorderStyle = BorderStyle.None;
        dgvRecentApplications.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvRecentApplications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvRecentApplications.GridColor = Color.FromArgb(230, 230, 230);
        dgvRecentApplications.Location = new Point(15, 45);
        dgvRecentApplications.ReadOnly = true;
        dgvRecentApplications.RowHeadersVisible = false;
        dgvRecentApplications.RowTemplate.Height = 30;
        dgvRecentApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRecentApplications.Size = new Size(560, 310);

        // 
        // pnlOverdueSection
        // 
        pnlOverdueSection.BackColor = Color.White;
        pnlOverdueSection.Controls.Add(dgvOverdueAccounts);
        pnlOverdueSection.Controls.Add(lblOverdueAccounts);
        pnlOverdueSection.Controls.Add(btnRefresh);
        pnlOverdueSection.Location = new Point(595, 295);
        pnlOverdueSection.Name = "pnlOverdueSection";
        pnlOverdueSection.Padding = new Padding(15);
        pnlOverdueSection.Size = new Size(590, 370);
        pnlOverdueSection.TabIndex = 2;

        lblOverdueAccounts.AutoSize = true;
        lblOverdueAccounts.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        lblOverdueAccounts.ForeColor = Color.FromArgb(231, 76, 60);
        lblOverdueAccounts.Location = new Point(15, 15);
        lblOverdueAccounts.Text = "Overdue Accounts";

        btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 9F);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(480, 10);
        btnRefresh.Size = new Size(90, 30);
        btnRefresh.Text = "Refresh";
        btnRefresh.Click += btnRefresh_Click;

        dgvOverdueAccounts.AllowUserToAddRows = false;
        dgvOverdueAccounts.AllowUserToDeleteRows = false;
        dgvOverdueAccounts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvOverdueAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvOverdueAccounts.BackgroundColor = Color.White;
        dgvOverdueAccounts.BorderStyle = BorderStyle.None;
        dgvOverdueAccounts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvOverdueAccounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvOverdueAccounts.GridColor = Color.FromArgb(230, 230, 230);
        dgvOverdueAccounts.Location = new Point(15, 45);
        dgvOverdueAccounts.ReadOnly = true;
        dgvOverdueAccounts.RowHeadersVisible = false;
        dgvOverdueAccounts.RowTemplate.Height = 30;
        dgvOverdueAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvOverdueAccounts.Size = new Size(560, 310);

        // 
        // DashboardControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(240, 240, 245);
        Controls.Add(pnlOverdueSection);
        Controls.Add(pnlRecentSection);
        Controls.Add(pnlCharts);
        Controls.Add(pnlStats);
        Name = "DashboardControl";
        Size = new Size(1200, 680);
        Load += DashboardControl_Load;
        Resize += DashboardControl_Resize;

        pnlStats.ResumeLayout(false);
        pnlActiveLoans.ResumeLayout(false);
        pnlOutstanding.ResumeLayout(false);
        pnlTodayCollections.ResumeLayout(false);
        pnlOverdue.ResumeLayout(false);
        pnlPendingApprovals.ResumeLayout(false);
        pnlCustomers.ResumeLayout(false);
        pnlBlacklisted.ResumeLayout(false);
        pnlCharts.ResumeLayout(false);
        pnlQuickStats.ResumeLayout(false);
        pnlRecentSection.ResumeLayout(false);
        pnlRecentSection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecentApplications).EndInit();
        pnlOverdueSection.ResumeLayout(false);
        pnlOverdueSection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvOverdueAccounts).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private FlowLayoutPanel pnlStats;
    private Panel pnlActiveLoans;
    private Label lblActiveLoansValue;
    private Label lblActiveLoansTitle;
    private Panel pnlOutstanding;
    private Label lblOutstandingValue;
    private Label lblOutstandingTitle;
    private Panel pnlTodayCollections;
    private Label lblTodayCollectionsValue;
    private Label lblTodayCollectionsTitle;
    private Panel pnlOverdue;
    private Label lblOverdueValue;
    private Label lblOverdueTitle;
    private Panel pnlPendingApprovals;
    private Label lblPendingValue;
    private Label lblPendingTitle;
    private Panel pnlCustomers;
    private Label lblCustomersValue;
    private Label lblCustomersTitle;
    private Panel pnlBlacklisted;
    private Label lblBlacklistedValue;
    private Label lblBlacklistedTitle;

    // Charts
    private Panel pnlCharts;
    private Panel pnlCollectionsChart;
    private Label lblCollectionsChartTitle;
    private Panel pnlLoanStatusChart;
    private Label lblLoanStatusTitle;
    private Panel pnlCustomerClassChart;
    private Label lblCustomerClassTitle;
    private Panel pnlQuickStats;
    private Label lblQuickStatsTitle;
    private Label lblWeeklyCollections;
    private Label lblMonthlyCollections;
    private Label lblNewCustomers;
    private Label lblLoansThisMonth;

    private Panel pnlRecentSection;
    private Label lblRecentApplications;
    private DataGridView dgvRecentApplications;
    private Panel pnlOverdueSection;
    private Label lblOverdueAccounts;
    private DataGridView dgvOverdueAccounts;
    private Button btnRefresh;
}
