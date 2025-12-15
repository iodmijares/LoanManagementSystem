using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Forms;

public partial class LoginForm : Form
{
    private readonly UserManager _userManager;

    public LoginForm()
    {
        InitializeComponent();
        var context = new LoanDbContext();
        context.Database.EnsureCreated();
        _userManager = new UserManager(context);
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            MessageBox.Show("Please enter both username and password.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnLogin.Enabled = false;
        btnLogin.Text = "Signing in...";

        try
        {
            var user = await _userManager.AuthenticateAsync(txtUsername.Text.Trim(), txtPassword.Text);

            if (user != null)
            {
                SessionManager.Login(user);

                var mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred during login: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnLogin.Enabled = true;
            btnLogin.Text = "Sign In";
        }
    }

    private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            btnLogin_Click(sender, e);
            e.Handled = true;
        }
    }
}
