using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class LoanApplicationForm : Form
{
    private readonly LoanDbContext _context;
    private readonly CustomerManager _customerManager;
    private readonly LoanProductManager _productManager;
    private readonly LoanApplicationManager _applicationManager;
    private readonly LoanManager _loanManager;
    private List<Customer> _customers = [];
    private List<LoanProduct> _products = [];
    private List<CoMaker> _existingCoMakers = [];

    public LoanApplicationForm(LoanDbContext context)
    {
        _context = context;
        _customerManager = new CustomerManager(context);
        _productManager = new LoanProductManager(context);
        _applicationManager = new LoanApplicationManager(context);
        _loanManager = new LoanManager(context);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlMain = new Panel();
        lblTitle = new Label();
        
        // Customer Selection
        lblCustomer = new Label();
        cboCustomer = new ComboBox();
        lblCreditScore = new Label();
        
        // Product Selection
        lblProduct = new Label();
        cboProduct = new ComboBox();
        lblProductInfo = new Label();
        
        // Loan Details
        lblAmount = new Label();
        txtAmount = new TextBox();
        lblTerm = new Label();
        cboTerm = new ComboBox();
        lblPurpose = new Label();
        txtPurpose = new TextBox();

        // Co-Maker Section
        grpCoMaker = new GroupBox();
        rbExistingCoMaker = new RadioButton();
        rbNewCoMaker = new RadioButton();
        cboExistingCoMaker = new ComboBox();
        lblCoMakerName = new Label();
        txtCoMakerName = new TextBox();
        lblCoMakerAddress = new Label();
        txtCoMakerAddress = new TextBox();
        lblCoMakerPhone = new Label();
        txtCoMakerPhone = new TextBox();
        lblCoMakerRelationship = new Label();
        txtCoMakerRelationship = new TextBox();
        lblCoMakerIdType = new Label();
        txtCoMakerIdType = new TextBox();
        lblCoMakerIdNumber = new Label();
        txtCoMakerIdNumber = new TextBox();
        lblCoMakerIncome = new Label();
        txtCoMakerIncome = new TextBox();
        lblCoMakerEmployer = new Label();
        txtCoMakerEmployer = new TextBox();

        // Collateral Section
        grpCollateral = new GroupBox();
        lblCollateralType = new Label();
        cboCollateralType = new ComboBox();
        lblCollateralDescription = new Label();
        txtCollateralDescription = new TextBox();
        lblCollateralValue = new Label();
        txtCollateralValue = new TextBox();
        
        // Computation Display
        grpComputation = new GroupBox();
        lblMonthlyPayment = new Label();
        lblTotalInterest = new Label();
        lblTotalPayable = new Label();
        lblServiceCharge = new Label();
        lblProcessingFee = new Label();
        lblNetProceeds = new Label();
        
        // Amortization Preview
        dgvAmortization = new DataGridView();
        
        btnCalculate = new Button();
        btnSubmit = new Button();
        btnCancel = new Button();

        pnlMain.SuspendLayout();
        grpComputation.SuspendLayout();
        grpCoMaker.SuspendLayout();
        grpCollateral.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAmortization).BeginInit();
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
        lblTitle.Text = "New Loan Application";

        int y = 55;
        int labelWidth = 120;
        int col2X = 400;

        // Customer
        lblCustomer.AutoSize = true;
        lblCustomer.Location = new Point(20, y);
        lblCustomer.Text = "Customer:*";
        cboCustomer.Location = new Point(labelWidth + 20, y - 3);
        cboCustomer.Size = new Size(250, 27);
        cboCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCustomer.SelectedIndexChanged += cboCustomer_SelectedIndexChanged;

        lblCreditScore.Location = new Point(col2X, y);
        lblCreditScore.Size = new Size(200, 23);
        lblCreditScore.ForeColor = Color.Gray;
        y += 35;

        // Product
        lblProduct.AutoSize = true;
        lblProduct.Location = new Point(20, y);
        lblProduct.Text = "Loan Product:*";
        cboProduct.Location = new Point(labelWidth + 20, y - 3);
        cboProduct.Size = new Size(250, 27);
        cboProduct.DropDownStyle = ComboBoxStyle.DropDownList;
        cboProduct.SelectedIndexChanged += cboProduct_SelectedIndexChanged;

        lblProductInfo.Location = new Point(col2X, y);
        lblProductInfo.Size = new Size(350, 23);
        lblProductInfo.ForeColor = Color.Gray;
        y += 35;

        // Amount
        lblAmount.AutoSize = true;
        lblAmount.Location = new Point(20, y);
        lblAmount.Text = "Loan Amount:*";
        txtAmount.Location = new Point(labelWidth + 20, y - 3);
        txtAmount.Size = new Size(150, 27);
        txtAmount.TextChanged += txtAmount_TextChanged;
        y += 35;

        // Term
        lblTerm.AutoSize = true;
        lblTerm.Location = new Point(20, y);
        lblTerm.Text = "Term (months):*";
        cboTerm.Location = new Point(labelWidth + 20, y - 3);
        cboTerm.Size = new Size(100, 27);
        cboTerm.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTerm.SelectedIndexChanged += cboTerm_SelectedIndexChanged;
        y += 35;

        // Purpose
        lblPurpose.AutoSize = true;
        lblPurpose.Location = new Point(20, y);
        lblPurpose.Text = "Purpose:";
        txtPurpose.Location = new Point(labelWidth + 20, y - 3);
        txtPurpose.Size = new Size(250, 27);
        y += 45;

        // Co-Maker GroupBox
        grpCoMaker.Location = new Point(20, y);
        grpCoMaker.Size = new Size(730, 180);
        grpCoMaker.Text = "Co-Maker Information (Required)";
        grpCoMaker.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        grpCoMaker.ForeColor = Color.FromArgb(231, 76, 60);
        grpCoMaker.Visible = false;

        // Radio buttons for co-maker selection
        rbExistingCoMaker.Text = "Select Existing Co-Maker";
        rbExistingCoMaker.Location = new Point(15, 25);
        rbExistingCoMaker.Size = new Size(180, 24);
        rbExistingCoMaker.Font = new Font("Segoe UI", 9F);
        rbExistingCoMaker.ForeColor = Color.Black;
        rbExistingCoMaker.Checked = true;
        rbExistingCoMaker.CheckedChanged += rbCoMaker_CheckedChanged;

        rbNewCoMaker.Text = "Add New Co-Maker";
        rbNewCoMaker.Location = new Point(200, 25);
        rbNewCoMaker.Size = new Size(150, 24);
        rbNewCoMaker.Font = new Font("Segoe UI", 9F);
        rbNewCoMaker.ForeColor = Color.Black;
        rbNewCoMaker.CheckedChanged += rbCoMaker_CheckedChanged;

        cboExistingCoMaker.Location = new Point(15, 55);
        cboExistingCoMaker.Size = new Size(300, 27);
        cboExistingCoMaker.DropDownStyle = ComboBoxStyle.DropDownList;
        cboExistingCoMaker.Font = new Font("Segoe UI", 9F);

        // New co-maker fields (initially hidden)
        int cmY = 55;
        int cmLabelWidth = 100;
        int cmCol2X = 370;

        lblCoMakerName.Text = "Full Name:*";
        lblCoMakerName.Location = new Point(15, cmY);
        lblCoMakerName.AutoSize = true;
        lblCoMakerName.Font = new Font("Segoe UI", 9F);
        lblCoMakerName.ForeColor = Color.Black;
        lblCoMakerName.Visible = false;
        txtCoMakerName.Location = new Point(cmLabelWidth + 15, cmY - 3);
        txtCoMakerName.Size = new Size(200, 25);
        txtCoMakerName.Font = new Font("Segoe UI", 9F);
        txtCoMakerName.Visible = false;

        lblCoMakerPhone.Text = "Phone:*";
        lblCoMakerPhone.Location = new Point(cmCol2X, cmY);
        lblCoMakerPhone.AutoSize = true;
        lblCoMakerPhone.Font = new Font("Segoe UI", 9F);
        lblCoMakerPhone.ForeColor = Color.Black;
        lblCoMakerPhone.Visible = false;
        txtCoMakerPhone.Location = new Point(cmCol2X + 80, cmY - 3);
        txtCoMakerPhone.Size = new Size(150, 25);
        txtCoMakerPhone.Font = new Font("Segoe UI", 9F);
        txtCoMakerPhone.Visible = false;

        cmY += 30;

        lblCoMakerAddress.Text = "Address:*";
        lblCoMakerAddress.Location = new Point(15, cmY);
        lblCoMakerAddress.AutoSize = true;
        lblCoMakerAddress.Font = new Font("Segoe UI", 9F);
        lblCoMakerAddress.ForeColor = Color.Black;
        lblCoMakerAddress.Visible = false;
        txtCoMakerAddress.Location = new Point(cmLabelWidth + 15, cmY - 3);
        txtCoMakerAddress.Size = new Size(200, 25);
        txtCoMakerAddress.Font = new Font("Segoe UI", 9F);
        txtCoMakerAddress.Visible = false;

        lblCoMakerRelationship.Text = "Relationship:*";
        lblCoMakerRelationship.Location = new Point(cmCol2X, cmY);
        lblCoMakerRelationship.AutoSize = true;
        lblCoMakerRelationship.Font = new Font("Segoe UI", 9F);
        lblCoMakerRelationship.ForeColor = Color.Black;
        lblCoMakerRelationship.Visible = false;
        txtCoMakerRelationship.Location = new Point(cmCol2X + 80, cmY - 3);
        txtCoMakerRelationship.Size = new Size(150, 25);
        txtCoMakerRelationship.Font = new Font("Segoe UI", 9F);
        txtCoMakerRelationship.Visible = false;

        cmY += 30;

        lblCoMakerIdType.Text = "ID Type:*";
        lblCoMakerIdType.Location = new Point(15, cmY);
        lblCoMakerIdType.AutoSize = true;
        lblCoMakerIdType.Font = new Font("Segoe UI", 9F);
        lblCoMakerIdType.ForeColor = Color.Black;
        lblCoMakerIdType.Visible = false;
        txtCoMakerIdType.Location = new Point(cmLabelWidth + 15, cmY - 3);
        txtCoMakerIdType.Size = new Size(200, 25);
        txtCoMakerIdType.Font = new Font("Segoe UI", 9F);
        txtCoMakerIdType.Visible = false;

        lblCoMakerIdNumber.Text = "ID Number:*";
        lblCoMakerIdNumber.Location = new Point(cmCol2X, cmY);
        lblCoMakerIdNumber.AutoSize = true;
        lblCoMakerIdNumber.Font = new Font("Segoe UI", 9F);
        lblCoMakerIdNumber.ForeColor = Color.Black;
        lblCoMakerIdNumber.Visible = false;
        txtCoMakerIdNumber.Location = new Point(cmCol2X + 80, cmY - 3);
        txtCoMakerIdNumber.Size = new Size(150, 25);
        txtCoMakerIdNumber.Font = new Font("Segoe UI", 9F);
        txtCoMakerIdNumber.Visible = false;

        cmY += 30;

        lblCoMakerIncome.Text = "Monthly Income:";
        lblCoMakerIncome.Location = new Point(15, cmY);
        lblCoMakerIncome.AutoSize = true;
        lblCoMakerIncome.Font = new Font("Segoe UI", 9F);
        lblCoMakerIncome.ForeColor = Color.Black;
        lblCoMakerIncome.Visible = false;
        txtCoMakerIncome.Location = new Point(cmLabelWidth + 15, cmY - 3);
        txtCoMakerIncome.Size = new Size(120, 25);
        txtCoMakerIncome.Font = new Font("Segoe UI", 9F);
        txtCoMakerIncome.Visible = false;

        lblCoMakerEmployer.Text = "Employer:";
        lblCoMakerEmployer.Location = new Point(cmCol2X, cmY);
        lblCoMakerEmployer.AutoSize = true;
        lblCoMakerEmployer.Font = new Font("Segoe UI", 9F);
        lblCoMakerEmployer.ForeColor = Color.Black;
        lblCoMakerEmployer.Visible = false;
        txtCoMakerEmployer.Location = new Point(cmCol2X + 80, cmY - 3);
        txtCoMakerEmployer.Size = new Size(150, 25);
        txtCoMakerEmployer.Font = new Font("Segoe UI", 9F);
        txtCoMakerEmployer.Visible = false;

        grpCoMaker.Controls.AddRange([
            rbExistingCoMaker, rbNewCoMaker, cboExistingCoMaker,
            lblCoMakerName, txtCoMakerName, lblCoMakerPhone, txtCoMakerPhone,
            lblCoMakerAddress, txtCoMakerAddress, lblCoMakerRelationship, txtCoMakerRelationship,
            lblCoMakerIdType, txtCoMakerIdType, lblCoMakerIdNumber, txtCoMakerIdNumber,
            lblCoMakerIncome, txtCoMakerIncome, lblCoMakerEmployer, txtCoMakerEmployer
        ]);

        y += 190;

        // Collateral GroupBox
        grpCollateral.Location = new Point(20, y);
        grpCollateral.Size = new Size(730, 100);
        grpCollateral.Text = "Collateral Information (Required)";
        grpCollateral.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        grpCollateral.ForeColor = Color.FromArgb(231, 76, 60);
        grpCollateral.Visible = false;

        lblCollateralType.Text = "Type:*";
        lblCollateralType.Location = new Point(15, 30);
        lblCollateralType.AutoSize = true;
        lblCollateralType.Font = new Font("Segoe UI", 9F);
        lblCollateralType.ForeColor = Color.Black;

        cboCollateralType.Location = new Point(100, 27);
        cboCollateralType.Size = new Size(150, 25);
        cboCollateralType.Font = new Font("Segoe UI", 9F);
        cboCollateralType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCollateralType.Items.AddRange(["Real Estate", "Vehicle", "Jewelry", "Equipment", "Other"]);

        lblCollateralValue.Text = "Est. Value:*";
        lblCollateralValue.Location = new Point(270, 30);
        lblCollateralValue.AutoSize = true;
        lblCollateralValue.Font = new Font("Segoe UI", 9F);
        lblCollateralValue.ForeColor = Color.Black;

        txtCollateralValue.Location = new Point(350, 27);
        txtCollateralValue.Size = new Size(120, 25);
        txtCollateralValue.Font = new Font("Segoe UI", 9F);

        lblCollateralDescription.Text = "Description:*";
        lblCollateralDescription.Location = new Point(15, 60);
        lblCollateralDescription.AutoSize = true;
        lblCollateralDescription.Font = new Font("Segoe UI", 9F);
        lblCollateralDescription.ForeColor = Color.Black;

        txtCollateralDescription.Location = new Point(100, 57);
        txtCollateralDescription.Size = new Size(600, 25);
        txtCollateralDescription.Font = new Font("Segoe UI", 9F);

        grpCollateral.Controls.AddRange([
            lblCollateralType, cboCollateralType,
            lblCollateralValue, txtCollateralValue,
            lblCollateralDescription, txtCollateralDescription
        ]);

        y += 110;

        // Calculate Button
        btnCalculate.BackColor = Color.FromArgb(52, 152, 219);
        btnCalculate.FlatAppearance.BorderSize = 0;
        btnCalculate.FlatStyle = FlatStyle.Flat;
        btnCalculate.Font = new Font("Segoe UI", 10F);
        btnCalculate.ForeColor = Color.White;
        btnCalculate.Location = new Point(20, y);
        btnCalculate.Size = new Size(120, 35);
        btnCalculate.Text = "Calculate";
        btnCalculate.Click += btnCalculate_Click;
        y += 50;

        // Computation Group
        grpComputation.Location = new Point(20, y);
        grpComputation.Size = new Size(300, 180);
        grpComputation.Text = "Loan Computation";
        grpComputation.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);

        int gy = 25;
        lblMonthlyPayment.Location = new Point(15, gy);
        lblMonthlyPayment.Size = new Size(270, 23);
        lblMonthlyPayment.Font = new Font("Segoe UI", 10F);
        lblMonthlyPayment.Text = "Monthly Payment: ?0.00";
        gy += 25;

        lblTotalInterest.Location = new Point(15, gy);
        lblTotalInterest.Size = new Size(270, 23);
        lblTotalInterest.Font = new Font("Segoe UI", 10F);
        lblTotalInterest.Text = "Total Interest: ?0.00";
        gy += 25;

        lblTotalPayable.Location = new Point(15, gy);
        lblTotalPayable.Size = new Size(270, 23);
        lblTotalPayable.Font = new Font("Segoe UI", 10F);
        lblTotalPayable.Text = "Total Payable: ?0.00";
        gy += 25;

        lblServiceCharge.Location = new Point(15, gy);
        lblServiceCharge.Size = new Size(270, 23);
        lblServiceCharge.Font = new Font("Segoe UI", 10F);
        lblServiceCharge.Text = "Service Charge: ?0.00";
        gy += 25;

        lblProcessingFee.Location = new Point(15, gy);
        lblProcessingFee.Size = new Size(270, 23);
        lblProcessingFee.Font = new Font("Segoe UI", 10F);
        lblProcessingFee.Text = "Processing Fee: ?0.00";
        gy += 25;

        lblNetProceeds.Location = new Point(15, gy);
        lblNetProceeds.Size = new Size(270, 23);
        lblNetProceeds.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        lblNetProceeds.ForeColor = Color.FromArgb(46, 204, 113);
        lblNetProceeds.Text = "Net Proceeds: ?0.00";

        grpComputation.Controls.AddRange([
            lblMonthlyPayment, lblTotalInterest, lblTotalPayable,
            lblServiceCharge, lblProcessingFee, lblNetProceeds
        ]);

        // Amortization Preview
        dgvAmortization.Location = new Point(340, y);
        dgvAmortization.Size = new Size(410, 180);
        dgvAmortization.AllowUserToAddRows = false;
        dgvAmortization.AllowUserToDeleteRows = false;
        dgvAmortization.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAmortization.BackgroundColor = Color.White;
        dgvAmortization.BorderStyle = BorderStyle.FixedSingle;
        dgvAmortization.ReadOnly = true;
        dgvAmortization.RowHeadersVisible = false;

        y += 200;

        // Buttons
        btnSubmit.BackColor = Color.FromArgb(46, 204, 113);
        btnSubmit.FlatAppearance.BorderSize = 0;
        btnSubmit.FlatStyle = FlatStyle.Flat;
        btnSubmit.Font = new Font("Segoe UI", 10F);
        btnSubmit.ForeColor = Color.White;
        btnSubmit.Location = new Point(540, y);
        btnSubmit.Size = new Size(100, 40);
        btnSubmit.Text = "Submit";
        btnSubmit.Click += btnSubmit_Click;

        btnCancel.BackColor = Color.FromArgb(149, 165, 166);
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 10F);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(650, y);
        btnCancel.Size = new Size(100, 40);
        btnCancel.Text = "Cancel";
        btnCancel.Click += btnCancel_Click;

        // Add controls
        pnlMain.Controls.AddRange([
            lblTitle, lblCustomer, cboCustomer, lblCreditScore,
            lblProduct, cboProduct, lblProductInfo,
            lblAmount, txtAmount, lblTerm, cboTerm,
            lblPurpose, txtPurpose,
            grpCoMaker, grpCollateral,
            btnCalculate, grpComputation, dgvAmortization,
            btnSubmit, btnCancel
        ]);

        // Form
        ClientSize = new Size(790, 750);
        Controls.Add(pnlMain);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "New Loan Application";
        Load += LoanApplicationForm_Load;

        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        grpComputation.ResumeLayout(false);
        grpCoMaker.ResumeLayout(false);
        grpCollateral.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAmortization).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlMain;
    private Label lblTitle;
    private Label lblCustomer, lblCreditScore, lblProduct, lblProductInfo;
    private Label lblAmount, lblTerm, lblPurpose;
    private ComboBox cboCustomer, cboProduct, cboTerm;
    private TextBox txtAmount, txtPurpose;

    // Co-Maker controls
    private GroupBox grpCoMaker;
    private RadioButton rbExistingCoMaker, rbNewCoMaker;
    private ComboBox cboExistingCoMaker;
    private Label lblCoMakerName, lblCoMakerAddress, lblCoMakerPhone, lblCoMakerRelationship;
    private Label lblCoMakerIdType, lblCoMakerIdNumber, lblCoMakerIncome, lblCoMakerEmployer;
    private TextBox txtCoMakerName, txtCoMakerAddress, txtCoMakerPhone, txtCoMakerRelationship;
    private TextBox txtCoMakerIdType, txtCoMakerIdNumber, txtCoMakerIncome, txtCoMakerEmployer;

    // Collateral controls
    private GroupBox grpCollateral;
    private Label lblCollateralType, lblCollateralDescription, lblCollateralValue;
    private ComboBox cboCollateralType;
    private TextBox txtCollateralDescription, txtCollateralValue;

    private GroupBox grpComputation;
    private Label lblMonthlyPayment, lblTotalInterest, lblTotalPayable;
    private Label lblServiceCharge, lblProcessingFee, lblNetProceeds;
    private DataGridView dgvAmortization;
    private Button btnCalculate, btnSubmit, btnCancel;

    private async void LoanApplicationForm_Load(object? sender, EventArgs e)
    {
        await LoadCustomersAsync();
        await LoadProductsAsync();
    }

    private async Task LoadCustomersAsync()
    {
        _customers = (await _customerManager.GetAllAsync())
            .Where(c => c.IsActive && c.Classification != CustomerClassification.Blacklisted)
            .ToList();
        cboCustomer.DisplayMember = "FullName";
        cboCustomer.ValueMember = "Id";
        cboCustomer.DataSource = _customers;
    }

    private async Task LoadProductsAsync()
    {
        _products = (await _productManager.GetActiveProductsAsync()).ToList();
        cboProduct.DisplayMember = "ProductName";
        cboProduct.ValueMember = "Id";
        cboProduct.DataSource = _products;
    }

    private void LoadExistingCoMakers(int customerId)
    {
        _existingCoMakers = _context.CoMakers.Where(c => c.CustomerId == customerId).ToList();
        cboExistingCoMaker.DataSource = null;
        cboExistingCoMaker.DisplayMember = "FullName";
        cboExistingCoMaker.ValueMember = "Id";
        cboExistingCoMaker.DataSource = _existingCoMakers;

        // If no existing co-makers, default to new co-maker
        if (_existingCoMakers.Count == 0)
        {
            rbNewCoMaker.Checked = true;
            rbExistingCoMaker.Enabled = false;
        }
        else
        {
            rbExistingCoMaker.Enabled = true;
        }
    }

    private void cboCustomer_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboCustomer.SelectedItem is Customer customer)
        {
            var rating = _customerManager.GetCreditRating(customer.CreditScore);
            lblCreditScore.Text = $"Credit Score: {customer.CreditScore:N2} ({rating})";
            lblCreditScore.ForeColor = customer.CreditScore >= 70 ? Color.Green : 
                                       customer.CreditScore >= 50 ? Color.Orange : Color.Red;

            // Load existing co-makers for this customer
            LoadExistingCoMakers(customer.Id);
        }
    }

    private void cboProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboProduct.SelectedItem is LoanProduct product)
        {
            lblProductInfo.Text = $"Rate: {product.AnnualInterestRate}% | Range: ?{product.MinimumAmount:N0} - ?{product.MaximumAmount:N0}";
            
            cboTerm.Items.Clear();
            foreach (var term in product.GetAvailableTerms())
                cboTerm.Items.Add(term);
            if (cboTerm.Items.Count > 0)
                cboTerm.SelectedIndex = 0;

            // Show/hide Co-Maker section based on product requirement
            grpCoMaker.Visible = product.RequiresCoMaker;
            grpCoMaker.Text = product.RequiresCoMaker 
                ? "Co-Maker Information (Required)" 
                : "Co-Maker Information (Optional)";

            // Show/hide Collateral section based on product requirement
            grpCollateral.Visible = product.RequiresCollateral;
            grpCollateral.Text = product.RequiresCollateral 
                ? "Collateral Information (Required)" 
                : "Collateral Information (Optional)";

            // Adjust form layout based on visible sections
            AdjustFormLayout();
        }
    }

    private void AdjustFormLayout()
    {
        int y = 235; // Starting Y after purpose field

        if (grpCoMaker.Visible)
        {
            grpCoMaker.Location = new Point(20, y);
            y += grpCoMaker.Height + 10;
        }

        if (grpCollateral.Visible)
        {
            grpCollateral.Location = new Point(20, y);
            y += grpCollateral.Height + 10;
        }

        btnCalculate.Location = new Point(20, y);
        y += 50;

        grpComputation.Location = new Point(20, y);
        dgvAmortization.Location = new Point(340, y);
        y += 200;

        btnSubmit.Location = new Point(540, y);
        btnCancel.Location = new Point(650, y);

        // Adjust form height
        int formHeight = y + 80;
        ClientSize = new Size(790, Math.Max(formHeight, 500));
    }

    private void rbCoMaker_CheckedChanged(object? sender, EventArgs e)
    {
        bool showNewCoMakerFields = rbNewCoMaker.Checked;

        // Toggle visibility of existing co-maker dropdown
        cboExistingCoMaker.Visible = !showNewCoMakerFields;

        // Toggle visibility of new co-maker fields
        lblCoMakerName.Visible = showNewCoMakerFields;
        txtCoMakerName.Visible = showNewCoMakerFields;
        lblCoMakerPhone.Visible = showNewCoMakerFields;
        txtCoMakerPhone.Visible = showNewCoMakerFields;
        lblCoMakerAddress.Visible = showNewCoMakerFields;
        txtCoMakerAddress.Visible = showNewCoMakerFields;
        lblCoMakerRelationship.Visible = showNewCoMakerFields;
        txtCoMakerRelationship.Visible = showNewCoMakerFields;
        lblCoMakerIdType.Visible = showNewCoMakerFields;
        txtCoMakerIdType.Visible = showNewCoMakerFields;
        lblCoMakerIdNumber.Visible = showNewCoMakerFields;
        txtCoMakerIdNumber.Visible = showNewCoMakerFields;
        lblCoMakerIncome.Visible = showNewCoMakerFields;
        txtCoMakerIncome.Visible = showNewCoMakerFields;
        lblCoMakerEmployer.Visible = showNewCoMakerFields;
        txtCoMakerEmployer.Visible = showNewCoMakerFields;
    }

    private void txtAmount_TextChanged(object? sender, EventArgs e) => ClearComputation();
    private void cboTerm_SelectedIndexChanged(object? sender, EventArgs e) => ClearComputation();

    private void ClearComputation()
    {
        lblMonthlyPayment.Text = "Monthly Payment: ?0.00";
        lblTotalInterest.Text = "Total Interest: ?0.00";
        lblTotalPayable.Text = "Total Payable: ?0.00";
        lblServiceCharge.Text = "Service Charge: ?0.00";
        lblProcessingFee.Text = "Processing Fee: ?0.00";
        lblNetProceeds.Text = "Net Proceeds: ?0.00";
        dgvAmortization.DataSource = null;
    }

    private void btnCalculate_Click(object? sender, EventArgs e)
    {
        if (!ValidateBasicInput(false)) return;

        var product = cboProduct.SelectedItem as LoanProduct;
        if (product == null) return;

        if (!decimal.TryParse(txtAmount.Text.Replace(",", ""), out decimal amount)) return;
        if (cboTerm.SelectedItem is not int term) return;

        var computation = _loanManager.ComputeLoan(product, amount, term);

        lblMonthlyPayment.Text = $"Monthly Payment: ?{computation.MonthlyPayment:N2}";
        lblTotalInterest.Text = $"Total Interest: ?{computation.TotalInterest:N2}";
        lblTotalPayable.Text = $"Total Payable: ?{computation.TotalPayable:N2}";
        lblServiceCharge.Text = $"Service Charge: ?{computation.ServiceCharge:N2}";
        lblProcessingFee.Text = $"Processing Fee: ?{computation.ProcessingFee:N2}";
        lblNetProceeds.Text = $"Net Proceeds: ?{computation.NetProceeds:N2}";

        // Generate amortization preview
        var schedule = _loanManager.GenerateAmortizationSchedule(product, amount, term, DateTime.Now);
        var data = schedule.Select(s => new
        {
            No = s.Period,
            DueDate = s.DueDate.ToString("MM/dd/yy"),
            Payment = s.Payment.ToString("N2"),
            Principal = s.Principal.ToString("N2"),
            Interest = s.Interest.ToString("N2"),
            Balance = s.EndingBalance.ToString("N2")
        }).ToList();

        dgvAmortization.DataSource = data;
    }

    private async void btnSubmit_Click(object? sender, EventArgs e)
    {
        if (!ValidateAllInput()) return;

        try
        {
            var customer = cboCustomer.SelectedItem as Customer;
            var product = cboProduct.SelectedItem as LoanProduct;
            decimal amount = decimal.Parse(txtAmount.Text.Replace(",", ""));
            int term = (int)cboTerm.SelectedItem!;

            // Check eligibility
            if (!_customerManager.IsEligibleForLoan(customer!.CreditScore, amount))
            {
                MessageBox.Show("Customer's credit score is too low for this loan amount.", "Eligibility Check",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Handle Co-Maker
            int? coMakerId = null;
            if (product!.RequiresCoMaker || grpCoMaker.Visible)
            {
                if (rbExistingCoMaker.Checked && cboExistingCoMaker.SelectedItem is CoMaker existingCoMaker)
                {
                    coMakerId = existingCoMaker.Id;
                }
                else if (rbNewCoMaker.Checked)
                {
                    // Create new co-maker
                    var newCoMaker = new CoMaker
                    {
                        CustomerId = customer.Id,
                        FullName = txtCoMakerName.Text.Trim(),
                        Address = txtCoMakerAddress.Text.Trim(),
                        Phone = txtCoMakerPhone.Text.Trim(),
                        Relationship = txtCoMakerRelationship.Text.Trim(),
                        IdType = txtCoMakerIdType.Text.Trim(),
                        IdNumber = txtCoMakerIdNumber.Text.Trim(),
                        MonthlyIncome = decimal.TryParse(txtCoMakerIncome.Text.Replace(",", ""), out var income) ? income : 0,
                        EmployerName = txtCoMakerEmployer.Text.Trim()
                    };

                    _context.CoMakers.Add(newCoMaker);
                    await _context.SaveChangesAsync();
                    coMakerId = newCoMaker.Id;
                }
            }

            var application = new LoanApplication
            {
                CustomerId = customer.Id,
                LoanProductId = product.Id,
                RequestedAmount = amount,
                RequestedTermMonths = term,
                PurposeOfLoan = txtPurpose.Text.Trim(),
                DesiredReleaseDate = DateTime.Now.AddDays(3),
                CoMakerId = coMakerId
            };

            // Handle Collateral
            if (product.RequiresCollateral || grpCollateral.Visible)
            {
                application.CollateralType = cboCollateralType.SelectedItem?.ToString();
                application.CollateralDescription = txtCollateralDescription.Text.Trim();
                if (decimal.TryParse(txtCollateralValue.Text.Replace(",", ""), out var collateralValue))
                {
                    application.CollateralValue = collateralValue;
                }
            }

            await _applicationManager.CreateApplicationAsync(application, SessionManager.GetCurrentUserId());

            MessageBox.Show($"Loan application submitted successfully!\nApplication No: {application.ApplicationNumber}",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error submitting application: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool ValidateBasicInput(bool showMessages)
    {
        if (cboCustomer.SelectedItem == null)
        {
            if (showMessages) MessageBox.Show("Please select a customer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (cboProduct.SelectedItem == null)
        {
            if (showMessages) MessageBox.Show("Please select a loan product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (!decimal.TryParse(txtAmount.Text.Replace(",", ""), out decimal amount) || amount <= 0)
        {
            if (showMessages) MessageBox.Show("Please enter a valid loan amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (cboTerm.SelectedItem == null)
        {
            if (showMessages) MessageBox.Show("Please select a loan term.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var product = cboProduct.SelectedItem as LoanProduct;
        if (product != null && (amount < product.MinimumAmount || amount > product.MaximumAmount))
        {
            if (showMessages) MessageBox.Show($"Loan amount must be between ?{product.MinimumAmount:N2} and ?{product.MaximumAmount:N2}.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private bool ValidateAllInput()
    {
        if (!ValidateBasicInput(true)) return false;

        var product = cboProduct.SelectedItem as LoanProduct;

        // Validate Co-Maker if required
        if (product?.RequiresCoMaker == true)
        {
            if (rbExistingCoMaker.Checked)
            {
                if (cboExistingCoMaker.SelectedItem == null)
                {
                    MessageBox.Show("Please select a co-maker or add a new one.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else if (rbNewCoMaker.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtCoMakerName.Text))
                {
                    MessageBox.Show("Please enter the co-maker's full name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtCoMakerAddress.Text))
                {
                    MessageBox.Show("Please enter the co-maker's address.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtCoMakerPhone.Text))
                {
                    MessageBox.Show("Please enter the co-maker's phone number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtCoMakerRelationship.Text))
                {
                    MessageBox.Show("Please enter the co-maker's relationship to the borrower.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtCoMakerIdType.Text))
                {
                    MessageBox.Show("Please enter the co-maker's ID type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtCoMakerIdNumber.Text))
                {
                    MessageBox.Show("Please enter the co-maker's ID number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
        }

        // Validate Collateral if required
        if (product?.RequiresCollateral == true)
        {
            if (cboCollateralType.SelectedItem == null)
            {
                MessageBox.Show("Please select the collateral type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCollateralDescription.Text))
            {
                MessageBox.Show("Please enter the collateral description.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(txtCollateralValue.Text.Replace(",", ""), out decimal value) || value <= 0)
            {
                MessageBox.Show("Please enter a valid collateral value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        return true;
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
