namespace LoanManagementSystem.Forms;

partial class MainForm
{
    private System.ComponentModel.Container components = null;

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
        pnlSidebar = new Panel();
        btnLogout = new Button();
        btnReports = new Button();
        btnPayments = new Button();
        btnLoans = new Button();
        btnCustomers = new Button();
        btnDashboard = new Button();
        btnUsers = new Button();
        btnProducts = new Button();
        lblUserInfo = new Label();
        lblAppTitle = new Label();
        pnlHeader = new Panel();
        lblPageTitle = new Label();
        lblCurrentUser = new Label();
        pnlContent = new Panel();
        pnlSidebar.SuspendLayout();
        pnlHeader.SuspendLayout();
        SuspendLayout();
        // 
        // pnlSidebar
        // 
        pnlSidebar.BackColor = Color.FromArgb(30, 60, 114);
        pnlSidebar.Controls.Add(btnLogout);
        pnlSidebar.Controls.Add(btnReports);
        pnlSidebar.Controls.Add(btnPayments);
        pnlSidebar.Controls.Add(btnLoans);
        pnlSidebar.Controls.Add(btnCustomers);
        pnlSidebar.Controls.Add(btnDashboard);
        pnlSidebar.Controls.Add(btnUsers);
        pnlSidebar.Controls.Add(btnProducts);
        pnlSidebar.Controls.Add(lblUserInfo);
        pnlSidebar.Controls.Add(lblAppTitle);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Location = new Point(0, 0);
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new Size(250, 800);
        pnlSidebar.TabIndex = 0;
        // 
        // btnLogout
        // 
        btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        btnLogout.BackColor = Color.FromArgb(192, 57, 43);
        btnLogout.Cursor = Cursors.Hand;
        btnLogout.FlatAppearance.BorderSize = 0;
        btnLogout.FlatStyle = FlatStyle.Flat;
        btnLogout.Font = new Font("Segoe UI", 11F);
        btnLogout.ForeColor = Color.White;
        btnLogout.Location = new Point(15, 730);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new Size(220, 50);
        btnLogout.TabIndex = 8;
        btnLogout.Text = "Logout";
        btnLogout.UseVisualStyleBackColor = false;
        btnLogout.Click += btnLogout_Click;
        // 
        // btnReports
        // 
        btnReports.BackColor = Color.FromArgb(30, 60, 114);
        btnReports.Cursor = Cursors.Hand;
        btnReports.FlatAppearance.BorderSize = 0;
        btnReports.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 80, 140);
        btnReports.FlatStyle = FlatStyle.Flat;
        btnReports.Font = new Font("Segoe UI", 11F);
        btnReports.ForeColor = Color.White;
        btnReports.Location = new Point(15, 480);
        btnReports.Name = "btnReports";
        btnReports.Size = new Size(220, 50);
        btnReports.TabIndex = 6;
        btnReports.Text = "Reports";
        btnReports.UseVisualStyleBackColor = false;
        btnReports.Click += btnReports_Click;
        // 
        // btnPayments
        // 
        btnPayments.BackColor = Color.FromArgb(30, 60, 114);
        btnPayments.Cursor = Cursors.Hand;
        btnPayments.FlatAppearance.BorderSize = 0;
        btnPayments.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 80, 140);
        btnPayments.FlatStyle = FlatStyle.Flat;
        btnPayments.Font = new Font("Segoe UI", 11F);
        btnPayments.ForeColor = Color.White;
        btnPayments.Location = new Point(15, 420);
        btnPayments.Name = "btnPayments";
        btnPayments.Size = new Size(220, 50);
        btnPayments.TabIndex = 5;
        btnPayments.Text = "Payments";
        btnPayments.UseVisualStyleBackColor = false;
        btnPayments.Click += btnPayments_Click;
        // 
        // btnLoans
        // 
        btnLoans.BackColor = Color.FromArgb(30, 60, 114);
        btnLoans.Cursor = Cursors.Hand;
        btnLoans.FlatAppearance.BorderSize = 0;
        btnLoans.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 80, 140);
        btnLoans.FlatStyle = FlatStyle.Flat;
        btnLoans.Font = new Font("Segoe UI", 11F);
        btnLoans.ForeColor = Color.White;
        btnLoans.Location = new Point(15, 360);
        btnLoans.Name = "btnLoans";
        btnLoans.Size = new Size(220, 50);
        btnLoans.TabIndex = 4;
        btnLoans.Text = "Loans";
        btnLoans.UseVisualStyleBackColor = false;
        btnLoans.Click += btnLoans_Click;
        // 
        // btnCustomers
        // 
        btnCustomers.BackColor = Color.FromArgb(30, 60, 114);
        btnCustomers.Cursor = Cursors.Hand;
        btnCustomers.FlatAppearance.BorderSize = 0;
        btnCustomers.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 80, 140);
        btnCustomers.FlatStyle = FlatStyle.Flat;
        btnCustomers.Font = new Font("Segoe UI", 11F);
        btnCustomers.ForeColor = Color.White;
        btnCustomers.Location = new Point(15, 300);
        btnCustomers.Name = "btnCustomers";
        btnCustomers.Size = new Size(220, 50);
        btnCustomers.TabIndex = 3;
        btnCustomers.Text = "Customers";
        btnCustomers.UseVisualStyleBackColor = false;
        btnCustomers.Click += btnCustomers_Click;
        // 
        // btnDashboard
        // 
        btnDashboard.BackColor = Color.FromArgb(0, 122, 204);
        btnDashboard.Cursor = Cursors.Hand;
        btnDashboard.FlatAppearance.BorderSize = 0;
        btnDashboard.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 80, 140);
        btnDashboard.FlatStyle = FlatStyle.Flat;
        btnDashboard.Font = new Font("Segoe UI", 11F);
        btnDashboard.ForeColor = Color.White;
        btnDashboard.Location = new Point(15, 180);
        btnDashboard.Name = "btnDashboard";
        btnDashboard.Size = new Size(220, 50);
        btnDashboard.TabIndex = 1;
        btnDashboard.Text = "Dashboard";
        btnDashboard.UseVisualStyleBackColor = false;
        btnDashboard.Click += btnDashboard_Click;
        // 
        // btnUsers
        // 
        btnUsers.BackColor = Color.FromArgb(30, 60, 114);
        btnUsers.Cursor = Cursors.Hand;
        btnUsers.FlatAppearance.BorderSize = 0;
        btnUsers.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 80, 140);
        btnUsers.FlatStyle = FlatStyle.Flat;
        btnUsers.Font = new Font("Segoe UI", 11F);
        btnUsers.ForeColor = Color.White;
        btnUsers.Location = new Point(15, 540);
        btnUsers.Name = "btnUsers";
        btnUsers.Size = new Size(220, 50);
        btnUsers.TabIndex = 7;
        btnUsers.Text = "Users";
        btnUsers.UseVisualStyleBackColor = false;
        btnUsers.Click += btnUsers_Click;
        // 
        // btnProducts
        // 
        btnProducts.BackColor = Color.FromArgb(30, 60, 114);
        btnProducts.Cursor = Cursors.Hand;
        btnProducts.FlatAppearance.BorderSize = 0;
        btnProducts.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 80, 140);
        btnProducts.FlatStyle = FlatStyle.Flat;
        btnProducts.Font = new Font("Segoe UI", 11F);
        btnProducts.ForeColor = Color.White;
        btnProducts.Location = new Point(15, 240);
        btnProducts.Name = "btnProducts";
        btnProducts.Size = new Size(220, 50);
        btnProducts.TabIndex = 2;
        btnProducts.Text = "Loan Products";
        btnProducts.UseVisualStyleBackColor = false;
        btnProducts.Click += btnProducts_Click;
        // 
        // lblUserInfo
        // 
        lblUserInfo.Font = new Font("Segoe UI", 9F);
        lblUserInfo.ForeColor = Color.LightGray;
        lblUserInfo.Location = new Point(15, 110);
        lblUserInfo.Name = "lblUserInfo";
        lblUserInfo.Size = new Size(220, 50);
        lblUserInfo.TabIndex = 0;
        lblUserInfo.Text = "Logged in as:\r\nAdmin";
        lblUserInfo.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAppTitle
        // 
        lblAppTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        lblAppTitle.ForeColor = Color.White;
        lblAppTitle.Location = new Point(15, 25);
        lblAppTitle.Name = "lblAppTitle";
        lblAppTitle.Size = new Size(220, 70);
        lblAppTitle.TabIndex = 0;
        lblAppTitle.Text = "Loan Management\r\nSystem";
        lblAppTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblCurrentUser);
        pnlHeader.Controls.Add(lblPageTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(250, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1100, 70);
        pnlHeader.TabIndex = 1;
        // 
        // lblPageTitle
        // 
        lblPageTitle.AutoSize = true;
        lblPageTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        lblPageTitle.ForeColor = Color.FromArgb(30, 60, 114);
        lblPageTitle.Location = new Point(30, 18);
        lblPageTitle.Name = "lblPageTitle";
        lblPageTitle.Size = new Size(148, 41);
        lblPageTitle.TabIndex = 0;
        lblPageTitle.Text = "Dashboard";
        // 
        // lblCurrentUser
        // 
        lblCurrentUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCurrentUser.Font = new Font("Segoe UI", 10F);
        lblCurrentUser.ForeColor = Color.Gray;
        lblCurrentUser.Location = new Point(800, 22);
        lblCurrentUser.Name = "lblCurrentUser";
        lblCurrentUser.Size = new Size(280, 25);
        lblCurrentUser.TabIndex = 1;
        lblCurrentUser.Text = "Welcome, Admin";
        lblCurrentUser.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContent
        // 
        pnlContent.BackColor = Color.FromArgb(240, 240, 245);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(250, 70);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(25);
        pnlContent.Size = new Size(1100, 730);
        pnlContent.TabIndex = 2;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1350, 800);
        Controls.Add(pnlContent);
        Controls.Add(pnlHeader);
        Controls.Add(pnlSidebar);
        MinimumSize = new Size(1350, 800);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Loan Management System";
        WindowState = FormWindowState.Maximized;
        FormClosing += MainForm_FormClosing;
        Load += MainForm_Load;
        pnlSidebar.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlSidebar;
    private Label lblAppTitle;
    private Label lblUserInfo;
    private Button btnDashboard;
    private Button btnCustomers;
    private Button btnLoans;
    private Button btnPayments;
    private Button btnReports;
    private Button btnLogout;
    private Button btnUsers;
    private Button btnProducts;
    private Panel pnlHeader;
    private Label lblPageTitle;
    private Label lblCurrentUser;
    private Panel pnlContent;
}
