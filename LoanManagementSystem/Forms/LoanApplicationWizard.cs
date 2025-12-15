using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class LoanApplicationWizard : Form
{
    private readonly LoanDbContext _context;
    private readonly CustomerManager _customerManager;
    private readonly LoanProductManager _productManager;
    private readonly LoanApplicationManager _applicationManager;
    private readonly LoanManager _loanManager;

    // Wizard state
    private int _currentStep = 1;
    private const int TotalSteps = 4;

    // Data collections
    private List<Customer> _customers = [];
    private List<LoanProduct> _products = [];

    // Application data
    private Customer? _selectedCustomer;
    private LoanProduct? _selectedProduct;
    private decimal _loanAmount;
    private int _loanTerm;
    private string _purpose = string.Empty;
    private LoanComputationResult? _computation;

    // Step panels
    private Panel pnlStep1 = null!;
    private Panel pnlStep2 = null!;
    private Panel pnlStep3 = null!;
    private Panel pnlStep4 = null!;

    public LoanApplicationWizard(LoanDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerManager = new CustomerManager(context);
        _productManager = new LoanProductManager(context);
        _applicationManager = new LoanApplicationManager(context);
        _loanManager = new LoanManager(context);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        // Main form settings
        Text = "Loan Application Wizard";
        ClientSize = new Size(800, 600);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(245, 247, 250);

        // Create main container
        var pnlMain = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20)
        };

        // Create header with step indicator
        var pnlHeader = CreateHeaderPanel();

        // Create content area
        var pnlContent = new Panel
        {
            Location = new Point(20, 100),
            Size = new Size(760, 420),
            BackColor = Color.White,
            BorderStyle = BorderStyle.None
        };

        // Create step panels
        pnlStep1 = CreateStep1Panel();
        pnlStep2 = CreateStep2Panel();
        pnlStep3 = CreateStep3Panel();
        pnlStep4 = CreateStep4Panel();

        pnlContent.Controls.AddRange([pnlStep1, pnlStep2, pnlStep3, pnlStep4]);

        // Create navigation buttons
        var pnlNavigation = CreateNavigationPanel();

        pnlMain.Controls.Add(pnlHeader);
        pnlMain.Controls.Add(pnlContent);
        pnlMain.Controls.Add(pnlNavigation);

        Controls.Add(pnlMain);

        Load += LoanApplicationWizard_Load;

        ResumeLayout(false);
    }

    private Panel CreateHeaderPanel()
    {
        var panel = new Panel
        {
            Location = new Point(20, 10),
            Size = new Size(760, 80),
            BackColor = Color.White
        };

        // Title
        lblTitle = new Label
        {
            Text = "New Loan Application",
            Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 60, 114),
            Location = new Point(20, 10),
            AutoSize = true
        };

        // Step indicators
        pnlStepIndicators = new Panel
        {
            Location = new Point(20, 45),
            Size = new Size(720, 30)
        };

        CreateStepIndicators();

        panel.Controls.Add(lblTitle);
        panel.Controls.Add(pnlStepIndicators);

        return panel;
    }

    private void CreateStepIndicators()
    {
        string[] stepNames = ["Customer", "Product & Amount", "Review", "Confirmation"];
        int stepWidth = 170;

        for (int i = 0; i < TotalSteps; i++)
        {
            var stepPanel = new Panel
            {
                Location = new Point(i * stepWidth, 0),
                Size = new Size(stepWidth, 30),
                Tag = i + 1
            };

            var lblNumber = new Label
            {
                Text = (i + 1).ToString(),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(24, 24),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = i == 0 ? Color.FromArgb(52, 152, 219) : Color.LightGray,
                ForeColor = i == 0 ? Color.White : Color.DarkGray,
                Location = new Point(0, 3),
                Tag = "number"
            };

            var lblName = new Label
            {
                Text = stepNames[i],
                Font = new Font("Segoe UI", 9F),
                ForeColor = i == 0 ? Color.FromArgb(52, 152, 219) : Color.Gray,
                Location = new Point(28, 5),
                AutoSize = true,
                Tag = "name"
            };

            stepPanel.Controls.Add(lblNumber);
            stepPanel.Controls.Add(lblName);
            pnlStepIndicators.Controls.Add(stepPanel);
        }
    }

    #region Step 1 - Customer Selection

    private Panel CreateStep1Panel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            Visible = true
        };

        var lblStepTitle = new Label
        {
            Text = "Step 1: Select Customer",
            Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 60, 114),
            Location = new Point(30, 20),
            AutoSize = true
        };

        var lblInstruction = new Label
        {
            Text = "Search and select the customer applying for a loan.",
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.Gray,
            Location = new Point(30, 50),
            AutoSize = true
        };

        // Search box
        var lblSearch = new Label
        {
            Text = "Search Customer:",
            Location = new Point(30, 90),
            AutoSize = true
        };

        txtCustomerSearch = new TextBox
        {
            Location = new Point(150, 87),
            Size = new Size(300, 27),
            PlaceholderText = "Enter name or customer ID..."
        };
        txtCustomerSearch.TextChanged += TxtCustomerSearch_TextChanged;

        // Customer list
        dgvCustomers = new DataGridView
        {
            Location = new Point(30, 130),
            Size = new Size(700, 180),
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            RowHeadersVisible = false
        };
        dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;

        // Customer details
        grpCustomerDetails = new GroupBox
        {
            Text = "Selected Customer Details",
            Location = new Point(30, 320),
            Size = new Size(700, 80),
            Font = new Font("Segoe UI", 10F)
        };

        lblCustomerName = new Label { Location = new Point(15, 25), Size = new Size(300, 23) };
        lblCustomerInfo = new Label { Location = new Point(15, 48), Size = new Size(300, 23), ForeColor = Color.Gray };
        lblCreditScore = new Label { Location = new Point(350, 25), Size = new Size(200, 23) };
        lblCreditRating = new Label { Location = new Point(350, 48), Size = new Size(200, 23) };

        grpCustomerDetails.Controls.AddRange([lblCustomerName, lblCustomerInfo, lblCreditScore, lblCreditRating]);

        panel.Controls.AddRange([lblStepTitle, lblInstruction, lblSearch, txtCustomerSearch, dgvCustomers, grpCustomerDetails]);

        return panel;
    }

    private void TxtCustomerSearch_TextChanged(object? sender, EventArgs e)
    {
        FilterCustomers();
    }

    private void FilterCustomers()
    {
        var searchText = txtCustomerSearch.Text.Trim().ToLower();

        var filtered = string.IsNullOrEmpty(searchText)
            ? _customers
            : _customers.Where(c =>
                c.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                c.CustomerId.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

        var displayData = filtered.Select(c => new
        {
            c.Id,
            CustomerID = c.CustomerId,
            Name = c.FullName,
            Contact = c.Phone,
            CreditScore = c.CreditScore.ToString("N2"),
            c.Classification
        }).ToList();

        dgvCustomers.DataSource = displayData;

        if (dgvCustomers.Columns.Contains("Id"))
            dgvCustomers.Columns["Id"].Visible = false;
    }

    private void DgvCustomers_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvCustomers.SelectedRows.Count == 0)
        {
            _selectedCustomer = null;
            ClearCustomerDetails();
            return;
        }

        var selectedId = (int)dgvCustomers.SelectedRows[0].Cells["Id"].Value;
        _selectedCustomer = _customers.FirstOrDefault(c => c.Id == selectedId);

        if (_selectedCustomer != null)
        {
            UpdateCustomerDetails();
        }
    }

    private void UpdateCustomerDetails()
    {
        if (_selectedCustomer == null) return;

        lblCustomerName.Text = $"Name: {_selectedCustomer.FullName}";
        lblCustomerInfo.Text = $"ID: {_selectedCustomer.CustomerId} | Contact: {_selectedCustomer.Phone}";
        lblCreditScore.Text = $"Credit Score: {_selectedCustomer.CreditScore:N2}";

        var rating = _customerManager.GetCreditRating(_selectedCustomer.CreditScore);
        lblCreditRating.Text = $"Rating: {rating}";
        lblCreditRating.ForeColor = _selectedCustomer.CreditScore >= 70 ? Color.Green :
                                    _selectedCustomer.CreditScore >= 50 ? Color.Orange : Color.Red;
    }

    private void ClearCustomerDetails()
    {
        lblCustomerName.Text = string.Empty;
        lblCustomerInfo.Text = string.Empty;
        lblCreditScore.Text = string.Empty;
        lblCreditRating.Text = string.Empty;
    }

    #endregion

    #region Step 2 - Product & Amount

    private Panel CreateStep2Panel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            Visible = false
        };

        var lblStepTitle = new Label
        {
            Text = "Step 2: Select Product & Enter Amount",
            Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 60, 114),
            Location = new Point(30, 20),
            AutoSize = true
        };

        var lblInstruction = new Label
        {
            Text = "Choose a loan product and specify the loan details.",
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.Gray,
            Location = new Point(30, 50),
            AutoSize = true
        };

        int y = 90;

        // Product selection
        var lblProduct = new Label { Text = "Loan Product:*", Location = new Point(30, y), AutoSize = true };
        cboProduct = new ComboBox
        {
            Location = new Point(150, y - 3),
            Size = new Size(300, 27),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cboProduct.SelectedIndexChanged += CboProduct_SelectedIndexChanged;

        y += 40;

        // Product info panel
        pnlProductInfo = new Panel
        {
            Location = new Point(30, y),
            Size = new Size(700, 60),
            BackColor = Color.FromArgb(240, 248, 255),
            BorderStyle = BorderStyle.FixedSingle
        };

        lblProductDetails = new Label
        {
            Location = new Point(10, 10),
            Size = new Size(680, 40),
            Font = new Font("Segoe UI", 9F)
        };
        pnlProductInfo.Controls.Add(lblProductDetails);

        y += 75;

        // Loan amount
        var lblAmount = new Label { Text = "Loan Amount:*", Location = new Point(30, y), AutoSize = true };
        txtLoanAmount = new TextBox
        {
            Location = new Point(150, y - 3),
            Size = new Size(150, 27)
        };
        lblAmountRange = new Label
        {
            Location = new Point(310, y),
            Size = new Size(300, 23),
            ForeColor = Color.Gray
        };

        y += 40;

        // Term selection
        var lblTerm = new Label { Text = "Term (months):*", Location = new Point(30, y), AutoSize = true };
        cboTerm = new ComboBox
        {
            Location = new Point(150, y - 3),
            Size = new Size(100, 27),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        y += 40;

        // Purpose
        var lblPurpose = new Label { Text = "Purpose:", Location = new Point(30, y), AutoSize = true };
        txtPurpose = new TextBox
        {
            Location = new Point(150, y - 3),
            Size = new Size(400, 27)
        };

        y += 50;

        // Calculate button
        btnCalculate = new Button
        {
            Text = "Calculate Loan",
            Location = new Point(150, y),
            Size = new Size(150, 35),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnCalculate.FlatAppearance.BorderSize = 0;
        btnCalculate.Click += BtnCalculate_Click;

        // Computation result
        grpComputation = new GroupBox
        {
            Text = "Loan Computation",
            Location = new Point(400, 165),
            Size = new Size(330, 180),
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        };

        int gy = 25;
        lblMonthlyPayment = new Label { Location = new Point(15, gy), Size = new Size(300, 23), Font = new Font("Segoe UI", 10F) };
        gy += 25;
        lblTotalInterest = new Label { Location = new Point(15, gy), Size = new Size(300, 23), Font = new Font("Segoe UI", 10F) };
        gy += 25;
        lblTotalPayable = new Label { Location = new Point(15, gy), Size = new Size(300, 23), Font = new Font("Segoe UI", 10F) };
        gy += 25;
        lblServiceCharge = new Label { Location = new Point(15, gy), Size = new Size(300, 23), Font = new Font("Segoe UI", 10F) };
        gy += 25;
        lblProcessingFee = new Label { Location = new Point(15, gy), Size = new Size(300, 23), Font = new Font("Segoe UI", 10F) };
        gy += 25;
        lblNetProceeds = new Label
        {
            Location = new Point(15, gy),
            Size = new Size(300, 23),
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(46, 204, 113)
        };

        grpComputation.Controls.AddRange([lblMonthlyPayment, lblTotalInterest, lblTotalPayable, lblServiceCharge, lblProcessingFee, lblNetProceeds]);
        ClearComputationDisplay();

        panel.Controls.AddRange([
            lblStepTitle, lblInstruction, lblProduct, cboProduct, pnlProductInfo,
            lblAmount, txtLoanAmount, lblAmountRange, lblTerm, cboTerm,
            lblPurpose, txtPurpose, btnCalculate, grpComputation
        ]);

        return panel;
    }

    private void CboProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboProduct.SelectedItem is not LoanProduct product)
        {
            lblProductDetails.Text = string.Empty;
            lblAmountRange.Text = string.Empty;
            cboTerm.Items.Clear();
            return;
        }

        _selectedProduct = product;

        lblProductDetails.Text = $"Interest Rate: {product.AnnualInterestRate}% per annum ({product.InterestMethod})\n" +
                                 $"Service Charge: {product.ServiceChargePercent}% | Processing Fee: ?{product.ProcessingFeeFixed:N2}";

        lblAmountRange.Text = $"Range: ?{product.MinimumAmount:N0} - ?{product.MaximumAmount:N0}";

        cboTerm.Items.Clear();
        foreach (var term in product.GetAvailableTerms())
            cboTerm.Items.Add(term);

        if (cboTerm.Items.Count > 0)
            cboTerm.SelectedIndex = 0;

        ClearComputationDisplay();
    }

    private void BtnCalculate_Click(object? sender, EventArgs e)
    {
        if (!ValidateStep2(true)) return;

        _computation = _loanManager.ComputeLoan(_selectedProduct!, _loanAmount, _loanTerm);

        lblMonthlyPayment.Text = $"Monthly Payment: ?{_computation.MonthlyPayment:N2}";
        lblTotalInterest.Text = $"Total Interest: ?{_computation.TotalInterest:N2}";
        lblTotalPayable.Text = $"Total Payable: ?{_computation.TotalPayable:N2}";
        lblServiceCharge.Text = $"Service Charge: ?{_computation.ServiceCharge:N2}";
        lblProcessingFee.Text = $"Processing Fee: ?{_computation.ProcessingFee:N2}";
        lblNetProceeds.Text = $"Net Proceeds: ?{_computation.NetProceeds:N2}";
    }

    private void ClearComputationDisplay()
    {
        _computation = null;
        lblMonthlyPayment.Text = "Monthly Payment: --";
        lblTotalInterest.Text = "Total Interest: --";
        lblTotalPayable.Text = "Total Payable: --";
        lblServiceCharge.Text = "Service Charge: --";
        lblProcessingFee.Text = "Processing Fee: --";
        lblNetProceeds.Text = "Net Proceeds: --";
    }

    #endregion

    #region Step 3 - Review

    private Panel CreateStep3Panel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            Visible = false,
            AutoScroll = true
        };

        var lblStepTitle = new Label
        {
            Text = "Step 3: Review Application",
            Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 60, 114),
            Location = new Point(30, 20),
            AutoSize = true
        };

        var lblInstruction = new Label
        {
            Text = "Please review all details before submitting the application.",
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.Gray,
            Location = new Point(30, 50),
            AutoSize = true
        };

        // Customer summary
        grpCustomerSummary = new GroupBox
        {
            Text = "Customer Information",
            Location = new Point(30, 85),
            Size = new Size(340, 120),
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        };

        lblReviewCustomer = new Label
        {
            Location = new Point(15, 25),
            Size = new Size(310, 85),
            Font = new Font("Segoe UI", 10F)
        };
        grpCustomerSummary.Controls.Add(lblReviewCustomer);

        // Loan summary
        grpLoanSummary = new GroupBox
        {
            Text = "Loan Details",
            Location = new Point(390, 85),
            Size = new Size(340, 120),
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        };

        lblReviewLoan = new Label
        {
            Location = new Point(15, 25),
            Size = new Size(310, 85),
            Font = new Font("Segoe UI", 10F)
        };
        grpLoanSummary.Controls.Add(lblReviewLoan);

        // Computation summary
        grpComputationSummary = new GroupBox
        {
            Text = "Payment Summary",
            Location = new Point(30, 215),
            Size = new Size(700, 100),
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        };

        lblReviewComputation = new Label
        {
            Location = new Point(15, 25),
            Size = new Size(670, 65),
            Font = new Font("Segoe UI", 10F)
        };
        grpComputationSummary.Controls.Add(lblReviewComputation);

        // Amortization preview
        var lblAmortization = new Label
        {
            Text = "Amortization Schedule Preview:",
            Font = new Font("Segoe UI Semibold", 10F),
            Location = new Point(30, 325),
            AutoSize = true
        };

        dgvAmortization = new DataGridView
        {
            Location = new Point(30, 350),
            Size = new Size(700, 140),
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            RowHeadersVisible = false
        };

        panel.Controls.AddRange([
            lblStepTitle, lblInstruction,
            grpCustomerSummary, grpLoanSummary, grpComputationSummary,
            lblAmortization, dgvAmortization
        ]);

        return panel;
    }

    private void UpdateReviewPanel()
    {
        if (_selectedCustomer == null || _selectedProduct == null || _computation == null) return;

        // Customer summary
        lblReviewCustomer.Text = $"Name: {_selectedCustomer.FullName}\n" +
                                 $"Customer ID: {_selectedCustomer.CustomerId}\n" +
                                 $"Contact: {_selectedCustomer.Phone}\n" +
                                 $"Credit Score: {_selectedCustomer.CreditScore:N2}";

        // Loan summary
        lblReviewLoan.Text = $"Product: {_selectedProduct.ProductName}\n" +
                             $"Amount: ?{_loanAmount:N2}\n" +
                             $"Term: {_loanTerm} months\n" +
                             $"Purpose: {(string.IsNullOrEmpty(_purpose) ? "Not specified" : _purpose)}";

        // Computation summary
        lblReviewComputation.Text = $"Monthly Payment: ?{_computation.MonthlyPayment:N2}    |    " +
                                    $"Total Interest: ?{_computation.TotalInterest:N2}    |    " +
                                    $"Total Payable: ?{_computation.TotalPayable:N2}\n\n" +
                                    $"Deductions: Service Charge ?{_computation.ServiceCharge:N2} + Processing Fee ?{_computation.ProcessingFee:N2}    |    " +
                                    $"Net Proceeds: ?{_computation.NetProceeds:N2}";

        // Generate amortization schedule
        var schedule = _loanManager.GenerateAmortizationSchedule(_selectedProduct, _loanAmount, _loanTerm, DateTime.Now);
        var data = schedule.Select(s => new
        {
            No = s.Period,
            DueDate = s.DueDate.ToString("MM/dd/yyyy"),
            Payment = s.Payment.ToString("N2"),
            Principal = s.Principal.ToString("N2"),
            Interest = s.Interest.ToString("N2"),
            Balance = s.EndingBalance.ToString("N2")
        }).ToList();

        dgvAmortization.DataSource = data;
    }

    #endregion

    #region Step 4 - Confirmation

    private Panel CreateStep4Panel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            Visible = false
        };

        // Success icon placeholder
        var pnlIcon = new Panel
        {
            Location = new Point(325, 80),
            Size = new Size(150, 150),
            BackColor = Color.FromArgb(46, 204, 113)
        };

        var lblCheckmark = new Label
        {
            Text = "?",
            Font = new Font("Segoe UI", 72F, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        pnlIcon.Controls.Add(lblCheckmark);

        lblConfirmationTitle = new Label
        {
            Text = "Application Submitted Successfully!",
            Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
            ForeColor = Color.FromArgb(46, 204, 113),
            Location = new Point(30, 250),
            Size = new Size(700, 35),
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblApplicationNumber = new Label
        {
            Text = "Application Number: ---",
            Font = new Font("Segoe UI", 14F),
            ForeColor = Color.FromArgb(30, 60, 114),
            Location = new Point(30, 295),
            Size = new Size(700, 30),
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblConfirmationMessage = new Label
        {
            Text = "The loan application has been submitted and is pending review.\n" +
                   "The customer will be notified once the application is processed.",
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.Gray,
            Location = new Point(30, 340),
            Size = new Size(700, 60),
            TextAlign = ContentAlignment.MiddleCenter
        };

        panel.Controls.AddRange([pnlIcon, lblConfirmationTitle, lblApplicationNumber, lblConfirmationMessage]);

        return panel;
    }

    #endregion

    #region Navigation

    private Panel CreateNavigationPanel()
    {
        var panel = new Panel
        {
            Location = new Point(20, 530),
            Size = new Size(760, 50),
            BackColor = Color.White
        };

        btnPrevious = new Button
        {
            Text = "? Previous",
            Location = new Point(20, 10),
            Size = new Size(100, 35),
            BackColor = Color.FromArgb(149, 165, 166),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Visible = false
        };
        btnPrevious.FlatAppearance.BorderSize = 0;
        btnPrevious.Click += BtnPrevious_Click;

        btnNext = new Button
        {
            Text = "Next ?",
            Location = new Point(540, 10),
            Size = new Size(100, 35),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnNext.FlatAppearance.BorderSize = 0;
        btnNext.Click += BtnNext_Click;

        btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(650, 10),
            Size = new Size(90, 35),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.Click += BtnCancel_Click;

        panel.Controls.AddRange([btnPrevious, btnNext, btnCancel]);

        return panel;
    }

    private void BtnPrevious_Click(object? sender, EventArgs e)
    {
        if (_currentStep > 1)
        {
            _currentStep--;
            UpdateWizardView();
        }
    }

    private async void BtnNext_Click(object? sender, EventArgs e)
    {
        if (!ValidateCurrentStep()) return;

        if (_currentStep < TotalSteps)
        {
            if (_currentStep == 2)
            {
                // Store values before moving to review
                _purpose = txtPurpose.Text.Trim();
            }

            if (_currentStep == TotalSteps - 1)
            {
                // Submit application
                await SubmitApplicationAsync();
                return;
            }

            _currentStep++;

            if (_currentStep == 3)
            {
                UpdateReviewPanel();
            }

            UpdateWizardView();
        }
        else
        {
            // Close wizard
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        if (_currentStep < TotalSteps)
        {
            var result = MessageBox.Show("Are you sure you want to cancel this application?",
                "Cancel Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
        else
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private void UpdateWizardView()
    {
        // Update step panels visibility
        pnlStep1.Visible = _currentStep == 1;
        pnlStep2.Visible = _currentStep == 2;
        pnlStep3.Visible = _currentStep == 3;
        pnlStep4.Visible = _currentStep == 4;

        // Update step indicators
        foreach (Control panel in pnlStepIndicators.Controls)
        {
            if (panel.Tag is not int step) continue;
            
            var lblNumber = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Tag?.ToString() == "number");
            var lblName = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Tag?.ToString() == "name");

            if (lblNumber == null || lblName == null) continue;

            if (step < _currentStep)
            {
                lblNumber.BackColor = Color.FromArgb(46, 204, 113);
                lblNumber.ForeColor = Color.White;
                lblName.ForeColor = Color.FromArgb(46, 204, 113);
            }
            else if (step == _currentStep)
            {
                lblNumber.BackColor = Color.FromArgb(52, 152, 219);
                lblNumber.ForeColor = Color.White;
                lblName.ForeColor = Color.FromArgb(52, 152, 219);
            }
            else
            {
                lblNumber.BackColor = Color.LightGray;
                lblNumber.ForeColor = Color.DarkGray;
                lblName.ForeColor = Color.Gray;
            }
        }

        // Update navigation buttons
        btnPrevious.Visible = _currentStep > 1 && _currentStep < TotalSteps;
        btnNext.Text = _currentStep == TotalSteps - 1 ? "Submit" :
                       _currentStep == TotalSteps ? "Close" : "Next ?";
        btnNext.BackColor = _currentStep == TotalSteps - 1 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(52, 152, 219);
        btnCancel.Visible = _currentStep < TotalSteps;

        // Update title
        lblTitle.Text = _currentStep switch
        {
            1 => "New Loan Application - Customer Selection",
            2 => "New Loan Application - Loan Details",
            3 => "New Loan Application - Review",
            4 => "Application Complete",
            _ => "New Loan Application"
        };
    }

    #endregion

    #region Validation

    private bool ValidateCurrentStep()
    {
        return _currentStep switch
        {
            1 => ValidateStep1(),
            2 => ValidateStep2(true),
            3 => true, // Review step - no validation needed
            _ => true
        };
    }

    private bool ValidateStep1()
    {
        if (_selectedCustomer == null)
        {
            ShowValidationError("Please select a customer to continue.");
            return false;
        }

        if (_selectedCustomer.Classification == CustomerClassification.Blacklisted)
        {
            ShowValidationError("This customer is blacklisted and cannot apply for a loan.");
            return false;
        }

        return true;
    }

    private bool ValidateStep2(bool showMessages)
    {
        if (_selectedProduct == null)
        {
            if (showMessages) ShowValidationError("Please select a loan product.");
            return false;
        }

        if (!decimal.TryParse(txtLoanAmount.Text.Replace(",", ""), out _loanAmount) || _loanAmount <= 0)
        {
            if (showMessages) ShowValidationError("Please enter a valid loan amount.");
            return false;
        }

        if (_loanAmount < _selectedProduct.MinimumAmount || _loanAmount > _selectedProduct.MaximumAmount)
        {
            if (showMessages) ShowValidationError($"Loan amount must be between ?{_selectedProduct.MinimumAmount:N2} and ?{_selectedProduct.MaximumAmount:N2}.");
            return false;
        }

        if (cboTerm.SelectedItem is not int selectedTerm)
        {
            if (showMessages) ShowValidationError("Please select a loan term.");
            return false;
        }

        _loanTerm = selectedTerm;

        if (_computation == null)
        {
            if (showMessages) ShowValidationError("Please click 'Calculate Loan' to compute the loan details.");
            return false;
        }

        // Check customer eligibility
        if (!_customerManager.IsEligibleForLoan(_selectedCustomer!.CreditScore, _loanAmount))
        {
            if (showMessages) ShowValidationError("Customer's credit score is too low for this loan amount.");
            return false;
        }

        return true;
    }

    private void ShowValidationError(string message)
    {
        MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    #endregion

    #region Data Loading

    private async void LoanApplicationWizard_Load(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            await LoadDataAsync();
            UpdateWizardView();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task LoadDataAsync()
    {
        // Load customers
        _customers = (await _customerManager.GetAllAsync())
            .Where(c => c.IsActive && c.Classification != CustomerClassification.Blacklisted)
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToList();

        FilterCustomers();

        // Load products
        _products = (await _productManager.GetActiveProductsAsync()).ToList();
        cboProduct.DisplayMember = "ProductName";
        cboProduct.ValueMember = "Id";
        cboProduct.DataSource = _products;
    }

    #endregion

    #region Submit Application

    private async Task SubmitApplicationAsync()
    {
        try
        {
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var application = new LoanApplication
            {
                CustomerId = _selectedCustomer!.Id,
                LoanProductId = _selectedProduct!.Id,
                RequestedAmount = _loanAmount,
                RequestedTermMonths = _loanTerm,
                PurposeOfLoan = _purpose,
                DesiredReleaseDate = DateTime.Now.AddDays(3)
            };

            await _applicationManager.CreateApplicationAsync(application, SessionManager.GetCurrentUserId());

            // Move to confirmation step
            _currentStep = TotalSteps;
            lblApplicationNumber.Text = $"Application Number: {application.ApplicationNumber}";
            UpdateWizardView();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    #endregion

    #region Control Declarations

    private Label lblTitle = null!;
    private Panel pnlStepIndicators = null!;

    // Step 1 controls
    private TextBox txtCustomerSearch = null!;
    private DataGridView dgvCustomers = null!;
    private GroupBox grpCustomerDetails = null!;
    private Label lblCustomerName = null!, lblCustomerInfo = null!, lblCreditScore = null!, lblCreditRating = null!;

    // Step 2 controls
    private ComboBox cboProduct = null!, cboTerm = null!;
    private TextBox txtLoanAmount = null!, txtPurpose = null!;
    private Panel pnlProductInfo = null!;
    private Label lblProductDetails = null!, lblAmountRange = null!;
    private GroupBox grpComputation = null!;
    private Label lblMonthlyPayment = null!, lblTotalInterest = null!, lblTotalPayable = null!;
    private Label lblServiceCharge = null!, lblProcessingFee = null!, lblNetProceeds = null!;
    private Button btnCalculate = null!;

    // Step 3 controls
    private GroupBox grpCustomerSummary = null!, grpLoanSummary = null!, grpComputationSummary = null!;
    private Label lblReviewCustomer = null!, lblReviewLoan = null!, lblReviewComputation = null!;
    private DataGridView dgvAmortization = null!;

    // Step 4 controls
    private Label lblConfirmationTitle = null!, lblApplicationNumber = null!, lblConfirmationMessage = null!;

    // Navigation controls
    private Button btnPrevious = null!, btnNext = null!, btnCancel = null!;

    #endregion
}
