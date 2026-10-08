using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public class ViewReportsForm : Form
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private DataGridView dgvReports;
        private Button btnRefresh;
        private Button btnExport;
        private Button btnBack;
        private Panel pnlStats;
        private Label lblTotal;
        private Label lblSubmitted;
        private Label lblInProgress;
        private Label lblResolved;
        private Panel pnlTotal;
        private Panel pnlSubmitted;
        private Panel pnlInProgress;
        private Panel pnlResolved;

        public ViewReportsForm()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 700);
            LoadReports();
        }

        private void InitializeComponent()
        {
            this.Text = "📋 View All Reports";
            this.BackColor = ThemeManager.BackgroundColor;
            this.MinimumSize = new Size(900, 600);

            // pnlHeader
            this.pnlHeader = new Panel();
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.BackColor = ThemeManager.SecondaryColor;

            this.lblTitle = new Label();
            this.lblTitle.Text = "📋 All Reported Issues";
            this.lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(30, 10);
            this.lblTitle.Size = new Size(400, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.pnlHeader.Controls.Add(this.lblTitle);

            // pnlStats
            this.pnlStats = new Panel();
            this.pnlStats.Dock = DockStyle.Top;
            this.pnlStats.Height = 80;
            this.pnlStats.BackColor = Color.White;
            this.pnlStats.Padding = new Padding(20);

            // Create stat panels
            this.pnlTotal = CreateStatPanel("Total Reports", "0", 20, ThemeManager.PrimaryColor);
            this.pnlSubmitted = CreateStatPanel("Submitted", "0", 240, ThemeManager.WarningColor);
            this.pnlInProgress = CreateStatPanel("In Progress", "0", 460, ThemeManager.SecondaryColor);
            this.pnlResolved = CreateStatPanel("Resolved", "0", 680, ThemeManager.SuccessColor);

            // dgvReports
            this.dgvReports = new DataGridView();
            this.dgvReports.Dock = DockStyle.Fill;
            this.dgvReports.BackgroundColor = Color.White;
            this.dgvReports.BorderStyle = BorderStyle.None;
            this.dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReports.AllowUserToAddRows = false;
            this.dgvReports.AllowUserToDeleteRows = false;
            this.dgvReports.ReadOnly = true;
            this.dgvReports.RowHeadersVisible = false;
            this.dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReports.MultiSelect = false;

            this.dgvReports.EnableHeadersVisualStyles = false;
            this.dgvReports.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.PrimaryColor;
            this.dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvReports.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            this.dgvReports.DefaultCellStyle.Padding = new Padding(5);
            this.dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            // Bottom panel
            Panel pnlBottom = new Panel();
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 60;
            pnlBottom.BackColor = Color.White;
            pnlBottom.Padding = new Padding(20);

            this.btnRefresh = new Button();
            this.btnRefresh.Text = "🔄 Refresh";
            this.btnRefresh.Size = new Size(120, 35);
            this.btnRefresh.Location = new Point(20, 12);
            ThemeManager.StyleButton(this.btnRefresh, true);
            this.btnRefresh.Click += (s, e) => LoadReports();

            this.btnExport = new Button();
            this.btnExport.Text = "📊 Export CSV";
            this.btnExport.Size = new Size(120, 35);
            this.btnExport.Location = new Point(160, 12);
            ThemeManager.StyleButton(this.btnExport, true);
            this.btnExport.Click += new EventHandler(this.BtnExport_Click);

            this.btnBack = new Button();
            this.btnBack.Text = "← Back";
            this.btnBack.Size = new Size(100, 35);
            this.btnBack.Location = new Point(780, 12);
            ThemeManager.StyleButton(this.btnBack, false);
            this.btnBack.Click += (s, e) => this.Close();

            pnlBottom.Controls.Add(this.btnRefresh);
            pnlBottom.Controls.Add(this.btnExport);
            pnlBottom.Controls.Add(this.btnBack);

            // Add to form            this.Controls.Add(this.dgvReports);
            this.Controls.Add(pnlBottom);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
        }

        private Panel CreateStatPanel(string title, string value, int x, Color color)
        {
            Panel panel = new Panel();
            panel.Size = new Size(200, 60);
            panel.Location = new Point(x, 10);
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.None;

            // Add border
            panel.Paint += (sender, e) =>
            {
                Control pnl = sender as Control;
                using (Pen pen = new Pen(color, 3))
                {
                    e.Graphics.DrawLine(pen, 0, 0, pnl.Width, 0);
                }
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
                }
            };

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 9F);
            lblTitle.ForeColor = ThemeManager.TextSecondary;
            lblTitle.Location = new Point(10, 8);
            lblTitle.Size = new Size(180, 20);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblValue.ForeColor = ThemeManager.TextPrimary;
            lblValue.Location = new Point(10, 28);
            lblValue.Size = new Size(180, 28);
            lblValue.TextAlign = ContentAlignment.MiddleCenter;

            // Store references
            if (title == "Total Reports") this.lblTotal = lblValue;
            else if (title == "Submitted") this.lblSubmitted = lblValue;
            else if (title == "In Progress") this.lblInProgress = lblValue;
            else if (title == "Resolved") this.lblResolved = lblValue;

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);
            this.pnlStats.Controls.Add(panel);

            return panel;
        }

        private void LoadReports()
        {
            var reports = Program.IssueReports.Select(r => new
            {
                ID = r.Id,
                Location = r.Location,
                Category = r.Category,
                Description = r.Description.Length > 50 ? r.Description.Substring(0, 50) + "..." : r.Description,
                Status = r.Status,
                Date = r.ReportDate.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            dgvReports.DataSource = reports;
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            int total = Program.IssueReports.Count;
            int submitted = Program.IssueReports.Count(r => r.Status == "Submitted");
            int inProgress = Program.IssueReports.Count(r => r.Status == "In Progress");
            int resolved = Program.IssueReports.Count(r => r.Status == "Resolved");

            if (lblTotal != null) lblTotal.Text = total.ToString();
            if (lblSubmitted != null) lblSubmitted.Text = submitted.ToString();
            if (lblInProgress != null) lblInProgress.Text = inProgress.ToString();
            if (lblResolved != null) lblResolved.Text = resolved.ToString();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files|*.csv";
                sfd.Title = "Export Reports to CSV";
                sfd.FileName = $"Municipality_Reports_{DateTime.Now:yyyyMMdd}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(sfd.FileName);
                    MessageBox.Show($"✅ Reports exported successfully!\n\nFile saved to:\n{sfd.FileName}",
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportToCSV(string filePath)
        {
            var lines = new List<string>();
            lines.Add("ID,Location,Category,Description,Status,Date,Attachment");

            foreach (var report in Program.IssueReports)
            {
                lines.Add($"{report.Id},{report.Location},{report.Category}," +
                         $"{report.Description.Replace(",", ";")},{report.Status}," +
                         $"{report.ReportDate},{report.AttachmentPath}");
            }

            System.IO.File.WriteAllLines(filePath, lines);
        }
    }
}