using LoanManagementSystem.Data;
using LoanManagementSystem.Managers;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace LoanManagementSystem.Forms;

public partial class DashboardControl : UserControl
{
    private readonly LoanDbContext _context;
    private readonly ReportManager _reportManager;
    private readonly SemaphoreSlim _loadLock = new(1, 1);
    private readonly CancellationTokenSource _cts = new();
    private const int MaxRetryAttempts = 3;
    private const int RetryDelayMs = 500;

    // Chart data
    private List<MonthlyCollectionData> _monthlyCollections = [];
    private ExtendedDashboardStats? _extendedStats;

    public DashboardControl(LoanDbContext context)
    {
        InitializeComponent();
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _reportManager = new ReportManager(context);

        // Register for cleanup when the control is disposed
        Disposed += DashboardControl_Disposed;

        // Setup chart paint events
        pnlCollectionsChart.Paint += PnlCollectionsChart_Paint;
        pnlLoanStatusChart.Paint += PnlLoanStatusChart_Paint;
        pnlCustomerClassChart.Paint += PnlCustomerClassChart_Paint;
    }

    private void DashboardControl_Disposed(object? sender, EventArgs e)
    {
        _cts.Cancel();
        _cts.Dispose();
        _loadLock.Dispose();
    }

    private async void DashboardControl_Load(object sender, EventArgs e)
    {
        StyleDataGridViews();
        await LoadDashboardDataAsync();
    }

    private void DashboardControl_Resize(object? sender, EventArgs e)
    {
        // Adjust layout on resize
        int availableWidth = Width - 20;
        int halfWidth = (availableWidth - 10) / 2;

        pnlRecentSection.Width = halfWidth;
        pnlOverdueSection.Left = halfWidth + 10;
        pnlOverdueSection.Width = halfWidth;
    }

    private void StyleDataGridViews()
    {
        StyleDataGridView(dgvRecentApplications);
        StyleDataGridView(dgvOverdueAccounts);
    }

    private void StyleDataGridView(DataGridView dgv)
    {
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
    }

    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        await LoadDashboardDataAsync();
    }

    private async Task LoadDashboardDataAsync()
    {
        if (!await _loadLock.WaitAsync(TimeSpan.FromSeconds(5), _cts.Token))
        {
            Debug.WriteLine("Dashboard load skipped - another load operation is in progress");
            return;
        }

        try
        {
            SetLoadingState(true);

            // Load all data in parallel
            var statsTask = LoadStatisticsWithRetryAsync();
            var extendedStatsTask = LoadExtendedStatsAsync();
            var collectionsTask = LoadMonthlyCollectionsAsync();
            var applicationsTask = LoadRecentApplicationsWithRetryAsync();
            var overdueTask = LoadOverdueAccountsWithRetryAsync();

            await Task.WhenAll(statsTask, extendedStatsTask, collectionsTask, applicationsTask, overdueTask);

            // Refresh charts
            RefreshCharts();
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine("Dashboard load was cancelled");
        }
        catch (Exception ex)
        {
            HandleCriticalError(ex);
        }
        finally
        {
            SetLoadingState(false);
            _loadLock.Release();
        }
    }

    private async Task LoadStatisticsWithRetryAsync()
    {
        for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
        {
            try
            {
                var stats = await _reportManager.GetDashboardStatsAsync();
                UpdateUI(() => UpdateStatisticsDisplay(stats));
                return;
            }
            catch (Exception ex) when (attempt < MaxRetryAttempts)
            {
                Debug.WriteLine($"Statistics load attempt {attempt} failed: {ex.Message}");
                await Task.Delay(RetryDelayMs * attempt, _cts.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Statistics load final attempt failed: {ex.Message}");
            }
        }
        UpdateUI(() => UpdateStatisticsDisplay(new DashboardStats()));
    }

    private async Task LoadExtendedStatsAsync()
    {
        try
        {
            _extendedStats = await _reportManager.GetExtendedDashboardStatsAsync();
            UpdateUI(() => UpdateQuickStats());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Extended stats load failed: {ex.Message}");
            _extendedStats = new ExtendedDashboardStats();
        }
    }

    private async Task LoadMonthlyCollectionsAsync()
    {
        try
        {
            _monthlyCollections = await _reportManager.GetMonthlyCollectionsAsync(6);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Monthly collections load failed: {ex.Message}");
            _monthlyCollections = [];
        }
    }

    private void UpdateStatisticsDisplay(DashboardStats stats)
    {
        lblActiveLoansValue.Text = stats.TotalActiveLoans.ToString("N0");
        lblOutstandingValue.Text = $"P {stats.TotalOutstandingBalance:N0}";
        lblTodayCollectionsValue.Text = $"P {stats.TodaysCollections:N0}";
        lblOverdueValue.Text = stats.OverdueAccounts.ToString("N0");
        lblPendingValue.Text = stats.PendingApplications.ToString("N0");
        lblCustomersValue.Text = stats.TotalCustomers.ToString("N0");
        lblBlacklistedValue.Text = stats.BlacklistedCustomers.ToString("N0");
    }

    private void UpdateQuickStats()
    {
        if (_extendedStats == null) return;

        lblWeeklyCollections.Text = $"Weekly: P {_extendedStats.WeeklyCollections:N2}";
        lblMonthlyCollections.Text = $"Monthly: P {_extendedStats.MonthlyCollections:N2}";
        lblNewCustomers.Text = $"New Customers: {_extendedStats.NewCustomersThisMonth}";
        lblLoansThisMonth.Text = $"Loans This Month: {_extendedStats.LoansReleasedThisMonth}";
    }

    private void RefreshCharts()
    {
        UpdateUI(() =>
        {
            pnlCollectionsChart.Invalidate();
            pnlLoanStatusChart.Invalidate();
            pnlCustomerClassChart.Invalidate();
        });
    }

    #region Chart Painting

    private void PnlCollectionsChart_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int chartLeft = 15;
        int chartTop = 35;
        int chartWidth = pnlCollectionsChart.Width - 30;
        int chartHeight = pnlCollectionsChart.Height - 60;

        if (chartWidth <= 0 || chartHeight <= 0) return;

        // Draw "No Data" if empty
        if (_monthlyCollections.Count == 0)
        {
            DrawNoDataMessage(g, pnlCollectionsChart);
            return;
        }

        decimal maxValue = _monthlyCollections.Max(m => m.TotalCollections);
        if (maxValue == 0) maxValue = 1;

        int barCount = _monthlyCollections.Count;
        int spacing = 8;
        int barWidth = Math.Max(25, (chartWidth - (spacing * (barCount + 1))) / barCount);

        // Draw bars
        int x = chartLeft + spacing;
        foreach (var data in _monthlyCollections)
        {
            int barHeight = (int)((double)data.TotalCollections / (double)maxValue * chartHeight);
            barHeight = Math.Max(3, barHeight);

            var rect = new Rectangle(x, chartTop + chartHeight - barHeight, barWidth, barHeight);

            // Draw bar with gradient
            if (rect.Height > 0 && rect.Width > 0)
            {
                using var brush = new LinearGradientBrush(
                    new Rectangle(rect.X, rect.Y, rect.Width, Math.Max(1, rect.Height)),
                    Color.FromArgb(52, 152, 219),
                    Color.FromArgb(41, 128, 185),
                    LinearGradientMode.Vertical);
                g.FillRectangle(brush, rect);
            }

            // Draw label
            using var font = new Font("Segoe UI", 7F);
            string label = data.Month.Length >= 3 ? data.Month[..3] : data.Month;
            var labelSize = g.MeasureString(label, font);
            g.DrawString(label, font, Brushes.Gray,
                x + (barWidth - labelSize.Width) / 2, chartTop + chartHeight + 5);

            x += barWidth + spacing;
        }

        // Draw axis lines
        using var axisPen = new Pen(Color.FromArgb(200, 200, 200), 1);
        g.DrawLine(axisPen, chartLeft, chartTop + chartHeight, chartLeft + chartWidth, chartTop + chartHeight);
    }

    private void PnlLoanStatusChart_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Draw "No Data" if no stats
        if (_extendedStats == null)
        {
            DrawNoDataMessage(g, pnlLoanStatusChart);
            return;
        }

        int total = _extendedStats.ActiveLoansCount + _extendedStats.FullyPaidLoansCount + _extendedStats.DefaultedLoansCount;

        var data = new[]
        {
            (Label: "Active", Count: _extendedStats.ActiveLoansCount, Color: Color.FromArgb(52, 152, 219)),
            (Label: "Paid", Count: _extendedStats.FullyPaidLoansCount, Color: Color.FromArgb(46, 204, 113)),
            (Label: "Default", Count: _extendedStats.DefaultedLoansCount, Color: Color.FromArgb(231, 76, 60))
        };

        int centerX = pnlLoanStatusChart.Width / 2;
        int centerY = 75;
        int radius = 40;

        if (total == 0)
        {
            // Draw empty pie chart placeholder
            using var emptyBrush = new SolidBrush(Color.FromArgb(230, 230, 230));
            g.FillEllipse(emptyBrush, centerX - radius, centerY - radius, radius * 2, radius * 2);
            
            using var font = new Font("Segoe UI", 8F);
            var text = "No Data";
            var textSize = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Gray, 
                centerX - textSize.Width / 2, centerY - textSize.Height / 2);
        }
        else
        {
            // Draw pie chart
            float startAngle = -90;
            foreach (var item in data.Where(d => d.Count > 0))
            {
                float sweepAngle = (float)item.Count / total * 360;
                using var brush = new SolidBrush(item.Color);
                g.FillPie(brush, centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                startAngle += sweepAngle;
            }
        }

        // Draw legend (always show)
        DrawLegend(g, data, pnlLoanStatusChart.Height - 25);
    }

    private void PnlCustomerClassChart_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Draw "No Data" if no stats
        if (_extendedStats == null)
        {
            DrawNoDataMessage(g, pnlCustomerClassChart);
            return;
        }

        int total = _extendedStats.RegularCustomers + _extendedStats.VipCustomers + _extendedStats.BlacklistedCustomers;

        var data = new[]
        {
            (Label: "Regular", Count: _extendedStats.RegularCustomers, Color: Color.FromArgb(52, 152, 219)),
            (Label: "VIP", Count: _extendedStats.VipCustomers, Color: Color.FromArgb(241, 196, 15)),
            (Label: "Blacklist", Count: _extendedStats.BlacklistedCustomers, Color: Color.FromArgb(231, 76, 60))
        };

        int centerX = pnlCustomerClassChart.Width / 2;
        int centerY = 75;
        int radius = 40;

        if (total == 0)
        {
            // Draw empty pie chart placeholder
            using var emptyBrush = new SolidBrush(Color.FromArgb(230, 230, 230));
            g.FillEllipse(emptyBrush, centerX - radius, centerY - radius, radius * 2, radius * 2);
            
            using var font = new Font("Segoe UI", 8F);
            var text = "No Data";
            var textSize = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Gray, 
                centerX - textSize.Width / 2, centerY - textSize.Height / 2);
        }
        else
        {
            // Draw pie chart
            float startAngle = -90;
            foreach (var item in data.Where(d => d.Count > 0))
            {
                float sweepAngle = (float)item.Count / total * 360;
                using var brush = new SolidBrush(item.Color);
                g.FillPie(brush, centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                startAngle += sweepAngle;
            }
        }

        
        DrawLegend(g, data, pnlCustomerClassChart.Height - 25);
    }

    private static void DrawNoDataMessage(Graphics g, Panel panel)
    {
        using var font = new Font("Segoe UI", 9F);
        var text = "No data available";
        var textSize = g.MeasureString(text, font);
        g.DrawString(text, font, Brushes.Gray,
            (panel.Width - textSize.Width) / 2,
            (panel.Height - textSize.Height) / 2);
    }

    private static void DrawLegend(Graphics g, (string Label, int Count, Color Color)[] data, int legendY)
    {
        using var font = new Font("Segoe UI", 7F);
        int legendX = 10;
        int itemWidth = 70;

        foreach (var item in data)
        {
            using var brush = new SolidBrush(item.Color);
            g.FillRectangle(brush, legendX, legendY, 10, 10);
            g.DrawString($"{item.Label}: {item.Count}", font, Brushes.DarkGray, legendX + 13, legendY - 2);
            legendX += itemWidth;
        }
    }

    #endregion

    private async Task LoadRecentApplicationsWithRetryAsync()
    {
        for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
        {
            try
            {
                var applicationManager = new LoanApplicationManager(_context);
                var applications = (await applicationManager.GetAllAsync())
                    .OrderByDescending(a => a.ApplicationDate)
                    .Take(10)
                    .Select(a => new
                    {
                        Application = a.ApplicationNumber ?? "N/A",
                        Customer = a.Customer?.FullName ?? "N/A",
                        Amount = $"P {a.RequestedAmount:N0}",
                        Status = a.Status.ToString(),
                        Date = a.ApplicationDate.ToString("MM/dd/yy")
                    })
                    .ToList();

                UpdateUI(() => dgvRecentApplications.DataSource = applications);
                return;
            }
            catch (Exception ex) when (attempt < MaxRetryAttempts)
            {
                Debug.WriteLine($"Recent applications load attempt {attempt} failed: {ex.Message}");
                await Task.Delay(RetryDelayMs * attempt, _cts.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Recent applications load final attempt failed: {ex.Message}");
            }
        }
        UpdateUI(() => dgvRecentApplications.DataSource = null);
    }

    private async Task LoadOverdueAccountsWithRetryAsync()
    {
        for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
        {
            try
            {
                var overdueAccounts = await _reportManager.GetOverdueAccountsReportAsync();
                var data = overdueAccounts
                    .Take(10)
                    .Select(o => new
                    {
                        Loan = o.LoanNumber ?? "N/A",
                        Customer = o.CustomerName ?? "N/A",
                        Overdue = $"P {o.TotalOverdueAmount:N0}",
                        Days = o.DaysOverdue,
                        Installments = o.OverdueInstallments
                    })
                    .ToList();

                UpdateUI(() => dgvOverdueAccounts.DataSource = data);
                return;
            }
            catch (Exception ex) when (attempt < MaxRetryAttempts)
            {
                Debug.WriteLine($"Overdue accounts load attempt {attempt} failed: {ex.Message}");
                await Task.Delay(RetryDelayMs * attempt, _cts.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Overdue accounts load final attempt failed: {ex.Message}");
            }
        }
        UpdateUI(() => dgvOverdueAccounts.DataSource = null);
    }

    private void UpdateUI(Action action)
    {
        if (InvokeRequired)
            Invoke(action);
        else
            action();
    }

    private void SetLoadingState(bool isLoading)
    {
        UpdateUI(() =>
        {
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
            btnRefresh.Enabled = !isLoading;
            btnRefresh.Text = isLoading ? "Loading..." : "Refresh";
        });
    }

    private void HandleCriticalError(Exception ex)
    {
        Debug.WriteLine($"Critical error loading dashboard: {ex}");

        var message = ex switch
        {
            InvalidOperationException => "Database connection error. Please check your connection and try again.",
            TimeoutException => "The operation timed out. Please try again.",
            _ => $"An unexpected error occurred: {ex.Message}"
        };

        UpdateUI(() => MessageBox.Show(message, "Dashboard Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
    }

    public async Task RefreshAsync()
    {
        await LoadDashboardDataAsync();
    }
}
