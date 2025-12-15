using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Forms;

public partial class LoanProductsControl : UserControl
{
    private readonly LoanDbContext _context;
    private readonly LoanProductManager _productManager;

    public LoanProductsControl(LoanDbContext context)
    {
        _context = context;
        _productManager = new LoanProductManager(context);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlTop = new Panel();
        btnAdd = new Button();
        btnRefresh = new Button();
        dgvProducts = new DataGridView();
        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
        SuspendLayout();

        // pnlTop
        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(btnAdd);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Size = new Size(940, 60);

        // btnRefresh
        btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 10F);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(20, 12);
        btnRefresh.Size = new Size(100, 36);
        btnRefresh.Text = "Refresh";
        btnRefresh.Click += btnRefresh_Click;

        // btnAdd
        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.BackColor = Color.FromArgb(46, 204, 113);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.Font = new Font("Segoe UI", 10F);
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(820, 12);
        btnAdd.Size = new Size(110, 36);
        btnAdd.Text = "+ Add New";
        btnAdd.Click += btnAdd_Click;

        // dgvProducts
        dgvProducts.AllowUserToAddRows = false;
        dgvProducts.AllowUserToDeleteRows = false;
        dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProducts.BackgroundColor = Color.White;
        dgvProducts.BorderStyle = BorderStyle.None;
        dgvProducts.Dock = DockStyle.Fill;
        dgvProducts.Location = new Point(0, 60);
        dgvProducts.ReadOnly = true;
        dgvProducts.RowHeadersVisible = false;
        dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProducts.Size = new Size(940, 540);
        dgvProducts.CellDoubleClick += dgvProducts_CellDoubleClick;

        // LoanProductsControl
        BackColor = Color.FromArgb(240, 240, 245);
        Controls.Add(dgvProducts);
        Controls.Add(pnlTop);
        Size = new Size(940, 600);
        Load += LoanProductsControl_Load;
        
        pnlTop.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Button btnAdd, btnRefresh;
    private DataGridView dgvProducts;

    private async void LoanProductsControl_Load(object sender, EventArgs e)
    {
        await LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        try
        {
            var products = await _productManager.GetAllAsync();
            var data = products.Select(p => new
            {
                p.Id,
                p.ProductCode,
                p.ProductName,
                Type = p.LoanType.ToString(),
                InterestRate = $"{p.AnnualInterestRate}%",
                Method = p.InterestMethod.ToString(),
                MinAmount = p.MinimumAmount.ToString("N2"),
                MaxAmount = p.MaximumAmount.ToString("N2"),
                Terms = p.AvailableTermsMonths,
                Status = p.IsActive ? "Active" : "Inactive"
            }).ToList();

            dgvProducts.DataSource = data;
            if (dgvProducts.Columns.Contains("Id"))
                dgvProducts.Columns["Id"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadProductsAsync();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        using var form = new LoanProductForm(_context);
        if (form.ShowDialog() == DialogResult.OK)
        {
            _ = LoadProductsAsync();
        }
    }

    private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            var id = (int)dgvProducts.Rows[e.RowIndex].Cells["Id"].Value;
            using var form = new LoanProductForm(_context, id);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = LoadProductsAsync();
            }
        }
    }
}
