using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public class MainForm : Form
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlMain;
        private Button btnReportIssues;
        private Button btnEvents;
        private Button btnServiceStatus;
        private Button btnViewReports;
        private Panel pnlFooter;
        private Label lblFooter;

        public MainForm()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            this.Text = "Municipality Citizen Services";
            this.Size = new Size(900, 650);
            this.MinimumSize = new Size(900, 650);
            this.BackColor = ThemeManager.BackgroundColor;

            // pnlHeader
            this.pnlHeader = new Panel();
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 120;
            this.pnlHeader.BackColor = ThemeManager.PrimaryColor;

            this.pnlHeader.Paint += (sender, e) =>
            {
                Rectangle rect = new Rectangle(0, 0, pnlHeader.Width, pnlHeader.Height);
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    ThemeManager.PrimaryColor,
                    ThemeManager.SecondaryColor,
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            };

            this.lblTitle = new Label();
            this.lblTitle.Text = "🏛️ Municipality Citizen Services";
            this.lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(30, 25);
            this.lblTitle.Size = new Size(600, 50);
            this.lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.lblSubtitle = new Label();
            this.lblSubtitle.Text = "Your voice matters - Report issues and help improve our community";
            this.lblSubtitle.Font = new Font("Segoe UI", 12F);
            this.lblSubtitle.ForeColor = Color.FromArgb(200, 220, 240);
            this.lblSubtitle.Location = new Point(35, 75);
            this.lblSubtitle.Size = new Size(600, 30);
            this.lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;

            // pnlMain
            this.pnlMain = new Panel();
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.BackColor = ThemeManager.BackgroundColor;
            this.pnlMain.Padding = new Padding(40);

            // btnReportIssues
            this.btnReportIssues = new Button();
            this.btnReportIssues.Text = "📝 Report Issues";
            this.btnReportIssues.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.btnReportIssues.Size = new Size(350, 90);
            this.btnReportIssues.Location = new Point(75, 30);
            this.btnReportIssues.BackColor = ThemeManager.SecondaryColor;
            this.btnReportIssues.ForeColor = Color.White;
            this.btnReportIssues.FlatStyle = FlatStyle.Flat;
            this.btnReportIssues.FlatAppearance.BorderSize = 0;
            this.btnReportIssues.Cursor = Cursors.Hand;
            this.btnReportIssues.Click += new EventHandler(this.BtnReportIssues_Click);

            // btnEvents - NOW ENABLED
            this.btnEvents = new Button();
            this.btnEvents.Text = "📅 Local Events & Announcements";
            this.btnEvents.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnEvents.Size = new Size(350, 80);
            this.btnEvents.Location = new Point(475, 40);
            this.btnEvents.BackColor = ThemeManager.AccentColor;
            this.btnEvents.ForeColor = Color.White;
            this.btnEvents.FlatStyle = FlatStyle.Flat;
            this.btnEvents.FlatAppearance.BorderSize = 0;
            this.btnEvents.Cursor = Cursors.Hand;
            this.btnEvents.Click += new EventHandler(this.BtnEvents_Click);

            // btnViewReports
            this.btnViewReports = new Button();
            this.btnViewReports.Text = "📋 View All Reports";
            this.btnViewReports.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnViewReports.Size = new Size(350, 80);
            this.btnViewReports.Location = new Point(75, 150);
            this.btnViewReports.BackColor = Color.White;
            this.btnViewReports.ForeColor = ThemeManager.TextPrimary;
            this.btnViewReports.FlatStyle = FlatStyle.Flat;
            this.btnViewReports.FlatAppearance.BorderColor = ThemeManager.SecondaryColor;
            this.btnViewReports.FlatAppearance.BorderSize = 2;
            this.btnViewReports.Cursor = Cursors.Hand;
            this.btnViewReports.Click += new EventHandler(this.BtnViewReports_Click);

            // btnServiceStatus
            this.btnServiceStatus = new Button();
            this.btnServiceStatus.Text = "🔍 Service Request Status";
            this.btnServiceStatus.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnServiceStatus.Size = new Size(350, 80);
            this.btnServiceStatus.Location = new Point(475, 150);
            this.btnServiceStatus.BackColor = Color.FromArgb(200, 200, 200);
            this.btnServiceStatus.ForeColor = Color.Gray;
            this.btnServiceStatus.FlatStyle = FlatStyle.Flat;
            this.btnServiceStatus.FlatAppearance.BorderSize = 0;
            this.btnServiceStatus.Enabled = false;

            // pnlFooter
            this.pnlFooter = new Panel();
            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Height = 40;
            this.pnlFooter.BackColor = ThemeManager.PrimaryColor;

            this.lblFooter = new Label();
            this.lblFooter.Text = "© 2026 Municipality Services | All Rights Reserved | v2.0";
            this.lblFooter.Font = new Font("Segoe UI", 9F);
            this.lblFooter.ForeColor = Color.FromArgb(200, 220, 240);
            this.lblFooter.Dock = DockStyle.Fill;
            this.lblFooter.TextAlign = ContentAlignment.MiddleCenter;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlMain.Controls.Add(this.btnReportIssues);
            this.pnlMain.Controls.Add(this.btnEvents);
            this.pnlMain.Controls.Add(this.btnViewReports);
            this.pnlMain.Controls.Add(this.btnServiceStatus);
            this.pnlFooter.Controls.Add(this.lblFooter);

            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
        }

        private void BtnReportIssues_Click(object sender, EventArgs e)
        {
            ReportIssueForm reportForm = new ReportIssueForm();
            reportForm.ShowDialog();
        }

        private void BtnEvents_Click(object sender, EventArgs e)
        {
            EventsForm eventsForm = new EventsForm();
            eventsForm.ShowDialog();
        }

        private void BtnViewReports_Click(object sender, EventArgs e)
        {
            ViewReportsForm viewForm = new ViewReportsForm();
            viewForm.ShowDialog();
        }
    }
}