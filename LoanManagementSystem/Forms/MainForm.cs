using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class MainForm : Form
{
    private readonly LoanDbContext _context;
    private Button? _activeButton;
    private readonly Color _activeButtonColor = Color.FromArgb(0, 122, 204);
    private readonly Color _defaultButtonColor = Color.FromArgb(30, 60, 114);

    public MainForm()
    {
        InitializeComponent();
        _context = new LoanDbContext();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        UpdateUserInfo();
        ConfigureMenuAccess();
        RepositionMenuButtons();
        LoadDashboard();
    }

    private void UpdateUserInfo()
    {
        if (SessionManager.CurrentUser != null)
        {
            lblUserInfo.Text = $"Logged in as:\n{SessionManager.CurrentUser.FullName}";
            lblCurrentUser.Text = $"Welcome, {SessionManager.CurrentUser.FullName} ({SessionManager.CurrentUser.Role})";
        }
    }

    private void ConfigureMenuAccess()
    {
        var user = SessionManager.CurrentUser;
        if (user == null) return;

        // Dashboard - always visible
        btnDashboard.Visible = user.HasPermission(Permission.ViewDashboard);

        // Loan Products - Admin only (system configuration)
        btnProducts.Visible = user.HasPermission(Permission.ManageProducts) || 
                              user.HasPermission(Permission.ConfigureSystem);

        // Customers - Admin, Loan Officers can manage
        btnCustomers.Visible = user.HasPermission(Permission.ViewCustomers) || 
                               user.HasPermission(Permission.ManageCustomers);

        // Loans - Loan Officers process applications, Cashiers can view for release
        btnLoans.Visible = user.HasPermission(Permission.ViewLoans) || 
                           user.HasPermission(Permission.CreateLoanApplication);

        // Payments - Cashiers process payments
        btnPayments.Visible = user.HasPermission(Permission.ProcessPayment);

        // Reports - Admin and Loan Officers
        btnReports.Visible = user.HasPermission(Permission.ViewReports);

        // Users - Admin only
        btnUsers.Visible = user.HasPermission(Permission.ManageUsers);

        // For Borrowers - only show dashboard and loans (their own)
        if (user.Role == UserRole.Borrower)
        {
            btnProducts.Visible = false;
            btnCustomers.Visible = false;
            btnPayments.Visible = false;
            btnReports.Visible = false;
            btnUsers.Visible = false;
        }
    }

    /// <summary>
    /// Repositions visible menu buttons to remove empty gaps
    /// </summary>
    private void RepositionMenuButtons()
    {
        // Get all menu buttons in the order they should appear
        var menuButtons = new List<Button>
        {
            btnDashboard,
            btnProducts,
            btnCustomers,
            btnLoans,
            btnPayments,
            btnReports,
            btnUsers
        };

        int startY = 180; // Starting Y position for first button
        int buttonHeight = 50;
        int spacing = 10; // Gap between buttons
        int currentY = startY;

        foreach (var button in menuButtons)
        {
            if (button.Visible)
            {
                button.Location = new Point(button.Location.X, currentY);
                currentY += buttonHeight + spacing;
            }
        }
    }

    private void SetActiveButton(Button button)
    {
        if (_activeButton != null)
        {
            _activeButton.BackColor = _defaultButtonColor;
        }

        _activeButton = button;
        _activeButton.BackColor = _activeButtonColor;
    }

    private void LoadUserControl(UserControl control, string title)
    {
        pnlContent.Controls.Clear();
        control.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(control);
        lblPageTitle.Text = title;
    }

    private void LoadDashboard()
    {
        SetActiveButton(btnDashboard);
        var dashboard = new DashboardControl(_context);
        LoadUserControl(dashboard, "Dashboard");
    }

    private void btnDashboard_Click(object sender, EventArgs e)
    {
        LoadDashboard();
    }

    private void btnProducts_Click(object sender, EventArgs e)
    {
        if (!SessionManager.HasPermission(Permission.ManageProducts) && 
            !SessionManager.HasPermission(Permission.ConfigureSystem))
        {
            MessageBox.Show("You don't have permission to access this feature.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetActiveButton(btnProducts);
        var products = new LoanProductsControl(_context);
        LoadUserControl(products, "Loan Products");
    }

    private void btnCustomers_Click(object sender, EventArgs e)
    {
        if (!SessionManager.HasPermission(Permission.ViewCustomers) && 
            !SessionManager.HasPermission(Permission.ManageCustomers))
        {
            MessageBox.Show("You don't have permission to access this feature.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetActiveButton(btnCustomers);
        var customers = new CustomersControl(_context);
        LoadUserControl(customers, "Customer Management");
    }

    private void btnLoans_Click(object sender, EventArgs e)
    {
        if (!SessionManager.HasPermission(Permission.ViewLoans) && 
            !SessionManager.HasPermission(Permission.CreateLoanApplication))
        {
            MessageBox.Show("You don't have permission to access this feature.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetActiveButton(btnLoans);
        var loans = new LoansControl(_context);
        LoadUserControl(loans, "Loan Management");
    }

    private void btnPayments_Click(object sender, EventArgs e)
    {
        if (!SessionManager.HasPermission(Permission.ProcessPayment))
        {
            MessageBox.Show("You don't have permission to access this feature.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetActiveButton(btnPayments);
        var payments = new PaymentsControl(_context);
        LoadUserControl(payments, "Payment Processing");
    }

    private void btnReports_Click(object sender, EventArgs e)
    {
        if (!SessionManager.HasPermission(Permission.ViewReports))
        {
            MessageBox.Show("You don't have permission to access this feature.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetActiveButton(btnReports);
        var reports = new ReportsControl(_context);
        LoadUserControl(reports, "Reports");
    }

    private void btnUsers_Click(object sender, EventArgs e)
    {
        if (!SessionManager.HasPermission(Permission.ManageUsers))
        {
            MessageBox.Show("You don't have permission to access this feature.", "Access Denied", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetActiveButton(btnUsers);
        var users = new UsersControl(_context);
        LoadUserControl(users, "User Management");
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            SessionManager.Logout();
            var loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (SessionManager.IsLoggedIn)
        {
            Application.Exit();
        }
    }
}
