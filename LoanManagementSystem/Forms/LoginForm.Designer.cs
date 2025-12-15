namespace LoanManagementSystem.Forms;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlMain = new Panel();
        pnlLogin = new Panel();
        btnLogin = new Button();
        txtPassword = new TextBox();
        txtUsername = new TextBox();
        lblPassword = new Label();
        lblUsername = new Label();
        lblTitle = new Label();
        lblSubtitle = new Label();
        pnlMain.SuspendLayout();
        pnlLogin.SuspendLayout();
        SuspendLayout();
        // 
        // pnlMain
        // 
        pnlMain.BackColor = Color.FromArgb(240, 240, 245);
        pnlMain.Controls.Add(pnlLogin);
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Location = new Point(0, 0);
        pnlMain.Name = "pnlMain";
        pnlMain.Size = new Size(550, 450);
        pnlMain.TabIndex = 0;
        // 
        // pnlLogin
        // 
        pnlLogin.Anchor = AnchorStyles.None;
        pnlLogin.BackColor = Color.White;
        pnlLogin.Controls.Add(btnLogin);
        pnlLogin.Controls.Add(txtPassword);
        pnlLogin.Controls.Add(txtUsername);
        pnlLogin.Controls.Add(lblPassword);
        pnlLogin.Controls.Add(lblUsername);
        pnlLogin.Controls.Add(lblSubtitle);
        pnlLogin.Controls.Add(lblTitle);
        pnlLogin.Location = new Point(50, 40);
        pnlLogin.Name = "pnlLogin";
        pnlLogin.Padding = new Padding(40);
        pnlLogin.Size = new Size(450, 370);
        pnlLogin.TabIndex = 0;
        // 
        // btnLogin
        // 
        btnLogin.BackColor = Color.FromArgb(0, 122, 204);
        btnLogin.Cursor = Cursors.Hand;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnLogin.ForeColor = Color.White;
        btnLogin.Location = new Point(40, 290);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(370, 50);
        btnLogin.TabIndex = 2;
        btnLogin.Text = "Sign In";
        btnLogin.UseVisualStyleBackColor = false;
        btnLogin.Click += btnLogin_Click;
        // 
        // txtPassword
        // 
        txtPassword.BorderStyle = BorderStyle.FixedSingle;
        txtPassword.Font = new Font("Segoe UI", 12F);
        txtPassword.Location = new Point(40, 230);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '?';
        txtPassword.Size = new Size(370, 34);
        txtPassword.TabIndex = 1;
        txtPassword.KeyPress += txtPassword_KeyPress;
        // 
        // txtUsername
        // 
        txtUsername.BorderStyle = BorderStyle.FixedSingle;
        txtUsername.Font = new Font("Segoe UI", 12F);
        txtUsername.Location = new Point(40, 140);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(370, 34);
        txtUsername.TabIndex = 0;
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Font = new Font("Segoe UI", 10F);
        lblPassword.ForeColor = Color.FromArgb(64, 64, 64);
        lblPassword.Location = new Point(40, 200);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(72, 23);
        lblPassword.TabIndex = 0;
        lblPassword.Text = "Password";
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Font = new Font("Segoe UI", 10F);
        lblUsername.ForeColor = Color.FromArgb(64, 64, 64);
        lblUsername.Location = new Point(40, 110);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(78, 23);
        lblUsername.TabIndex = 0;
        lblUsername.Text = "Username";
        // 
        // lblTitle
        // 
        lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(0, 122, 204);
        lblTitle.Location = new Point(40, 30);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(370, 40);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Loan Management System";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblSubtitle
        // 
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.Gray;
        lblSubtitle.Location = new Point(40, 70);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(370, 25);
        lblSubtitle.TabIndex = 0;
        lblSubtitle.Text = "Sign in to your account";
        lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // LoginForm
        // 
        AcceptButton = btnLogin;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(550, 450);
        Controls.Add(pnlMain);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Login - Loan Management System";
        pnlMain.ResumeLayout(false);
        pnlLogin.ResumeLayout(false);
        pnlLogin.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlMain;
    private Panel pnlLogin;
    private Button btnLogin;
    private TextBox txtPassword;
    private TextBox txtUsername;
    private Label lblPassword;
    private Label lblUsername;
    private Label lblTitle;
    private Label lblSubtitle;
}
