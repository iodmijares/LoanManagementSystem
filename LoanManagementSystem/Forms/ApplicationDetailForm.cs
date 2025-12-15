using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class ApplicationDetailForm : Form
{
    private readonly LoanDbContext _context;
    private readonly LoanApplicationManager _applicationManager;
    private readonly int _applicationId;
    private LoanApplication? _application;

    public ApplicationDetailForm(LoanDbContext context, int applicationId)
    {
        _context = context;
        _applicationManager = new LoanApplicationManager(context);
        _applicationId = applicationId;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlMain = new Panel();
        lblTitle = new Label();
        lblAppNo = new Label();
        lblStatus = new Label();
        lblCustomer = new Label();
        lblProduct = new Label();
        lblAmount = new Label();
        lblTerm = new Label();
        lblMonthly = new Label();
        lblTotal = new Label();
        lblNetProceeds = new Label();
        lblCreditScore = new Label();
        lblPurpose = new Label();
        lblDate = new Label();
        
        btnApprove = new Button();
        btnReject = new Button();
        btnRelease = new Button();
        btnClose = new Button();

        pnlMain.SuspendLayout();
        SuspendLayout();

        // pnlMain
        pnlMain.AutoScroll = true;
        pnlMain.BackColor = Color.White;
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Padding = new Padding(20);

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblTitle.Location = new Point(20, 15);
        lblTitle.Text = "Loan Application Details";

        int y = 60;
        int spacing = 30;

        CreateInfoLabel(lblAppNo, "Application No:", y, ref y, spacing);
        CreateInfoLabel(lblStatus, "Status:", y, ref y, spacing);
        CreateInfoLabel(lblCustomer, "Customer:", y, ref y, spacing);
        CreateInfoLabel(lblProduct, "Loan Product:", y, ref y, spacing);
        CreateInfoLabel(lblAmount, "Requested Amount:", y, ref y, spacing);
        CreateInfoLabel(lblTerm, "Term:", y, ref y, spacing);
        CreateInfoLabel(lblMonthly, "Monthly Payment:", y, ref y, spacing);
        CreateInfoLabel(lblTotal, "Total Payable:", y, ref y, spacing);
        CreateInfoLabel(lblNetProceeds, "Net Proceeds:", y, ref y, spacing);
        CreateInfoLabel(lblCreditScore, "Credit Score:", y, ref y, spacing);
        CreateInfoLabel(lblPurpose, "Purpose:", y, ref y, spacing);
        CreateInfoLabel(lblDate, "Application Date:", y, ref y, spacing);
        
        y += 20;

        // Buttons
        btnApprove.BackColor = Color.FromArgb(46, 204, 113);
        btnApprove.FlatAppearance.BorderSize = 0;
        btnApprove.FlatStyle = FlatStyle.Flat;
        btnApprove.Font = new Font("Segoe UI", 10F);
        btnApprove.ForeColor = Color.White;
        btnApprove.Location = new Point(20, y);
        btnApprove.Size = new Size(100, 38);
        btnApprove.Text = "Approve";
        btnApprove.Click += btnApprove_Click;

        btnReject.BackColor = Color.FromArgb(231, 76, 60);
        btnReject.FlatAppearance.BorderSize = 0;
        btnReject.FlatStyle = FlatStyle.Flat;
        btnReject.Font = new Font("Segoe UI", 10F);
        btnReject.ForeColor = Color.White;
        btnReject.Location = new Point(130, y);
        btnReject.Size = new Size(100, 38);
        btnReject.Text = "Reject";
        btnReject.Click += btnReject_Click;

        btnRelease.BackColor = Color.FromArgb(155, 89, 182);
        btnRelease.FlatAppearance.BorderSize = 0;
        btnRelease.FlatStyle = FlatStyle.Flat;
        btnRelease.Font = new Font("Segoe UI", 10F);
        btnRelease.ForeColor = Color.White;
        btnRelease.Location = new Point(240, y);
        btnRelease.Size = new Size(100, 38);
        btnRelease.Text = "Release";
        btnRelease.Click += btnRelease_Click;

        btnClose.BackColor = Color.FromArgb(149, 165, 166);
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Segoe UI", 10F);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(380, y);
        btnClose.Size = new Size(100, 38);
        btnClose.Text = "Close";
        btnClose.Click += btnClose_Click;

        // Add controls
        pnlMain.Controls.AddRange(new Control[] {
            lblTitle, lblAppNo, lblStatus, lblCustomer, lblProduct,
            lblAmount, lblTerm, lblMonthly, lblTotal, lblNetProceeds,
            lblCreditScore, lblPurpose, lblDate,
            btnApprove, btnReject, btnRelease, btnClose
        });

        // Form
        ClientSize = new Size(500, y + 70);
        Controls.Add(pnlMain);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Application Details";
        Load += ApplicationDetailForm_Load;

        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ResumeLayout(false);
    }

    private void CreateInfoLabel(Label label, string prefix, int y, ref int nextY, int spacing)
    {
        label.AutoSize = true;
        label.Font = new Font("Segoe UI", 10F);
        label.Location = new Point(20, y);
        label.Text = $"{prefix}";
        nextY = y + spacing;
    }

    private Panel pnlMain;
    private Label lblTitle, lblAppNo, lblStatus, lblCustomer, lblProduct;
    private Label lblAmount, lblTerm, lblMonthly, lblTotal, lblNetProceeds;
    private Label lblCreditScore, lblPurpose, lblDate;
    private Button btnApprove, btnReject, btnRelease, btnClose;

    private async void ApplicationDetailForm_Load(object sender, EventArgs e)
    {
        await LoadApplicationAsync();
        ConfigureButtonAccess();
    }

    private void ConfigureButtonAccess()
    {
        var user = SessionManager.CurrentUser;
        if (user == null) return;

        // Approve/Reject - Only Loan Officers and Admins
        bool canApproveReject = user.HasPermission(Permission.ApproveLoan) || 
                                 user.HasPermission(Permission.RejectLoan);
        btnApprove.Enabled = canApproveReject;
        btnReject.Enabled = canApproveReject;

        // Release - Only Cashiers and Admins (disbursement)
        bool canRelease = user.HasPermission(Permission.ReleaseLoan);
        btnRelease.Enabled = canRelease;

        // For Loan Officers, check approval limit
        if (user is LoanOfficer officer && _application != null)
        {
            if (!officer.CanApproveLoan(_application.RequestedAmount))
            {
                btnApprove.Enabled = false;
                btnApprove.Text = "Over Limit";
            }
        }
    }

    private async Task LoadApplicationAsync()
    {
        try
        {
            _application = await _applicationManager.GetWithDetailsAsync(_applicationId);
            if (_application == null)
            {
                MessageBox.Show("Application not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblAppNo.Text = $"Application No: {_application.ApplicationNumber}";
            lblStatus.Text = $"Status: {_application.Status}";
            lblStatus.ForeColor = GetStatusColor(_application.Status);
            lblCustomer.Text = $"Customer: {_application.Customer?.FullName}";
            lblProduct.Text = $"Loan Product: {_application.LoanProduct?.ProductName}";
            lblAmount.Text = $"Requested Amount: P {_application.RequestedAmount:N2}";
            lblTerm.Text = $"Term: {_application.RequestedTermMonths} months";
            lblMonthly.Text = $"Monthly Payment: P {_application.ComputedMonthlyPayment:N2}";
            lblTotal.Text = $"Total Payable: P {_application.ComputedTotalPayable:N2}";
            lblNetProceeds.Text = $"Net Proceeds: P {_application.NetProceedsAmount:N2}";
            lblCreditScore.Text = $"Credit Score at Application: {_application.CreditScoreAtApplication:N2}";
            lblPurpose.Text = $"Purpose: {_application.PurposeOfLoan}";
            lblDate.Text = $"Application Date: {_application.ApplicationDate:MM/dd/yyyy hh:mm tt}";

            // Show/hide buttons based on status AND permissions
            bool isPendingOrReview = _application.Status == ApplicationStatus.Pending || 
                                      _application.Status == ApplicationStatus.UnderReview;
            bool isApproved = _application.Status == ApplicationStatus.Approved;

            btnApprove.Visible = isPendingOrReview && SessionManager.HasPermission(Permission.ApproveLoan);
            btnReject.Visible = isPendingOrReview && SessionManager.HasPermission(Permission.RejectLoan);
            btnRelease.Visible = isApproved && SessionManager.HasPermission(Permission.ReleaseLoan);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Color GetStatusColor(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.Pending => Color.Orange,
            ApplicationStatus.UnderReview => Color.Blue,
            ApplicationStatus.Approved => Color.Green,
            ApplicationStatus.Rejected => Color.Red,
            ApplicationStatus.Released => Color.Purple,
            ApplicationStatus.Cancelled => Color.Gray,
            _ => Color.Black
        };
    }

    private async void btnApprove_Click(object sender, EventArgs e)
    {
        // Double-check permission
        if (!SessionManager.HasPermission(Permission.ApproveLoan))
        {
            MessageBox.Show("You don't have permission to approve loans.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Check approval limit for Loan Officers
        if (SessionManager.CurrentUser is LoanOfficer officer && _application != null)
        {
            if (!officer.CanApproveLoan(_application.RequestedAmount))
            {
                MessageBox.Show($"This loan amount (P {_application.RequestedAmount:N2}) exceeds your approval limit (P {officer.ApprovalLimit:N2}).\nPlease escalate to a supervisor.", 
                    "Approval Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        var result = MessageBox.Show("Are you sure you want to approve this application?", "Confirm Approval",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                await _applicationManager.ApproveApplicationAsync(_applicationId, SessionManager.GetCurrentUserId());
                MessageBox.Show("Application approved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                await LoadApplicationAsync();
                ConfigureButtonAccess();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async void btnReject_Click(object sender, EventArgs e)
    {
        // Double-check permission
        if (!SessionManager.HasPermission(Permission.RejectLoan))
        {
            MessageBox.Show("You don't have permission to reject loans.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string reason = Microsoft.VisualBasic.Interaction.InputBox("Please enter the rejection reason:", "Reject Application", "");
        if (string.IsNullOrWhiteSpace(reason)) return;

        try
        {
            await _applicationManager.RejectApplicationAsync(_applicationId, SessionManager.GetCurrentUserId(), reason);
            MessageBox.Show("Application rejected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            await LoadApplicationAsync();
            ConfigureButtonAccess();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error rejecting application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnRelease_Click(object sender, EventArgs e)
    {
        // Double-check permission - Only Cashiers and Admins can release
        if (!SessionManager.HasPermission(Permission.ReleaseLoan))
        {
            MessageBox.Show("You don't have permission to release loans.\nOnly Cashiers can disburse loan proceeds.", 
                "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show("Are you sure you want to release this loan?\nThis will disburse the funds and activate the loan.",
            "Confirm Release", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                var loan = await _applicationManager.ReleaseApplicationAsync(_applicationId, SessionManager.GetCurrentUserId());
                MessageBox.Show($"Loan released successfully!\nLoan No: {loan.LoanNumber}\n\nNet Proceeds: P {_application?.NetProceedsAmount:N2}", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error releasing loan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}
