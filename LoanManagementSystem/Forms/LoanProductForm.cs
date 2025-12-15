using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Forms;

public partial class LoanProductForm : Form
{
    private readonly LoanDbContext _context;
    private readonly LoanProductManager _productManager;
    private readonly int? _productId;
    private LoanProduct? _product;

    public LoanProductForm(LoanDbContext context, int? productId = null)
    {
        _context = context;
        _productManager = new LoanProductManager(context);
        _productId = productId;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlMain = new Panel();
        lblTitle = new Label();
        
        lblProductCode = new Label();
        txtProductCode = new TextBox();
        lblProductName = new Label();
        txtProductName = new TextBox();
        lblLoanType = new Label();
        cboLoanType = new ComboBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblInterestRate = new Label();
        nudInterestRate = new NumericUpDown();
        lblInterestMethod = new Label();
        cboInterestMethod = new ComboBox();
        lblServiceCharge = new Label();
        nudServiceCharge = new NumericUpDown();
        lblProcessingFee = new Label();
        txtProcessingFee = new TextBox();
        lblMinAmount = new Label();
        txtMinAmount = new TextBox();
        lblMaxAmount = new Label();
        txtMaxAmount = new TextBox();
        lblTerms = new Label();
        txtTerms = new TextBox();
        lblPenaltyRate = new Label();
        nudPenaltyRate = new NumericUpDown();
        lblGracePeriod = new Label();
        nudGracePeriod = new NumericUpDown();
        chkRequiresCoMaker = new CheckBox();
        chkRequiresCollateral = new CheckBox();
        chkIsActive = new CheckBox();
        
        btnSave = new Button();
        btnCancel = new Button();

        pnlMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudInterestRate).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudServiceCharge).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudPenaltyRate).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudGracePeriod).BeginInit();
        SuspendLayout();

        // pnlMain
        pnlMain.AutoScroll = true;
        pnlMain.BackColor = Color.White;
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Padding = new Padding(25);

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblTitle.Location = new Point(25, 20);
        lblTitle.Text = "Loan Product Configuration";

        int y = 70;
        int labelWidth = 150;
        int inputWidth = 220;
        int rowHeight = 45;

        // Product Code
        AddLabelAndControl(lblProductCode, "Product Code *", txtProductCode, 25, y, labelWidth, 150);
        y += rowHeight;

        // Product Name
        AddLabelAndControl(lblProductName, "Product Name *", txtProductName, 25, y, labelWidth, inputWidth);
        y += rowHeight;

        // Loan Type
        lblLoanType.AutoSize = true;
        lblLoanType.Font = new Font("Segoe UI", 10F);
        lblLoanType.Location = new Point(25, y);
        lblLoanType.Text = "Loan Type *";
        cboLoanType.Location = new Point(25 + labelWidth, y - 3);
        cboLoanType.Size = new Size(inputWidth, 30);
        cboLoanType.Font = new Font("Segoe UI", 10F);
        cboLoanType.DropDownStyle = ComboBoxStyle.DropDownList;
        foreach (var type in Enum.GetValues<LoanType>())
            cboLoanType.Items.Add(type);
        y += rowHeight;

        // Description
        AddLabelAndControl(lblDescription, "Description", txtDescription, 25, y, labelWidth, 350);
        y += rowHeight;

        // Interest Rate
        lblInterestRate.AutoSize = true;
        lblInterestRate.Font = new Font("Segoe UI", 10F);
        lblInterestRate.Location = new Point(25, y);
        lblInterestRate.Text = "Annual Interest % *";
        nudInterestRate.Location = new Point(25 + labelWidth, y - 3);
        nudInterestRate.Size = new Size(100, 30);
        nudInterestRate.Font = new Font("Segoe UI", 10F);
        nudInterestRate.DecimalPlaces = 2;
        nudInterestRate.Maximum = 100;
        y += rowHeight;

        // Interest Method
        lblInterestMethod.AutoSize = true;
        lblInterestMethod.Font = new Font("Segoe UI", 10F);
        lblInterestMethod.Location = new Point(25, y);
        lblInterestMethod.Text = "Interest Method *";
        cboInterestMethod.Location = new Point(25 + labelWidth, y - 3);
        cboInterestMethod.Size = new Size(inputWidth, 30);
        cboInterestMethod.Font = new Font("Segoe UI", 10F);
        cboInterestMethod.DropDownStyle = ComboBoxStyle.DropDownList;
        foreach (var method in Enum.GetValues<InterestCalculationMethod>())
            cboInterestMethod.Items.Add(method);
        y += rowHeight;

        // Service Charge
        lblServiceCharge.AutoSize = true;
        lblServiceCharge.Font = new Font("Segoe UI", 10F);
        lblServiceCharge.Location = new Point(25, y);
        lblServiceCharge.Text = "Service Charge %";
        nudServiceCharge.Location = new Point(25 + labelWidth, y - 3);
        nudServiceCharge.Size = new Size(100, 30);
        nudServiceCharge.Font = new Font("Segoe UI", 10F);
        nudServiceCharge.DecimalPlaces = 2;
        nudServiceCharge.Maximum = 20;
        y += rowHeight;

        // Processing Fee
        AddLabelAndControl(lblProcessingFee, "Processing Fee (P)", txtProcessingFee, 25, y, labelWidth, 120);
        y += rowHeight;

        // Min Amount
        AddLabelAndControl(lblMinAmount, "Min Amount (P) *", txtMinAmount, 25, y, labelWidth, 150);
        y += rowHeight;

        // Max Amount
        AddLabelAndControl(lblMaxAmount, "Max Amount (P) *", txtMaxAmount, 25, y, labelWidth, 150);
        y += rowHeight;

        // Available Terms
        AddLabelAndControl(lblTerms, "Terms (months)", txtTerms, 25, y, labelWidth, 200);
        txtTerms.PlaceholderText = "e.g., 6,12,18,24";
        y += rowHeight;

        // Penalty Rate
        lblPenaltyRate.AutoSize = true;
        lblPenaltyRate.Font = new Font("Segoe UI", 10F);
        lblPenaltyRate.Location = new Point(25, y);
        lblPenaltyRate.Text = "Penalty % per day";
        nudPenaltyRate.Location = new Point(25 + labelWidth, y - 3);
        nudPenaltyRate.Size = new Size(100, 30);
        nudPenaltyRate.Font = new Font("Segoe UI", 10F);
        nudPenaltyRate.DecimalPlaces = 3;
        nudPenaltyRate.Maximum = 5;
        nudPenaltyRate.Increment = 0.01M;
        y += rowHeight;

        // Grace Period
        lblGracePeriod.AutoSize = true;
        lblGracePeriod.Font = new Font("Segoe UI", 10F);
        lblGracePeriod.Location = new Point(25, y);
        lblGracePeriod.Text = "Grace Period (days)";
        nudGracePeriod.Location = new Point(25 + labelWidth, y - 3);
        nudGracePeriod.Size = new Size(100, 30);
        nudGracePeriod.Font = new Font("Segoe UI", 10F);
        nudGracePeriod.Maximum = 30;
        y += rowHeight + 10;

        // Checkboxes - with proper sizing and spacing
        chkRequiresCollateral.AutoSize = true;
        chkRequiresCollateral.Font = new Font("Segoe UI", 10F);
        chkRequiresCollateral.Location = new Point(25, y);
        chkRequiresCollateral.Text = "Requires Collateral";

        chkRequiresCoMaker.AutoSize = true;
        chkRequiresCoMaker.Font = new Font("Segoe UI", 10F);
        chkRequiresCoMaker.Location = new Point(200, y);
        chkRequiresCoMaker.Text = "Requires Co-Maker";

        chkIsActive.AutoSize = true;
        chkIsActive.Font = new Font("Segoe UI", 10F);
        chkIsActive.Location = new Point(380, y);
        chkIsActive.Text = "Active";
        chkIsActive.Checked = true;
        y += rowHeight + 15;

        // Buttons
        btnSave.BackColor = Color.FromArgb(46, 204, 113);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(350, y);
        btnSave.Size = new Size(120, 45);
        btnSave.Text = "Save";
        btnSave.Click += btnSave_Click;

        btnCancel.BackColor = Color.FromArgb(149, 165, 166);
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(480, y);
        btnCancel.Size = new Size(120, 45);
        btnCancel.Text = "Cancel";
        btnCancel.Click += btnCancel_Click;

        // Add controls to panel
        pnlMain.Controls.AddRange(new Control[] {
            lblTitle, lblProductCode, txtProductCode, lblProductName, txtProductName,
            lblLoanType, cboLoanType, lblDescription, txtDescription,
            lblInterestRate, nudInterestRate, lblInterestMethod, cboInterestMethod,
            lblServiceCharge, nudServiceCharge, lblProcessingFee, txtProcessingFee,
            lblMinAmount, txtMinAmount, lblMaxAmount, txtMaxAmount,
            lblTerms, txtTerms, lblPenaltyRate, nudPenaltyRate, lblGracePeriod, nudGracePeriod,
            chkRequiresCollateral, chkRequiresCoMaker, chkIsActive,
            btnSave, btnCancel
        });

        // Form
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(630, y + 80);
        Controls.Add(pnlMain);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = _productId.HasValue ? "Edit Loan Product" : "Add Loan Product";
        Load += LoanProductForm_Load;

        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudInterestRate).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudServiceCharge).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudPenaltyRate).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudGracePeriod).EndInit();
        ResumeLayout(false);
    }

    private void AddLabelAndControl(Label label, string labelText, TextBox textBox, int x, int y, int labelWidth, int inputWidth)
    {
        label.AutoSize = true;
        label.Font = new Font("Segoe UI", 10F);
        label.Location = new Point(x, y);
        label.Text = labelText;
        textBox.Location = new Point(x + labelWidth, y - 3);
        textBox.Size = new Size(inputWidth, 30);
        textBox.Font = new Font("Segoe UI", 10F);
    }

    private Panel pnlMain;
    private Label lblTitle;
    private Label lblProductCode, lblProductName, lblLoanType, lblDescription;
    private Label lblInterestRate, lblInterestMethod, lblServiceCharge, lblProcessingFee;
    private Label lblMinAmount, lblMaxAmount, lblTerms, lblPenaltyRate, lblGracePeriod;
    private TextBox txtProductCode, txtProductName, txtDescription, txtProcessingFee;
    private TextBox txtMinAmount, txtMaxAmount, txtTerms;
    private ComboBox cboLoanType, cboInterestMethod;
    private NumericUpDown nudInterestRate, nudServiceCharge, nudPenaltyRate, nudGracePeriod;
    private CheckBox chkRequiresCoMaker, chkRequiresCollateral, chkIsActive;
    private Button btnSave, btnCancel;

    private async void LoanProductForm_Load(object sender, EventArgs e)
    {
        if (_productId.HasValue)
        {
            await LoadProductAsync();
        }
    }

    private async Task LoadProductAsync()
    {
        try
        {
            _product = await _productManager.GetByIdAsync(_productId!.Value);
            if (_product != null)
            {
                txtProductCode.Text = _product.ProductCode;
                txtProductName.Text = _product.ProductName;
                cboLoanType.SelectedItem = _product.LoanType;
                txtDescription.Text = _product.Description;
                nudInterestRate.Value = _product.AnnualInterestRate;
                cboInterestMethod.SelectedItem = _product.InterestMethod;
                nudServiceCharge.Value = _product.ServiceChargePercent;
                txtProcessingFee.Text = _product.ProcessingFeeFixed.ToString("N2");
                txtMinAmount.Text = _product.MinimumAmount.ToString("N2");
                txtMaxAmount.Text = _product.MaximumAmount.ToString("N2");
                txtTerms.Text = _product.AvailableTermsMonths;
                nudPenaltyRate.Value = _product.PenaltyRatePerDay;
                nudGracePeriod.Value = _product.GracePeriodDays;
                chkRequiresCoMaker.Checked = _product.RequiresCoMaker;
                chkRequiresCollateral.Checked = _product.RequiresCollateral;
                chkIsActive.Checked = _product.IsActive;

                txtProductCode.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;

        try
        {
            if (_product == null)
                _product = new LoanProduct();

            _product.ProductCode = txtProductCode.Text.Trim();
            _product.ProductName = txtProductName.Text.Trim();
            _product.LoanType = (LoanType)cboLoanType.SelectedItem!;
            _product.Description = txtDescription.Text.Trim();
            _product.AnnualInterestRate = nudInterestRate.Value;
            _product.InterestMethod = (InterestCalculationMethod)cboInterestMethod.SelectedItem!;
            _product.ServiceChargePercent = nudServiceCharge.Value;
            _product.ProcessingFeeFixed = decimal.Parse(txtProcessingFee.Text.Replace(",", ""));
            _product.MinimumAmount = decimal.Parse(txtMinAmount.Text.Replace(",", ""));
            _product.MaximumAmount = decimal.Parse(txtMaxAmount.Text.Replace(",", ""));
            _product.AvailableTermsMonths = txtTerms.Text.Trim();
            _product.PenaltyRatePerDay = nudPenaltyRate.Value;
            _product.GracePeriodDays = (int)nudGracePeriod.Value;
            _product.RequiresCoMaker = chkRequiresCoMaker.Checked;
            _product.RequiresCollateral = chkRequiresCollateral.Checked;
            _product.IsActive = chkIsActive.Checked;

            if (_productId.HasValue)
                await _productManager.UpdateProductAsync(_product);
            else
                await _productManager.CreateProductAsync(_product);

            MessageBox.Show("Loan product saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtProductCode.Text))
        {
            MessageBox.Show("Product code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtProductName.Text))
        {
            MessageBox.Show("Product name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (cboLoanType.SelectedIndex == -1)
        {
            MessageBox.Show("Loan type is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (cboInterestMethod.SelectedIndex == -1)
        {
            MessageBox.Show("Interest method is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
