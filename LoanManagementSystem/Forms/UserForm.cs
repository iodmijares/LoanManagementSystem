using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Utilities;

namespace LoanManagementSystem.Forms;

public partial class UserForm : Form
{
    private readonly LoanDbContext _context;
    private readonly UserManager _userManager;

    public UserForm(LoanDbContext context)
    {
        _context = context;
        _userManager = new UserManager(context);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        pnlMain = new Panel();
        lblTitle = new Label();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        lblFullName = new Label();
        txtFullName = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblRole = new Label();
        cboRole = new ComboBox();
        btnSave = new Button();
        btnCancel = new Button();

        pnlMain.SuspendLayout();
        SuspendLayout();

        // pnlMain
        pnlMain.BackColor = Color.White;
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Padding = new Padding(20);

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Text = "Add New User";

        int y = 70;
        int labelWidth = 100;

        AddLabelAndTextBox(lblUsername, "Username:*", txtUsername, 20, y, labelWidth, 200);
        y += 40;
        AddLabelAndTextBox(lblPassword, "Password:*", txtPassword, 20, y, labelWidth, 200);
        txtPassword.PasswordChar = '?';
        y += 40;
        AddLabelAndTextBox(lblFullName, "Full Name:*", txtFullName, 20, y, labelWidth, 250);
        y += 40;
        AddLabelAndTextBox(lblEmail, "Email:", txtEmail, 20, y, labelWidth, 250);
        y += 40;
        AddLabelAndTextBox(lblPhone, "Phone:", txtPhone, 20, y, labelWidth, 150);
        y += 40;

        lblRole.AutoSize = true;
        lblRole.Location = new Point(20, y);
        lblRole.Text = "Role:*";
        cboRole.Location = new Point(labelWidth + 20, y - 3);
        cboRole.Size = new Size(150, 27);
        cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRole.Items.AddRange(new object[] { "Admin", "LoanOfficer", "Cashier" });
        y += 50;

        // Buttons
        btnSave.BackColor = Color.FromArgb(46, 204, 113);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 10F);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(170, y);
        btnSave.Size = new Size(100, 40);
        btnSave.Text = "Save";
        btnSave.Click += btnSave_Click;

        btnCancel.BackColor = Color.FromArgb(149, 165, 166);
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 10F);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(280, y);
        btnCancel.Size = new Size(100, 40);
        btnCancel.Text = "Cancel";
        btnCancel.Click += btnCancel_Click;

        pnlMain.Controls.AddRange(new Control[] {
            lblTitle, lblUsername, txtUsername, lblPassword, txtPassword,
            lblFullName, txtFullName, lblEmail, txtEmail, lblPhone, txtPhone,
            lblRole, cboRole, btnSave, btnCancel
        });

        // Form
        ClientSize = new Size(420, y + 80);
        Controls.Add(pnlMain);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Add User";

        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ResumeLayout(false);
    }

    private void AddLabelAndTextBox(Label label, string labelText, TextBox textBox, int x, int y, int labelWidth, int inputWidth)
    {
        label.AutoSize = true;
        label.Location = new Point(x, y);
        label.Text = labelText;
        textBox.Location = new Point(x + labelWidth, y - 3);
        textBox.Size = new Size(inputWidth, 27);
    }

    private Panel pnlMain;
    private Label lblTitle, lblUsername, lblPassword, lblFullName, lblEmail, lblPhone, lblRole;
    private TextBox txtUsername, txtPassword, txtFullName, txtEmail, txtPhone;
    private ComboBox cboRole;
    private Button btnSave, btnCancel;

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            string.IsNullOrWhiteSpace(txtFullName.Text) ||
            cboRole.SelectedIndex == -1)
        {
            MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            string role = cboRole.SelectedItem!.ToString()!;
            User user;

            switch (role)
            {
                case "Admin":
                    user = await _userManager.CreateAdminAsync(
                        txtUsername.Text.Trim(),
                        txtPassword.Text,
                        txtFullName.Text.Trim(),
                        txtEmail.Text.Trim(),
                        txtPhone.Text.Trim());
                    break;
                case "LoanOfficer":
                    user = await _userManager.CreateLoanOfficerAsync(
                        txtUsername.Text.Trim(),
                        txtPassword.Text,
                        txtFullName.Text.Trim(),
                        txtEmail.Text.Trim(),
                        txtPhone.Text.Trim(),
                        100000M);
                    break;
                case "Cashier":
                    user = await _userManager.CreateCashierAsync(
                        txtUsername.Text.Trim(),
                        txtPassword.Text,
                        txtFullName.Text.Trim(),
                        txtEmail.Text.Trim(),
                        txtPhone.Text.Trim(),
                        500000M);
                    break;
                default:
                    throw new InvalidOperationException("Invalid role selected");
            }

            MessageBox.Show("User created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
