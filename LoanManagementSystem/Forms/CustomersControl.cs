using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class CustomersControl : UserControl
{
    private readonly LoanDbContext _context;
    private readonly CustomerManager _customerManager;

    public CustomersControl(LoanDbContext context)
    {
        _context = context;
        _customerManager = new CustomerManager(context);
        InitializeComponent();
        ConfigureAccessByRole();
    }

    private void ConfigureAccessByRole()
    {
        var user = SessionManager.CurrentUser;
        if (user == null) return;

        // Add Customer button - Only for those who can manage customers
        btnAdd.Visible = user.HasPermission(Permission.ManageCustomers);
        btnAdd.Enabled = user.HasPermission(Permission.ManageCustomers);
    }

    private void InitializeComponent()
    {
        pnlTop = new Panel();
        btnAdd = new Button();
        btnRefresh = new Button();
        txtSearch = new TextBox();
        lblSearch = new Label();
        dgvCustomers = new DataGridView();
        pnlDetails = new Panel();
        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
        SuspendLayout();

        // pnlTop
        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(btnAdd);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(txtSearch);
        pnlTop.Controls.Add(lblSearch);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(20, 15, 20, 15);
        pnlTop.Size = new Size(1050, 80);
        pnlTop.TabIndex = 0;

        // lblSearch
        lblSearch.AutoSize = true;
        lblSearch.Font = new Font("Segoe UI", 11F);
        lblSearch.Location = new Point(20, 27);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(62, 25);
        lblSearch.TabIndex = 0;
        lblSearch.Text = "Search:";

        // txtSearch
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.Font = new Font("Segoe UI", 11F);
        txtSearch.Location = new Point(95, 23);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search by name, ID, email, or phone...";
        txtSearch.Size = new Size(350, 32);
        txtSearch.TabIndex = 1;
        txtSearch.TextChanged += txtSearch_TextChanged;

        // btnRefresh
        btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 10F);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(465, 20);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(110, 40);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "Refresh";
        btnRefresh.Click += btnRefresh_Click;

        // btnAdd
        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.BackColor = Color.FromArgb(46, 204, 113);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(900, 20);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(130, 40);
        btnAdd.TabIndex = 3;
        btnAdd.Text = "+ Add Customer";
        btnAdd.Click += btnAdd_Click;

        // dgvCustomers
        dgvCustomers.AllowUserToAddRows = false;
        dgvCustomers.AllowUserToDeleteRows = false;
        dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCustomers.BackgroundColor = Color.White;
        dgvCustomers.BorderStyle = BorderStyle.None;
        dgvCustomers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvCustomers.ColumnHeadersHeight = 45;
        dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvCustomers.Dock = DockStyle.Fill;
        dgvCustomers.GridColor = Color.FromArgb(230, 230, 230);
        dgvCustomers.Location = new Point(0, 80);
        dgvCustomers.Name = "dgvCustomers";
        dgvCustomers.ReadOnly = true;
        dgvCustomers.RowHeadersVisible = false;
        dgvCustomers.RowTemplate.Height = 40;
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCustomers.Size = new Size(1050, 620);
        dgvCustomers.TabIndex = 1;
        dgvCustomers.CellDoubleClick += dgvCustomers_CellDoubleClick;

        // CustomersControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 240, 245);
        Controls.Add(dgvCustomers);
        Controls.Add(pnlTop);
        Name = "CustomersControl";
        Size = new Size(1050, 700);
        Load += CustomersControl_Load;
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Button btnAdd;
    private Button btnRefresh;
    private TextBox txtSearch;
    private Label lblSearch;
    private DataGridView dgvCustomers;
    private Panel pnlDetails;

    private async void CustomersControl_Load(object sender, EventArgs e)
    {
        await LoadCustomersAsync();
        StyleDataGridView(dgvCustomers);
    }

    private void StyleDataGridView(DataGridView dgv)
    {
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
        dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
    }

    private async Task LoadCustomersAsync()
    {
        try
        {
            var customers = await _customerManager.GetAllAsync();
            var data = customers.Select(c => new
            {
                c.Id,
                c.CustomerId,
                Name = c.FullName,
                c.Phone,
                c.Email,
                MonthlyIncome = c.MonthlyIncome.ToString("N2"),
                CreditScore = c.CreditScore.ToString("N2"),
                c.Classification,
                Status = c.IsActive ? "Active" : "Inactive"
            }).ToList();

            dgvCustomers.DataSource = data;
            if (dgvCustomers.Columns.Contains("Id"))
                dgvCustomers.Columns["Id"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSearch.Text))
        {
            await LoadCustomersAsync();
            return;
        }

        try
        {
            var customers = await _customerManager.SearchAsync(txtSearch.Text);
            var data = customers.Select(c => new
            {
                c.Id,
                c.CustomerId,
                Name = c.FullName,
                c.Phone,
                c.Email,
                MonthlyIncome = c.MonthlyIncome.ToString("N2"),
                CreditScore = c.CreditScore.ToString("N2"),
                c.Classification,
                Status = c.IsActive ? "Active" : "Inactive"
            }).ToList();

            dgvCustomers.DataSource = data;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error searching customers: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadCustomersAsync();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (!SessionManager.HasPermission(Permission.ManageCustomers))
        {
            MessageBox.Show("You don't have permission to add customers.", "Access Denied",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new CustomerForm(_context);
        if (form.ShowDialog() == DialogResult.OK)
        {
            _ = LoadCustomersAsync();
        }
    }

    private void dgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            var id = (int)dgvCustomers.Rows[e.RowIndex].Cells["Id"].Value;

            // Check if user can edit customers
            bool canEdit = SessionManager.HasPermission(Permission.ManageCustomers);

            using var form = new CustomerForm(_context, id);
            if (form.ShowDialog() == DialogResult.OK && canEdit)
            {
                _ = LoadCustomersAsync();
            }
        }
    }
}
