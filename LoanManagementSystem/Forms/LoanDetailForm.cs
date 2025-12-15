using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Forms;

public partial class LoanDetailForm : Form
{
    private readonly LoanDbContext _context;
    private readonly LoanManager _loanManager;
    private readonly int _loanId;
    private Loan? _loan;

    public LoanDetailForm(LoanDbContext context, int loanId)
    {
        _context = context;
        _loanManager = new LoanManager(context);
        _loanId = loanId;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlInfo = new Panel();
        lblTitle = new Label();
        lblLoanNo = new Label();
        lblCustomer = new Label();
        lblProduct = new Label();
        lblPrincipal = new Label();
        lblMonthly = new Label();
        lblOutstanding = new Label();
        lblTotalPaid = new Label();
        lblStatus = new Label();
        lblMaturity = new Label();

        dgvSchedule = new DataGridView();
        dgvPayments = new DataGridView();
        lblSchedule = new Label();
        lblPayments = new Label();
        btnClose = new Button();

        pnlInfo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSchedule).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPayments).BeginInit();
        SuspendLayout();

        // pnlInfo
        pnlInfo.BackColor = Color.White;
        pnlInfo.Dock = DockStyle.Top;
        pnlInfo.Size = new Size(900, 180);
        pnlInfo.Padding = new Padding(15);

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblTitle.Location = new Point(15, 10);
        lblTitle.Text = "Loan Details";

        int y = 50;
        int col1 = 15, col2 = 250, col3 = 500;

        lblLoanNo.Location = new Point(col1, y);
        lblLoanNo.AutoSize = true;
        lblLoanNo.Font = new Font("Segoe UI", 10F);

        lblCustomer.Location = new Point(col2, y);
        lblCustomer.AutoSize = true;
        lblCustomer.Font = new Font("Segoe UI", 10F);

        lblProduct.Location = new Point(col3, y);
        lblProduct.AutoSize = true;
        lblProduct.Font = new Font("Segoe UI", 10F);

        y += 30;
        lblPrincipal.Location = new Point(col1, y);
        lblPrincipal.AutoSize = true;
        lblPrincipal.Font = new Font("Segoe UI", 10F);

        lblMonthly.Location = new Point(col2, y);
        lblMonthly.AutoSize = true;
        lblMonthly.Font = new Font("Segoe UI", 10F);

        lblStatus.Location = new Point(col3, y);
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);

        y += 30;
        lblOutstanding.Location = new Point(col1, y);
        lblOutstanding.AutoSize = true;
        lblOutstanding.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblOutstanding.ForeColor = Color.FromArgb(231, 76, 60);

        lblTotalPaid.Location = new Point(col2, y);
        lblTotalPaid.AutoSize = true;
        lblTotalPaid.Font = new Font("Segoe UI", 10F);
        lblTotalPaid.ForeColor = Color.FromArgb(46, 204, 113);

        lblMaturity.Location = new Point(col3, y);
        lblMaturity.AutoSize = true;
        lblMaturity.Font = new Font("Segoe UI", 10F);

        pnlInfo.Controls.AddRange(new Control[] {
            lblTitle, lblLoanNo, lblCustomer, lblProduct,
            lblPrincipal, lblMonthly, lblStatus,
            lblOutstanding, lblTotalPaid, lblMaturity
        });

        // lblSchedule
        lblSchedule.AutoSize = true;
        lblSchedule.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        lblSchedule.Location = new Point(15, 190);
        lblSchedule.Text = "Payment Schedule";

        // dgvSchedule
        dgvSchedule.Location = new Point(15, 220);
        dgvSchedule.Size = new Size(430, 250);
        dgvSchedule.AllowUserToAddRows = false;
        dgvSchedule.AllowUserToDeleteRows = false;
        dgvSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvSchedule.BackgroundColor = Color.White;
        dgvSchedule.BorderStyle = BorderStyle.FixedSingle;
        dgvSchedule.ReadOnly = true;
        dgvSchedule.RowHeadersVisible = false;
        dgvSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // lblPayments
        lblPayments.AutoSize = true;
        lblPayments.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        lblPayments.Location = new Point(460, 190);
        lblPayments.Text = "Payment History";

        // dgvPayments
        dgvPayments.Location = new Point(460, 220);
        dgvPayments.Size = new Size(420, 250);
        dgvPayments.AllowUserToAddRows = false;
        dgvPayments.AllowUserToDeleteRows = false;
        dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPayments.BackgroundColor = Color.White;
        dgvPayments.BorderStyle = BorderStyle.FixedSingle;
        dgvPayments.ReadOnly = true;
        dgvPayments.RowHeadersVisible = false;
        dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // btnClose
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnClose.BackColor = Color.FromArgb(149, 165, 166);
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Segoe UI", 10F);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(780, 485);
        btnClose.Size = new Size(100, 38);
        btnClose.Text = "Close";
        btnClose.Click += btnClose_Click;

        // Form
        BackColor = Color.FromArgb(240, 240, 245);
        ClientSize = new Size(900, 540);
        Controls.Add(pnlInfo);
        Controls.Add(lblSchedule);
        Controls.Add(dgvSchedule);
        Controls.Add(lblPayments);
        Controls.Add(dgvPayments);
        Controls.Add(btnClose);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Loan Details";
        Load += LoanDetailForm_Load;

        pnlInfo.ResumeLayout(false);
        pnlInfo.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSchedule).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPayments).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlInfo;
    private Label lblTitle, lblLoanNo, lblCustomer, lblProduct;
    private Label lblPrincipal, lblMonthly, lblOutstanding, lblTotalPaid, lblStatus, lblMaturity;
    private Label lblSchedule, lblPayments;
    private DataGridView dgvSchedule, dgvPayments;
    private Button btnClose;

    private async void LoanDetailForm_Load(object sender, EventArgs e)
    {
        await LoadLoanAsync();
    }

    private async Task LoadLoanAsync()
    {
        try
        {
            _loan = await _loanManager.GetWithSchedulesAsync(_loanId);
            if (_loan == null)
            {
                MessageBox.Show("Loan not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblLoanNo.Text = $"Loan No: {_loan.LoanNumber}";
            lblCustomer.Text = $"Customer: {_loan.Customer?.FullName}";
            lblProduct.Text = $"Product: {_loan.LoanProduct?.ProductName}";
            lblPrincipal.Text = $"Principal: ?{_loan.PrincipalAmount:N2}";
            lblMonthly.Text = $"Monthly: ?{_loan.MonthlyPayment:N2}";
            lblOutstanding.Text = $"Outstanding: ?{_loan.OutstandingPrincipal:N2}";
            lblTotalPaid.Text = $"Total Paid: ?{_loan.TotalPaid:N2}";
            lblStatus.Text = $"Status: {_loan.Status}";
            lblStatus.ForeColor = _loan.Status == LoanStatus.Active ? Color.Green : 
                                  _loan.Status == LoanStatus.FullyPaid ? Color.Blue : Color.Red;
            lblMaturity.Text = $"Maturity: {_loan.MaturityDate:MM/dd/yyyy}";

            // Load schedule
            var scheduleData = _loan.PaymentSchedules.OrderBy(s => s.InstallmentNumber).Select(s => new
            {
                No = s.InstallmentNumber,
                DueDate = s.DueDate.ToString("MM/dd/yy"),
                Due = s.TotalDue.ToString("N2"),
                Paid = s.TotalPaid.ToString("N2"),
                s.Status
            }).ToList();
            dgvSchedule.DataSource = scheduleData;

            // Load payments
            var paymentData = _loan.Payments.OrderByDescending(p => p.PaymentDate).Select(p => new
            {
                Receipt = p.ReceiptNumber,
                Date = p.PaymentDate.ToString("MM/dd/yy"),
                Amount = p.AmountPaid.ToString("N2"),
                p.PaymentType
            }).ToList();
            dgvPayments.DataSource = paymentData;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading loan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();
}
