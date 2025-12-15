using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class PaymentsControl : UserControl
{
    private readonly LoanDbContext _context;
    private readonly PaymentManager _paymentManager;
    private readonly LoanManager _loanManager;

    public PaymentsControl(LoanDbContext context)
    {
        _context = context;
        _paymentManager = new PaymentManager(context);
        _loanManager = new LoanManager(context);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlPayment = new Panel();
        lblPaymentTitle = new Label();
        lblSearchBy = new Label();
        txtSearch = new TextBox();
        btnSearch = new Button();
        
        lblSelectLoan = new Label();
        dgvLoans = new DataGridView();
        
        grpLoanInfo = new GroupBox();
        lblCustomerName = new Label();
        lblLoanNumber = new Label();
        lblOutstanding = new Label();
        lblMonthlyDue = new Label();
        lblOverdue = new Label();
        
        lblPaymentAmount = new Label();
        txtPaymentAmount = new TextBox();
        lblPaymentMethod = new Label();
        cboPaymentMethod = new ComboBox();
        btnProcessPayment = new Button();
        
        pnlHistory = new Panel();
        lblHistoryTitle = new Label();
        dgvTodaysPayments = new DataGridView();
        lblTodaysTotal = new Label();

        pnlPayment.SuspendLayout();
        grpLoanInfo.SuspendLayout();
        pnlHistory.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLoans).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvTodaysPayments).BeginInit();
        SuspendLayout();

        // pnlPayment
        pnlPayment.BackColor = Color.White;
        pnlPayment.Location = new Point(0, 0);
        pnlPayment.Size = new Size(500, 700);
        pnlPayment.Padding = new Padding(25);

        // lblPaymentTitle
        lblPaymentTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        lblPaymentTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblPaymentTitle.Location = new Point(25, 25);
        lblPaymentTitle.Size = new Size(250, 35);
        lblPaymentTitle.Text = "Process Payment";

        int y = 75;

        // Search by Customer Name
        lblSearchBy.AutoSize = true;
        lblSearchBy.Font = new Font("Segoe UI", 11F);
        lblSearchBy.Location = new Point(25, y);
        lblSearchBy.Text = "Search Customer Name";
        
        txtSearch.Location = new Point(25, y + 28);
        txtSearch.Size = new Size(280, 35);
        txtSearch.Font = new Font("Segoe UI", 11F);
        txtSearch.PlaceholderText = "Enter customer name...";
        
        btnSearch.BackColor = Color.FromArgb(52, 152, 219);
        btnSearch.FlatAppearance.BorderSize = 0;
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Font = new Font("Segoe UI", 10F);
        btnSearch.ForeColor = Color.White;
        btnSearch.Location = new Point(320, y + 26);
        btnSearch.Size = new Size(100, 40);
        btnSearch.Text = "Search";
        btnSearch.Click += btnSearch_Click;
        y += 85;

        // Select Loan Label
        lblSelectLoan.AutoSize = true;
        lblSelectLoan.Font = new Font("Segoe UI", 10F);
        lblSelectLoan.ForeColor = Color.FromArgb(100, 100, 100);
        lblSelectLoan.Location = new Point(25, y);
        lblSelectLoan.Text = "Select a loan from search results:";
        y += 25;

        // Loans DataGridView for search results
        dgvLoans.Location = new Point(25, y);
        dgvLoans.Size = new Size(450, 120);
        dgvLoans.AllowUserToAddRows = false;
        dgvLoans.AllowUserToDeleteRows = false;
        dgvLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLoans.BackgroundColor = Color.White;
        dgvLoans.BorderStyle = BorderStyle.FixedSingle;
        dgvLoans.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvLoans.ColumnHeadersHeight = 35;
        dgvLoans.GridColor = Color.FromArgb(230, 230, 230);
        dgvLoans.ReadOnly = true;
        dgvLoans.RowHeadersVisible = false;
        dgvLoans.RowTemplate.Height = 30;
        dgvLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLoans.CellClick += dgvLoans_CellClick;
        y += 135;

        // Loan Info Group
        grpLoanInfo.Location = new Point(25, y);
        grpLoanInfo.Size = new Size(450, 165);
        grpLoanInfo.Text = "Selected Loan Information";
        grpLoanInfo.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        grpLoanInfo.ForeColor = Color.FromArgb(52, 73, 94);

        int gy = 28;
        lblCustomerName.Location = new Point(15, gy);
        lblCustomerName.Size = new Size(420, 25);
        lblCustomerName.Font = new Font("Segoe UI", 10F);
        lblCustomerName.ForeColor = Color.Black;
        lblCustomerName.Text = "Customer: --";
        gy += 28;

        lblLoanNumber.Location = new Point(15, gy);
        lblLoanNumber.Size = new Size(420, 25);
        lblLoanNumber.Font = new Font("Segoe UI", 10F);
        lblLoanNumber.ForeColor = Color.Gray;
        lblLoanNumber.Text = "Loan #: --";
        gy += 28;

        lblOutstanding.Location = new Point(15, gy);
        lblOutstanding.Size = new Size(420, 25);
        lblOutstanding.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        lblOutstanding.ForeColor = Color.FromArgb(231, 76, 60);
        lblOutstanding.Text = "Outstanding Balance: P 0.00";
        gy += 28;

        lblMonthlyDue.Location = new Point(15, gy);
        lblMonthlyDue.Size = new Size(200, 25);
        lblMonthlyDue.Font = new Font("Segoe UI", 10F);
        lblMonthlyDue.ForeColor = Color.Black;
        lblMonthlyDue.Text = "Monthly Due: P 0.00";

        lblOverdue.Location = new Point(220, gy);
        lblOverdue.Size = new Size(210, 25);
        lblOverdue.Font = new Font("Segoe UI", 10F);
        lblOverdue.ForeColor = Color.Red;
        lblOverdue.Text = "Overdue: P 0.00";

        grpLoanInfo.Controls.AddRange(new Control[] { lblCustomerName, lblLoanNumber, lblOutstanding, lblMonthlyDue, lblOverdue });
        y += 180;

        // Payment Amount
        lblPaymentAmount.AutoSize = true;
        lblPaymentAmount.Font = new Font("Segoe UI", 11F);
        lblPaymentAmount.Location = new Point(25, y);
        lblPaymentAmount.Text = "Payment Amount (P)";
        
        txtPaymentAmount.Location = new Point(25, y + 28);
        txtPaymentAmount.Size = new Size(180, 40);
        txtPaymentAmount.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        txtPaymentAmount.TextAlign = HorizontalAlignment.Right;

        // Payment Method
        lblPaymentMethod.AutoSize = true;
        lblPaymentMethod.Font = new Font("Segoe UI", 11F);
        lblPaymentMethod.Location = new Point(230, y);
        lblPaymentMethod.Text = "Payment Method";
        
        cboPaymentMethod.Location = new Point(230, y + 28);
        cboPaymentMethod.Size = new Size(150, 35);
        cboPaymentMethod.Font = new Font("Segoe UI", 11F);
        cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPaymentMethod.Items.AddRange(new object[] { "Cash", "Check", "Bank Transfer", "GCash", "Maya" });
        cboPaymentMethod.SelectedIndex = 0;
        y += 90;

        // Process Button
        btnProcessPayment.BackColor = Color.FromArgb(46, 204, 113);
        btnProcessPayment.FlatAppearance.BorderSize = 0;
        btnProcessPayment.FlatStyle = FlatStyle.Flat;
        btnProcessPayment.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        btnProcessPayment.ForeColor = Color.White;
        btnProcessPayment.Location = new Point(25, y);
        btnProcessPayment.Size = new Size(200, 50);
        btnProcessPayment.Text = "Process Payment";
        btnProcessPayment.Click += btnProcessPayment_Click;

        pnlPayment.Controls.AddRange(new Control[] {
            lblPaymentTitle, lblSearchBy, txtSearch, btnSearch,
            lblSelectLoan, dgvLoans,
            grpLoanInfo, lblPaymentAmount, txtPaymentAmount,
            lblPaymentMethod, cboPaymentMethod, btnProcessPayment
        });

        // pnlHistory
        pnlHistory.BackColor = Color.White;
        pnlHistory.Location = new Point(520, 0);
        pnlHistory.Size = new Size(530, 700);
        pnlHistory.Padding = new Padding(25);

        // lblHistoryTitle
        lblHistoryTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        lblHistoryTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblHistoryTitle.Location = new Point(25, 25);
        lblHistoryTitle.Size = new Size(250, 35);
        lblHistoryTitle.Text = "Today's Collections";

        // lblTodaysTotal
        lblTodaysTotal.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        lblTodaysTotal.ForeColor = Color.FromArgb(46, 204, 113);
        lblTodaysTotal.Location = new Point(25, 65);
        lblTodaysTotal.Size = new Size(480, 35);
        lblTodaysTotal.Text = "Total: P 0.00";

        // dgvTodaysPayments
        dgvTodaysPayments.Location = new Point(25, 110);
        dgvTodaysPayments.Size = new Size(480, 565);
        dgvTodaysPayments.AllowUserToAddRows = false;
        dgvTodaysPayments.AllowUserToDeleteRows = false;
        dgvTodaysPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvTodaysPayments.BackgroundColor = Color.White;
        dgvTodaysPayments.BorderStyle = BorderStyle.None;
        dgvTodaysPayments.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvTodaysPayments.ColumnHeadersHeight = 40;
        dgvTodaysPayments.GridColor = Color.FromArgb(230, 230, 230);
        dgvTodaysPayments.ReadOnly = true;
        dgvTodaysPayments.RowHeadersVisible = false;
        dgvTodaysPayments.RowTemplate.Height = 35;
        dgvTodaysPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        pnlHistory.Controls.AddRange(new Control[] { lblHistoryTitle, lblTodaysTotal, dgvTodaysPayments });

        // PaymentsControl
        BackColor = Color.FromArgb(240, 240, 245);
        Controls.Add(pnlPayment);
        Controls.Add(pnlHistory);
        Size = new Size(1050, 700);
        Load += PaymentsControl_Load;

        pnlPayment.ResumeLayout(false);
        pnlPayment.PerformLayout();
        grpLoanInfo.ResumeLayout(false);
        pnlHistory.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLoans).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvTodaysPayments).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlPayment, pnlHistory;
    private Label lblPaymentTitle, lblSearchBy, lblSelectLoan, lblPaymentAmount, lblPaymentMethod;
    private Label lblHistoryTitle, lblTodaysTotal;
    private TextBox txtSearch, txtPaymentAmount;
    private Button btnSearch, btnProcessPayment;
    private ComboBox cboPaymentMethod;
    private DataGridView dgvLoans;
    private GroupBox grpLoanInfo;
    private Label lblCustomerName, lblLoanNumber, lblOutstanding, lblMonthlyDue, lblOverdue;
    private DataGridView dgvTodaysPayments;

    private Loan? _currentLoan;
    private bool _isLoading = false;

    private async void PaymentsControl_Load(object sender, EventArgs e)
    {
        await LoadTodaysCollectionsAsync();
        StyleDataGridView(dgvTodaysPayments);
        StyleDataGridView(dgvLoans);
    }

    private void StyleDataGridView(DataGridView dgv)
    {
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
    }

    private async Task LoadTodaysCollectionsAsync()
    {
        if (_isLoading) return;
        _isLoading = true;

        try
        {
            var payments = await _paymentManager.GetTodaysCollectionsAsync();
            var total = await _paymentManager.GetTotalCollectionsByDateAsync(DateTime.Today);

            lblTodaysTotal.Text = $"Total: P {total:N2}";

            var data = payments.Select(p => new
            {
                Receipt = p.ReceiptNumber,
                Loan = p.Loan?.LoanNumber,
                Customer = p.Loan?.Customer?.FullName,
                Amount = p.AmountPaid.ToString("N2"),
                Time = p.PaymentDate.ToString("hh:mm tt")
            }).ToList();

            dgvTodaysPayments.DataSource = data;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading collections: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async void btnSearch_Click(object sender, EventArgs e)
    {
        if (_isLoading) return;
        
        if (string.IsNullOrWhiteSpace(txtSearch.Text))
        {
            MessageBox.Show("Please enter a customer name to search.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _isLoading = true;
        btnSearch.Enabled = false;

        try
        {
            var loans = await _loanManager.SearchByCustomerNameAsync(txtSearch.Text.Trim());
            
            if (!loans.Any())
            {
                MessageBox.Show("No active loans found for this customer.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvLoans.DataSource = null;
                ClearLoanInfo();
                return;
            }

            var data = loans.Select(l => new
            {
                l.Id,
                LoanNo = l.LoanNumber,
                Customer = l.Customer?.FullName,
                Outstanding = l.OutstandingPrincipal.ToString("N2"),
                Monthly = l.MonthlyPayment.ToString("N2")
            }).ToList();

            dgvLoans.DataSource = data;
            
            if (dgvLoans.Columns.Contains("Id"))
                dgvLoans.Columns["Id"].Visible = false;

            if (loans.Count() == 1)
            {
                _isLoading = false;
                await SelectLoanAsync(loans.First().Id);
                return;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error searching: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
            btnSearch.Enabled = true;
        }
    }

    private async void dgvLoans_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (_isLoading) return;
        
        if (e.RowIndex >= 0 && dgvLoans.Rows[e.RowIndex].Cells["Id"].Value != null)
        {
            int loanId = (int)dgvLoans.Rows[e.RowIndex].Cells["Id"].Value;
            await SelectLoanAsync(loanId);
        }
    }

    private async Task SelectLoanAsync(int loanId)
    {
        if (_isLoading) return;
        _isLoading = true;

        try
        {
            _currentLoan = await _loanManager.GetByIdAsync(loanId);

            if (_currentLoan == null)
            {
                ClearLoanInfo();
                return;
            }

            lblCustomerName.Text = $"Customer: {_currentLoan.Customer?.FullName}";
            lblLoanNumber.Text = $"Loan #: {_currentLoan.LoanNumber}";
            lblOutstanding.Text = $"Outstanding Balance: P {_currentLoan.OutstandingPrincipal:N2}";
            lblMonthlyDue.Text = $"Monthly Due: P {_currentLoan.MonthlyPayment:N2}";

            var overdueSchedules = _currentLoan.PaymentSchedules
                .Where(s => s.Status != PaymentStatus.Paid && s.DueDate < DateTime.Now)
                .Sum(s => s.TotalDue - s.TotalPaid);
            lblOverdue.Text = $"Overdue: P {overdueSchedules:N2}";

            txtPaymentAmount.Text = _currentLoan.MonthlyPayment.ToString("N2");
            txtPaymentAmount.Focus();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading loan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void ClearLoanInfo()
    {
        _currentLoan = null;
        lblCustomerName.Text = "Customer: --";
        lblLoanNumber.Text = "Loan #: --";
        lblOutstanding.Text = "Outstanding Balance: P 0.00";
        lblMonthlyDue.Text = "Monthly Due: P 0.00";
        lblOverdue.Text = "Overdue: P 0.00";
        txtPaymentAmount.Clear();
    }

    private async void btnProcessPayment_Click(object sender, EventArgs e)
    {
        if (_isLoading) return;

        if (_currentLoan == null)
        {
            MessageBox.Show("Please search and select a loan first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!decimal.TryParse(txtPaymentAmount.Text.Replace(",", ""), out decimal amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid payment amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"Process payment of P {amount:N2} for:\n\n" +
            $"Customer: {_currentLoan.Customer?.FullName}\n" +
            $"Loan #: {_currentLoan.LoanNumber}",
            "Confirm Payment",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        _isLoading = true;
        btnProcessPayment.Enabled = false;

        try
        {
            var result = await _paymentManager.ProcessPaymentAsync(
                _currentLoan.Id,
                amount,
                SessionManager.GetCurrentUserId(),
                cboPaymentMethod.SelectedItem?.ToString() ?? "Cash");

            if (result.Success)
            {
                var message = $"Payment processed successfully!\n\n" +
                    $"Receipt No: {result.ReceiptNumber}\n" +
                    $"Amount: P {result.AmountReceived:N2}\n" +
                    $"Principal: P {result.PrincipalPortion:N2}\n" +
                    $"Interest: P {result.InterestPortion:N2}\n" +
                    $"Penalty: P {result.PenaltyPortion:N2}";

                MessageBox.Show(message, "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearLoanInfo();
                txtSearch.Clear();
                dgvLoans.DataSource = null;
                
                _isLoading = false;
                await LoadTodaysCollectionsAsync();
            }
            else
            {
                MessageBox.Show($"Payment failed: {result.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
            btnProcessPayment.Enabled = true;
        }
    }
}
