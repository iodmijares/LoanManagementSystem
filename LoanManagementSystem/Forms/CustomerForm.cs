using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class CustomerForm : Form
{
    private readonly LoanDbContext _context;
    private readonly CustomerManager _customerManager;
    private readonly ReportManager _reportManager;
    private readonly int? _customerId;
    private Customer? _customer;
    private CustomerPaymentBehavior? _paymentBehavior;

    public CustomerForm(LoanDbContext context, int? customerId = null)
    {
        _context = context;
        _customerManager = new CustomerManager(context);
        _reportManager = new ReportManager(context);
        _customerId = customerId;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlMain = new Panel();
        lblTitle = new Label();
        
        // Personal Info Section
        lblPersonalInfo = new Label();
        lblFirstName = new Label();
        txtFirstName = new TextBox();
        lblMiddleName = new Label();
        txtMiddleName = new TextBox();
        lblLastName = new Label();
        txtLastName = new TextBox();
        lblDateOfBirth = new Label();
        dtpDateOfBirth = new DateTimePicker();
        lblGender = new Label();
        cboGender = new ComboBox();
        lblCivilStatus = new Label();
        cboCivilStatus = new ComboBox();

        // Contact Info Section
        lblContactInfo = new Label();
        lblAddress = new Label();
        txtAddress = new TextBox();
        lblCity = new Label();
        txtCity = new TextBox();
        lblProvince = new Label();
        txtProvince = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();

        // Employment Info Section
        lblEmploymentInfo = new Label();
        lblEmployer = new Label();
        txtEmployer = new TextBox();
        lblJobTitle = new Label();
        txtJobTitle = new TextBox();
        lblMonthlyIncome = new Label();
        txtMonthlyIncome = new TextBox();
        lblYearsEmployed = new Label();
        nudYearsEmployed = new NumericUpDown();

        // ID Info
        lblIdType = new Label();
        cboIdType = new ComboBox();
        lblIdNumber = new Label();
        txtIdNumber = new TextBox();

        // Payment Behavior Section (for existing customers)
        grpPaymentBehavior = new GroupBox();
        lblTotalLoans = new Label();
        lblOnTimePayments = new Label();
        lblLatePayments = new Label();
        lblOverduePayments = new Label();
        lblComplianceRate = new Label();
        lblCreditScore = new Label();
        lblClassification = new Label();
        pnlClassificationIndicator = new Panel();

        // Buttons
        btnSave = new Button();
        btnCancel = new Button();
        btnBlacklist = new Button();
        btnUnblacklist = new Button();

        pnlMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudYearsEmployed).BeginInit();
        grpPaymentBehavior.SuspendLayout();
        SuspendLayout();

        // pnlMain
        pnlMain.AutoScroll = true;
        pnlMain.BackColor = Color.White;
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Padding = new Padding(30);

        // lblTitle
        lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblTitle.Location = new Point(30, 25);
        lblTitle.Size = new Size(400, 35);
        lblTitle.Text = _customerId.HasValue ? "Edit Customer" : "New Customer Registration";

        int y = 80;
        int inputWidth = 220;

        // Personal Info Section Header
        lblPersonalInfo = CreateSectionHeader("Personal Information", 30, y);
        y += 40;

        // First Name
        lblFirstName.AutoSize = true;
        lblFirstName.Font = new Font("Segoe UI", 10F);
        lblFirstName.Location = new Point(30, y);
        lblFirstName.Text = "First Name *";
        txtFirstName.Location = new Point(30, y + 25);
        txtFirstName.Size = new Size(inputWidth, 32);
        txtFirstName.Font = new Font("Segoe UI", 10F);

        // Middle Name
        lblMiddleName.AutoSize = true;
        lblMiddleName.Font = new Font("Segoe UI", 10F);
        lblMiddleName.Location = new Point(270, y);
        lblMiddleName.Text = "Middle Name";
        txtMiddleName.Location = new Point(270, y + 25);
        txtMiddleName.Size = new Size(180, 32);
        txtMiddleName.Font = new Font("Segoe UI", 10F);

        // Last Name
        lblLastName.AutoSize = true;
        lblLastName.Font = new Font("Segoe UI", 10F);
        lblLastName.Location = new Point(470, y);
        lblLastName.Text = "Last Name *";
        txtLastName.Location = new Point(470, y + 25);
        txtLastName.Size = new Size(inputWidth, 32);
        txtLastName.Font = new Font("Segoe UI", 10F);
        y += 70;

        // Date of Birth
        lblDateOfBirth.AutoSize = true;
        lblDateOfBirth.Font = new Font("Segoe UI", 10F);
        lblDateOfBirth.Location = new Point(30, y);
        lblDateOfBirth.Text = "Date of Birth *";
        dtpDateOfBirth.Location = new Point(30, y + 25);
        dtpDateOfBirth.Size = new Size(180, 32);
        dtpDateOfBirth.Font = new Font("Segoe UI", 10F);
        dtpDateOfBirth.Format = DateTimePickerFormat.Short;

        // Gender
        lblGender.AutoSize = true;
        lblGender.Font = new Font("Segoe UI", 10F);
        lblGender.Location = new Point(240, y);
        lblGender.Text = "Gender *";
        cboGender.Location = new Point(240, y + 25);
        cboGender.Size = new Size(150, 32);
        cboGender.Font = new Font("Segoe UI", 10F);
        cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGender.Items.AddRange(new object[] { "Male", "Female", "Other" });

        // Civil Status
        lblCivilStatus.AutoSize = true;
        lblCivilStatus.Font = new Font("Segoe UI", 10F);
        lblCivilStatus.Location = new Point(420, y);
        lblCivilStatus.Text = "Civil Status *";
        cboCivilStatus.Location = new Point(420, y + 25);
        cboCivilStatus.Size = new Size(150, 32);
        cboCivilStatus.Font = new Font("Segoe UI", 10F);
        cboCivilStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCivilStatus.Items.AddRange(new object[] { "Single", "Married", "Widowed", "Separated" });
        y += 85;

        // Contact Info Section Header
        lblContactInfo = CreateSectionHeader("Contact Information", 30, y);
        y += 40;

        // Address
        lblAddress.AutoSize = true;
        lblAddress.Font = new Font("Segoe UI", 10F);
        lblAddress.Location = new Point(30, y);
        lblAddress.Text = "Complete Address *";
        txtAddress.Location = new Point(30, y + 25);
        txtAddress.Size = new Size(560, 32);
        txtAddress.Font = new Font("Segoe UI", 10F);
        y += 70;

        // City
        lblCity.AutoSize = true;
        lblCity.Font = new Font("Segoe UI", 10F);
        lblCity.Location = new Point(30, y);
        lblCity.Text = "City *";
        txtCity.Location = new Point(30, y + 25);
        txtCity.Size = new Size(inputWidth, 32);
        txtCity.Font = new Font("Segoe UI", 10F);

        // Province
        lblProvince.AutoSize = true;
        lblProvince.Font = new Font("Segoe UI", 10F);
        lblProvince.Location = new Point(270, y);
        lblProvince.Text = "Province *";
        txtProvince.Location = new Point(270, y + 25);
        txtProvince.Size = new Size(inputWidth, 32);
        txtProvince.Font = new Font("Segoe UI", 10F);
        y += 70;

        // Phone
        lblPhone.AutoSize = true;
        lblPhone.Font = new Font("Segoe UI", 10F);
        lblPhone.Location = new Point(30, y);
        lblPhone.Text = "Phone Number *";
        txtPhone.Location = new Point(30, y + 25);
        txtPhone.Size = new Size(inputWidth, 32);
        txtPhone.Font = new Font("Segoe UI", 10F);

        // Email
        lblEmail.AutoSize = true;
        lblEmail.Font = new Font("Segoe UI", 10F);
        lblEmail.Location = new Point(270, y);
        lblEmail.Text = "Email Address";
        txtEmail.Location = new Point(270, y + 25);
        txtEmail.Size = new Size(inputWidth, 32);
        txtEmail.Font = new Font("Segoe UI", 10F);
        y += 85;

        // ID Section
        lblIdType.AutoSize = true;
        lblIdType.Font = new Font("Segoe UI", 10F);
        lblIdType.Location = new Point(30, y);
        lblIdType.Text = "Primary ID Type *";
        cboIdType.Location = new Point(30, y + 25);
        cboIdType.Size = new Size(inputWidth, 32);
        cboIdType.Font = new Font("Segoe UI", 10F);
        cboIdType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboIdType.Items.AddRange(new object[] { "SSS", "PhilHealth", "Pag-IBIG", "Driver's License", "Passport", "Voter's ID", "PRC ID", "National ID" });

        lblIdNumber.AutoSize = true;
        lblIdNumber.Font = new Font("Segoe UI", 10F);
        lblIdNumber.Location = new Point(270, y);
        lblIdNumber.Text = "ID Number *";
        txtIdNumber.Location = new Point(270, y + 25);
        txtIdNumber.Size = new Size(inputWidth, 32);
        txtIdNumber.Font = new Font("Segoe UI", 10F);
        y += 85;

        // Employment Info Section Header
        lblEmploymentInfo = CreateSectionHeader("Employment Information", 30, y);
        y += 40;

        // Employer
        lblEmployer.AutoSize = true;
        lblEmployer.Font = new Font("Segoe UI", 10F);
        lblEmployer.Location = new Point(30, y);
        lblEmployer.Text = "Employer/Company";
        txtEmployer.Location = new Point(30, y + 25);
        txtEmployer.Size = new Size(inputWidth, 32);
        txtEmployer.Font = new Font("Segoe UI", 10F);

        // Job Title
        lblJobTitle.AutoSize = true;
        lblJobTitle.Font = new Font("Segoe UI", 10F);
        lblJobTitle.Location = new Point(270, y);
        lblJobTitle.Text = "Job Title/Position";
        txtJobTitle.Location = new Point(270, y + 25);
        txtJobTitle.Size = new Size(inputWidth, 32);
        txtJobTitle.Font = new Font("Segoe UI", 10F);
        y += 70;

        // Monthly Income
        lblMonthlyIncome.AutoSize = true;
        lblMonthlyIncome.Font = new Font("Segoe UI", 10F);
        lblMonthlyIncome.Location = new Point(30, y);
        lblMonthlyIncome.Text = "Monthly Income (P) *";
        txtMonthlyIncome.Location = new Point(30, y + 25);
        txtMonthlyIncome.Size = new Size(180, 32);
        txtMonthlyIncome.Font = new Font("Segoe UI", 10F);

        // Years Employed
        lblYearsEmployed.AutoSize = true;
        lblYearsEmployed.Font = new Font("Segoe UI", 10F);
        lblYearsEmployed.Location = new Point(240, y);
        lblYearsEmployed.Text = "Years Employed";
        nudYearsEmployed.Location = new Point(240, y + 25);
        nudYearsEmployed.Size = new Size(100, 32);
        nudYearsEmployed.Font = new Font("Segoe UI", 10F);
        nudYearsEmployed.Maximum = 50;
        y += 90;

        // Payment Behavior Section (only for existing customers)
        grpPaymentBehavior.Text = "Payment Behavior Analysis";
        grpPaymentBehavior.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        grpPaymentBehavior.ForeColor = Color.FromArgb(30, 60, 114);
        grpPaymentBehavior.Location = new Point(30, y);
        grpPaymentBehavior.Size = new Size(660, 130);
        grpPaymentBehavior.Visible = _customerId.HasValue;

        pnlClassificationIndicator.Location = new Point(15, 30);
        pnlClassificationIndicator.Size = new Size(15, 15);
        pnlClassificationIndicator.BackColor = Color.Gray;

        lblClassification.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblClassification.Location = new Point(35, 28);
        lblClassification.Size = new Size(150, 20);
        lblClassification.ForeColor = Color.Black;
        lblClassification.Text = "Regular";

        lblCreditScore.Font = new Font("Segoe UI", 9F);
        lblCreditScore.Location = new Point(200, 28);
        lblCreditScore.Size = new Size(150, 20);
        lblCreditScore.ForeColor = Color.Black;
        lblCreditScore.Text = "Credit Score: 0";

        lblTotalLoans.Font = new Font("Segoe UI", 9F);
        lblTotalLoans.Location = new Point(15, 55);
        lblTotalLoans.Size = new Size(150, 20);
        lblTotalLoans.ForeColor = Color.Black;
        lblTotalLoans.Text = "Total Loans: 0";

        lblOnTimePayments.Font = new Font("Segoe UI", 9F);
        lblOnTimePayments.Location = new Point(170, 55);
        lblOnTimePayments.Size = new Size(150, 20);
        lblOnTimePayments.ForeColor = Color.FromArgb(46, 204, 113);
        lblOnTimePayments.Text = "On-Time: 0";

        lblLatePayments.Font = new Font("Segoe UI", 9F);
        lblLatePayments.Location = new Point(325, 55);
        lblLatePayments.Size = new Size(150, 20);
        lblLatePayments.ForeColor = Color.FromArgb(241, 196, 15);
        lblLatePayments.Text = "Late: 0";

        lblOverduePayments.Font = new Font("Segoe UI", 9F);
        lblOverduePayments.Location = new Point(480, 55);
        lblOverduePayments.Size = new Size(150, 20);
        lblOverduePayments.ForeColor = Color.FromArgb(231, 76, 60);
        lblOverduePayments.Text = "Overdue: 0";

        lblComplianceRate.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblComplianceRate.Location = new Point(15, 85);
        lblComplianceRate.Size = new Size(300, 25);
        lblComplianceRate.ForeColor = Color.Black;
        lblComplianceRate.Text = "Payment Compliance Rate: 0%";

        grpPaymentBehavior.Controls.AddRange(new Control[] {
            pnlClassificationIndicator, lblClassification, lblCreditScore,
            lblTotalLoans, lblOnTimePayments, lblLatePayments, lblOverduePayments,
            lblComplianceRate
        });

        if (_customerId.HasValue)
            y += 145;

        // Buttons
        btnSave.BackColor = Color.FromArgb(46, 204, 113);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(350, y);
        btnSave.Size = new Size(100, 45);
        btnSave.Text = "Save";
        btnSave.Click += btnSave_Click;

        btnCancel.BackColor = Color.FromArgb(149, 165, 166);
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(460, y);
        btnCancel.Size = new Size(100, 45);
        btnCancel.Text = "Cancel";
        btnCancel.Click += btnCancel_Click;

        btnBlacklist.BackColor = Color.FromArgb(231, 76, 60);
        btnBlacklist.FlatAppearance.BorderSize = 0;
        btnBlacklist.FlatStyle = FlatStyle.Flat;
        btnBlacklist.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        btnBlacklist.ForeColor = Color.White;
        btnBlacklist.Location = new Point(570, y);
        btnBlacklist.Size = new Size(120, 45);
        btnBlacklist.Text = "Blacklist";
        btnBlacklist.Visible = false;
        btnBlacklist.Click += btnBlacklist_Click;

        btnUnblacklist.BackColor = Color.FromArgb(52, 152, 219);
        btnUnblacklist.FlatAppearance.BorderSize = 0;
        btnUnblacklist.FlatStyle = FlatStyle.Flat;
        btnUnblacklist.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        btnUnblacklist.ForeColor = Color.White;
        btnUnblacklist.Location = new Point(570, y);
        btnUnblacklist.Size = new Size(120, 45);
        btnUnblacklist.Text = "Unblacklist";
        btnUnblacklist.Visible = false;
        btnUnblacklist.Click += btnUnblacklist_Click;

        // Add all controls to panel
        pnlMain.Controls.AddRange(new Control[] {
            lblTitle, lblPersonalInfo, lblFirstName, txtFirstName, lblMiddleName, txtMiddleName,
            lblLastName, txtLastName, lblDateOfBirth, dtpDateOfBirth,
            lblGender, cboGender, lblCivilStatus, cboCivilStatus,
            lblContactInfo, lblAddress, txtAddress, lblCity, txtCity, lblProvince, txtProvince,
            lblPhone, txtPhone, lblEmail, txtEmail,
            lblIdType, cboIdType, lblIdNumber, txtIdNumber,
            lblEmploymentInfo, lblEmployer, txtEmployer, lblJobTitle, txtJobTitle,
            lblMonthlyIncome, txtMonthlyIncome, lblYearsEmployed, nudYearsEmployed,
            grpPaymentBehavior,
            btnSave, btnCancel, btnBlacklist, btnUnblacklist
        });

        // Form
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(720, y + 80);
        Controls.Add(pnlMain);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CustomerForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = _customerId.HasValue ? "Edit Customer" : "Add New Customer";
        Load += CustomerForm_Load;
        
        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudYearsEmployed).EndInit();
        grpPaymentBehavior.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Label CreateSectionHeader(string text, int x, int y)
    {
        var label = new Label
        {
            Text = text,
            Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94),
            Location = new Point(x, y),
            AutoSize = true
        };
        return label;
    }

    private Panel pnlMain;
    private Label lblTitle, lblPersonalInfo, lblContactInfo, lblEmploymentInfo;
    private Label lblFirstName, lblMiddleName, lblLastName, lblDateOfBirth, lblGender, lblCivilStatus;
    private TextBox txtFirstName, txtMiddleName, txtLastName;
    private DateTimePicker dtpDateOfBirth;
    private ComboBox cboGender, cboCivilStatus;
    private Label lblAddress, lblCity, lblProvince, lblPhone, lblEmail;
    private TextBox txtAddress, txtCity, txtProvince, txtPhone, txtEmail;
    private Label lblEmployer, lblJobTitle, lblMonthlyIncome, lblYearsEmployed;
    private TextBox txtEmployer, txtJobTitle, txtMonthlyIncome;
    private NumericUpDown nudYearsEmployed;
    private Label lblIdType, lblIdNumber;
    private ComboBox cboIdType;
    private TextBox txtIdNumber;

    // Payment Behavior
    private GroupBox grpPaymentBehavior;
    private Label lblTotalLoans, lblOnTimePayments, lblLatePayments, lblOverduePayments;
    private Label lblComplianceRate, lblCreditScore, lblClassification;
    private Panel pnlClassificationIndicator;

    private Button btnSave, btnCancel, btnBlacklist, btnUnblacklist;

    private async void CustomerForm_Load(object? sender, EventArgs e)
    {
        if (_customerId.HasValue)
        {
            await LoadCustomerAsync();
            await LoadPaymentBehaviorAsync();
            ConfigureBlacklistButtons();
        }
    }

    private void ConfigureBlacklistButtons()
    {
        if (_customer == null || !SessionManager.HasPermission(Permission.ManageCustomers)) return;

        if (_customer.Classification == CustomerClassification.Blacklisted)
        {
            btnUnblacklist.Visible = true;
            btnBlacklist.Visible = false;
        }
        else
        {
            btnBlacklist.Visible = true;
            btnUnblacklist.Visible = false;
        }
    }

    private async Task LoadCustomerAsync()
    {
        try
        {
            _customer = await _customerManager.GetByIdAsync(_customerId!.Value);
            if (_customer != null)
            {
                txtFirstName.Text = _customer.FirstName;
                txtMiddleName.Text = _customer.MiddleName;
                txtLastName.Text = _customer.LastName;
                dtpDateOfBirth.Value = _customer.DateOfBirth != default ? _customer.DateOfBirth : DateTime.Today.AddYears(-25);
                cboGender.SelectedItem = _customer.Gender;
                cboCivilStatus.SelectedItem = _customer.CivilStatus;
                txtAddress.Text = _customer.Address;
                txtCity.Text = _customer.City;
                txtProvince.Text = _customer.Province;
                txtPhone.Text = _customer.Phone;
                txtEmail.Text = _customer.Email;
                cboIdType.SelectedItem = _customer.PrimaryIdType;
                txtIdNumber.Text = _customer.PrimaryIdNumber;
                txtEmployer.Text = _customer.EmployerName;
                txtJobTitle.Text = _customer.JobTitle;
                txtMonthlyIncome.Text = _customer.MonthlyIncome.ToString("N2");
                nudYearsEmployed.Value = _customer.YearsEmployed;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading customer: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadPaymentBehaviorAsync()
    {
        if (!_customerId.HasValue) return;

        try
        {
            _paymentBehavior = await _reportManager.GetCustomerPaymentBehaviorAsync(_customerId.Value);
            UpdatePaymentBehaviorDisplay();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading payment behavior: {ex.Message}");
        }
    }

    private void UpdatePaymentBehaviorDisplay()
    {
        if (_paymentBehavior == null) return;

        lblTotalLoans.Text = $"Total Loans: {_paymentBehavior.TotalLoans} (Active: {_paymentBehavior.ActiveLoans})";
        lblOnTimePayments.Text = $"On-Time: {_paymentBehavior.OnTimePayments}";
        lblLatePayments.Text = $"Late: {_paymentBehavior.LatePayments}";
        lblOverduePayments.Text = $"Overdue: {_paymentBehavior.CurrentOverdueCount}";
        lblCreditScore.Text = $"Credit Score: {_paymentBehavior.CreditScore:N2}";
        lblComplianceRate.Text = $"Payment Compliance Rate: {_paymentBehavior.PaymentComplianceRate:N1}%";

        // Update compliance rate color
        lblComplianceRate.ForeColor = _paymentBehavior.PaymentComplianceRate >= 80 ? Color.FromArgb(46, 204, 113) :
                                      _paymentBehavior.PaymentComplianceRate >= 50 ? Color.FromArgb(241, 196, 15) :
                                      Color.FromArgb(231, 76, 60);

        // Update classification indicator
        switch (_paymentBehavior.Classification)
        {
            case CustomerClassification.VIP:
                pnlClassificationIndicator.BackColor = Color.FromArgb(241, 196, 15);
                lblClassification.Text = "VIP Customer";
                lblClassification.ForeColor = Color.FromArgb(241, 196, 15);
                break;
            case CustomerClassification.Blacklisted:
                pnlClassificationIndicator.BackColor = Color.FromArgb(231, 76, 60);
                lblClassification.Text = "BLACKLISTED";
                lblClassification.ForeColor = Color.FromArgb(231, 76, 60);
                break;
            default:
                pnlClassificationIndicator.BackColor = Color.FromArgb(52, 152, 219);
                lblClassification.Text = "Regular Customer";
                lblClassification.ForeColor = Color.FromArgb(52, 152, 219);
                break;
        }

        // Show warning if should be blacklisted
        if (_paymentBehavior.ShouldBeBlacklisted && _paymentBehavior.Classification != CustomerClassification.Blacklisted)
        {
            lblClassification.Text += " ?? (Review Needed)";
        }
    }

    private async void btnBlacklist_Click(object? sender, EventArgs e)
    {
        if (_customer == null) return;

        var result = MessageBox.Show(
            $"Are you sure you want to blacklist {_customer.FullName}?\n\n" +
            "This will prevent them from applying for new loans.",
            "Confirm Blacklist",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            try
            {
                await _customerManager.BlacklistCustomerAsync(_customer.Id);
                _customer.Classification = CustomerClassification.Blacklisted;
                
                MessageBox.Show("Customer has been blacklisted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadPaymentBehaviorAsync();
                ConfigureBlacklistButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error blacklisting customer: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async void btnUnblacklist_Click(object? sender, EventArgs e)
    {
        if (_customer == null) return;

        var result = MessageBox.Show(
            $"Are you sure you want to remove {_customer.FullName} from the blacklist?\n\n" +
            "They will be able to apply for loans again.",
            "Confirm Remove from Blacklist",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                _customer.Classification = CustomerClassification.Regular;
                await _customerManager.UpdateCustomerAsync(_customer);
                
                MessageBox.Show("Customer has been removed from blacklist.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadPaymentBehaviorAsync();
                ConfigureBlacklistButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing customer from blacklist: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!ValidateInput()) return;

        try
        {
            if (_customer == null)
            {
                _customer = new Customer();
            }

            _customer.FirstName = txtFirstName.Text.Trim();
            _customer.MiddleName = txtMiddleName.Text.Trim();
            _customer.LastName = txtLastName.Text.Trim();
            _customer.DateOfBirth = dtpDateOfBirth.Value;
            _customer.Gender = cboGender.SelectedItem?.ToString() ?? "";
            _customer.CivilStatus = cboCivilStatus.SelectedItem?.ToString() ?? "";
            _customer.Address = txtAddress.Text.Trim();
            _customer.City = txtCity.Text.Trim();
            _customer.Province = txtProvince.Text.Trim();
            _customer.Phone = txtPhone.Text.Trim();
            _customer.Email = txtEmail.Text.Trim();
            _customer.PrimaryIdType = cboIdType.SelectedItem?.ToString() ?? "";
            _customer.PrimaryIdNumber = txtIdNumber.Text.Trim();
            _customer.EmployerName = txtEmployer.Text.Trim();
            _customer.JobTitle = txtJobTitle.Text.Trim();
            _customer.MonthlyIncome = decimal.Parse(txtMonthlyIncome.Text.Replace(",", ""));
            _customer.YearsEmployed = (int)nudYearsEmployed.Value;
            _customer.EmploymentStatus = string.IsNullOrEmpty(txtEmployer.Text) ? "Unemployed" : "Employed";

            if (_customerId.HasValue)
            {
                await _customerManager.UpdateCustomerAsync(_customer);
                MessageBox.Show("Customer updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                await _customerManager.RegisterCustomerAsync(_customer);
                MessageBox.Show("Customer registered successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving customer: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
        {
            MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFirstName.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtLastName.Text))
        {
            MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtLastName.Focus();
            return false;
        }
        if (cboGender.SelectedIndex == -1)
        {
            MessageBox.Show("Gender is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboGender.Focus();
            return false;
        }
        if (cboCivilStatus.SelectedIndex == -1)
        {
            MessageBox.Show("Civil status is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboCivilStatus.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtAddress.Text))
        {
            MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtAddress.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtPhone.Text))
        {
            MessageBox.Show("Phone number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPhone.Focus();
            return false;
        }
        if (!decimal.TryParse(txtMonthlyIncome.Text.Replace(",", ""), out decimal income) || income < 0)
        {
            MessageBox.Show("Please enter a valid monthly income.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtMonthlyIncome.Focus();
            return false;
        }
        return true;
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
