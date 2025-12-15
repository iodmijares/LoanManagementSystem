using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models.Users;

namespace LoanManagementSystem.Forms;

public partial class UsersControl : UserControl
{
    private readonly LoanDbContext _context;
    private readonly UserManager _userManager;

    public UsersControl(LoanDbContext context)
    {
        _context = context;
        _userManager = new UserManager(context);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlTop = new Panel();
        btnAdd = new Button();
        btnRefresh = new Button();
        dgvUsers = new DataGridView();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
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
        btnAdd.Text = "+ Add User";
        btnAdd.Click += btnAdd_Click;

        // dgvUsers
        dgvUsers.AllowUserToAddRows = false;
        dgvUsers.AllowUserToDeleteRows = false;
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvUsers.BackgroundColor = Color.White;
        dgvUsers.BorderStyle = BorderStyle.None;
        dgvUsers.Dock = DockStyle.Fill;
        dgvUsers.Location = new Point(0, 60);
        dgvUsers.ReadOnly = true;
        dgvUsers.RowHeadersVisible = false;
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsers.Size = new Size(940, 540);

        // UsersControl
        BackColor = Color.FromArgb(240, 240, 245);
        Controls.Add(dgvUsers);
        Controls.Add(pnlTop);
        Size = new Size(940, 600);
        Load += UsersControl_Load;

        pnlTop.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Button btnAdd, btnRefresh;
    private DataGridView dgvUsers;

    private async void UsersControl_Load(object sender, EventArgs e)
    {
        await LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            var users = await _userManager.GetAllAsync();
            var data = users.Select(u => new
            {
                u.Id,
                u.Username,
                u.FullName,
                u.Email,
                u.Role,
                Status = u.IsActive ? "Active" : "Inactive",
                Created = u.CreatedAt.ToString("MM/dd/yyyy")
            }).ToList();

            dgvUsers.DataSource = data;
            if (dgvUsers.Columns.Contains("Id"))
                dgvUsers.Columns["Id"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadUsersAsync();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        using var form = new UserForm(_context);
        if (form.ShowDialog() == DialogResult.OK)
        {
            _ = LoadUsersAsync();
        }
    }
}
