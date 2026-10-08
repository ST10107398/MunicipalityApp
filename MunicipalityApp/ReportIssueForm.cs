using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public class ReportIssueForm : Form
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlContent;
        private Panel pnlLocation;
        private Label lblLocation;
        private TextBox txtLocation;
        private Panel pnlCategory;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Panel pnlDescription;
        private Label lblDescription;
        private RichTextBox rtbDescription;
        private Panel pnlAttachment;
        private Button btnAttach;
        private Label lblAttachment;
        private Panel pnlEngagement;
        private Label lblEngagement;
        private ProgressBar prbEngagement;
        private Label lblProgressPercent;
        private Label lblProgressStatus;
        private Panel pnlButtons;
        private Button btnSubmit;
        private Button btnBack;
        private OpenFileDialog openFileDialog1;

        private string attachmentPath = string.Empty;
        private List<string> engagementMessages;
        private Random random = new Random();

        // Real-time tracking
        private int currentProgress = 0;
        private const string LOCATION_PLACEHOLDER = "Enter the location (e.g., 123 Main St, Johannesburg)";

        // Progress weights for each field (total = 100)
        private const int LOCATION_WEIGHT = 30;
        private const int CATEGORY_WEIGHT = 15;  // Auto-filled, so small weight
        private const int DESCRIPTION_WEIGHT = 40;
        private const int ATTACHMENT_WEIGHT = 15;

        public ReportIssueForm()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(700, 750);
            this.MaximumSize = new Size(700, 800);

            engagementMessages = new List<string>
            {
                "🌟 Thank you for helping improve our community!",
                "💪 Your voice matters! Every report makes a difference.",
                "🏙️ Together we can make our city better!",
                "✨ You're making a positive impact on your neighborhood!",
                "🤝 Community engagement starts with you!",
                "⭐ Great job taking action!",
                "🌱 Your report helps us grow and improve!"
            };

            // Initialize progress tracking
            UpdateProgress();
        }

        private void InitializeComponent()
        {
            this.Text = "Report an Issue";
            this.Size = new Size(700, 780);
            this.BackColor = ThemeManager.BackgroundColor;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.openFileDialog1 = new OpenFileDialog();
            this.openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Document Files|*.pdf;*.doc;*.docx;*.txt|All Files|*.*";
            this.openFileDialog1.Title = "Select an image or document";

            // pnlHeader
            this.pnlHeader = new Panel();
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 80;
            this.pnlHeader.BackColor = ThemeManager.SecondaryColor;

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
            this.lblTitle.Text = "📝 Report an Issue";
            this.lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(30, 15);
            this.lblTitle.Size = new Size(400, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.lblSubtitle = new Label();
            this.lblSubtitle.Text = "Help us improve our community by reporting issues";
            this.lblSubtitle.Font = new Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = Color.FromArgb(200, 220, 240);
            this.lblSubtitle.Location = new Point(35, 55);
            this.lblSubtitle.Size = new Size(500, 25);
            this.lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // pnlContent
            this.pnlContent = new Panel();
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeManager.BackgroundColor;
            this.pnlContent.Padding = new Padding(30);
            this.pnlContent.AutoScroll = true;

            // ===== LOCATION FIELD =====
            this.pnlLocation = new Panel();
            this.pnlLocation.Size = new Size(620, 60);
            this.pnlLocation.Location = new Point(10, 10);
            this.pnlLocation.BackColor = Color.White;
            this.pnlLocation.Padding = new Padding(15);

            this.lblLocation = new Label();
            this.lblLocation.Text = "📍 Location *";
            this.lblLocation.Font = ThemeManager.LabelFont;
            this.lblLocation.ForeColor = ThemeManager.TextPrimary;
            this.lblLocation.Location = new Point(15, 5);
            this.lblLocation.Size = new Size(200, 20);

            this.txtLocation = new TextBox();
            this.txtLocation.Font = new Font("Segoe UI", 12F);
            this.txtLocation.Location = new Point(15, 25);
            this.txtLocation.Size = new Size(580, 30);
            this.txtLocation.Text = LOCATION_PLACEHOLDER;
            this.txtLocation.ForeColor = Color.Gray;
            this.txtLocation.BackColor = Color.White;
            this.txtLocation.BorderStyle = BorderStyle.None;
            this.txtLocation.Enter += new EventHandler(this.TxtLocation_Enter);
            this.txtLocation.Leave += new EventHandler(this.TxtLocation_Leave);
            // Real-time tracking: fires on every keystroke
            this.txtLocation.TextChanged += new EventHandler(this.Field_TextChanged);

            this.pnlLocation.Controls.Add(this.lblLocation);
            this.pnlLocation.Controls.Add(this.txtLocation);

            // ===== CATEGORY FIELD =====
            this.pnlCategory = new Panel();
            this.pnlCategory.Size = new Size(620, 60);
            this.pnlCategory.Location = new Point(10, 80);
            this.pnlCategory.BackColor = Color.White;
            this.pnlCategory.Padding = new Padding(15);

            this.lblCategory = new Label();
            this.lblCategory.Text = "📂 Category *";
            this.lblCategory.Font = ThemeManager.LabelFont;
            this.lblCategory.ForeColor = ThemeManager.TextPrimary;
            this.lblCategory.Location = new Point(15, 5);
            this.lblCategory.Size = new Size(200, 20);

            this.cmbCategory = new ComboBox();
            this.cmbCategory.Font = new Font("Segoe UI", 12F);
            this.cmbCategory.Location = new Point(15, 25);
            this.cmbCategory.Size = new Size(580, 30);
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.BackColor = Color.White;
            this.cmbCategory.FlatStyle = FlatStyle.Flat;
            this.cmbCategory.Items.AddRange(new object[] {
                "-- Select Category --",
                "Sanitation - Garbage Collection",
                "Sanitation - Recycling",
                "Roads - Potholes",
                "Roads - Street Lighting",
                "Roads - Traffic Signals",
                "Utilities - Water Supply",
                "Utilities - Electricity",
                "Utilities - Sewerage",
                "Parks & Recreation",
                "Public Safety",
                "Noise Complaints",
                "Graffiti/Vandalism",
                "Other"
            });
            this.cmbCategory.SelectedIndex = 0;
            // Real-time tracking
            this.cmbCategory.SelectedIndexChanged += new EventHandler(this.Field_TextChanged);

            this.pnlCategory.Controls.Add(this.lblCategory);
            this.pnlCategory.Controls.Add(this.cmbCategory);

            // ===== DESCRIPTION FIELD =====
            this.pnlDescription = new Panel();
            this.pnlDescription.Size = new Size(620, 160);
            this.pnlDescription.Location = new Point(10, 150);
            this.pnlDescription.BackColor = Color.White;
            this.pnlDescription.Padding = new Padding(15);

            this.lblDescription = new Label();
            this.lblDescription.Text = "📝 Description * (min 20 characters)";
            this.lblDescription.Font = ThemeManager.LabelFont;
            this.lblDescription.ForeColor = ThemeManager.TextPrimary;
            this.lblDescription.Location = new Point(15, 5);
            this.lblDescription.Size = new Size(300, 20);

            this.rtbDescription = new RichTextBox();
            this.rtbDescription.Font = new Font("Segoe UI", 12F);
            this.rtbDescription.Location = new Point(15, 28);
            this.rtbDescription.Size = new Size(580, 115);
            this.rtbDescription.BorderStyle = BorderStyle.None;
            this.rtbDescription.BackColor = Color.White;
            // Real-time tracking
            this.rtbDescription.TextChanged += new EventHandler(this.Field_TextChanged);

            this.pnlDescription.Controls.Add(this.lblDescription);
            this.pnlDescription.Controls.Add(this.rtbDescription);

            // ===== ATTACHMENT FIELD =====
            this.pnlAttachment = new Panel();
            this.pnlAttachment.Size = new Size(620, 55);
            this.pnlAttachment.Location = new Point(10, 320);
            this.pnlAttachment.BackColor = Color.White;
            this.pnlAttachment.Padding = new Padding(15);

            this.btnAttach = new Button();
            this.btnAttach.Text = "📎 Attach File";
            this.btnAttach.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnAttach.Size = new Size(140, 35);
            this.btnAttach.Location = new Point(15, 10);
            this.btnAttach.BackColor = ThemeManager.BackgroundColor;
            this.btnAttach.ForeColor = ThemeManager.TextPrimary;
            this.btnAttach.FlatStyle = FlatStyle.Flat;
            this.btnAttach.FlatAppearance.BorderColor = ThemeManager.SecondaryColor;
            this.btnAttach.FlatAppearance.BorderSize = 1;
            this.btnAttach.Cursor = Cursors.Hand;
            this.btnAttach.Click += new EventHandler(this.BtnAttach_Click);

            this.lblAttachment = new Label();
            this.lblAttachment.Text = "Optional - No file attached";
            this.lblAttachment.Font = new Font("Segoe UI", 10F);
            this.lblAttachment.ForeColor = Color.Gray;
            this.lblAttachment.Location = new Point(170, 15);
            this.lblAttachment.Size = new Size(430, 25);
            this.lblAttachment.TextAlign = ContentAlignment.MiddleLeft;

            this.pnlAttachment.Controls.Add(this.btnAttach);
            this.pnlAttachment.Controls.Add(this.lblAttachment);

            // ===== ENGAGEMENT / PROGRESS SECTION =====
            this.pnlEngagement = new Panel();
            this.pnlEngagement.Size = new Size(620, 130);
            this.pnlEngagement.Location = new Point(10, 390);
            this.pnlEngagement.BackColor = Color.FromArgb(240, 248, 255);
            this.pnlEngagement.Padding = new Padding(20);

            this.lblEngagement = new Label();
            this.lblEngagement.Text = "🌟 Start filling the form to see your progress!";
            this.lblEngagement.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            this.lblEngagement.ForeColor = ThemeManager.SecondaryColor;
            this.lblEngagement.Location = new Point(15, 10);
            this.lblEngagement.Size = new Size(580, 30);
            this.lblEngagement.TextAlign = ContentAlignment.MiddleCenter;

            // Progress status label - shows what step user is on
            this.lblProgressStatus = new Label();
            this.lblProgressStatus.Text = "Progress: 0% - Start by entering a location";
            this.lblProgressStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblProgressStatus.ForeColor = ThemeManager.TextPrimary;
            this.lblProgressStatus.Location = new Point(15, 42);
            this.lblProgressStatus.Size = new Size(500, 20);
            this.lblProgressStatus.TextAlign = ContentAlignment.MiddleLeft;

            // Progress percentage label
            this.lblProgressPercent = new Label();
            this.lblProgressPercent.Text = "0%";
            this.lblProgressPercent.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblProgressPercent.ForeColor = ThemeManager.SecondaryColor;
            this.lblProgressPercent.Location = new Point(520, 40);
            this.lblProgressPercent.Size = new Size(80, 25);
            this.lblProgressPercent.TextAlign = ContentAlignment.MiddleRight;

            this.prbEngagement = new ProgressBar();
            this.prbEngagement.Location = new Point(15, 70);
            this.prbEngagement.Size = new Size(580, 30);
            this.prbEngagement.Style = ProgressBarStyle.Continuous;
            this.prbEngagement.Minimum = 0;
            this.prbEngagement.Maximum = 100;
            this.prbEngagement.Value = 0;
            this.prbEngagement.BackColor = Color.White;

            this.pnlEngagement.Controls.Add(this.lblEngagement);
            this.pnlEngagement.Controls.Add(this.lblProgressStatus);
            this.pnlEngagement.Controls.Add(this.lblProgressPercent);
            this.pnlEngagement.Controls.Add(this.prbEngagement);

            // ===== BUTTONS =====
            this.pnlButtons = new Panel();
            this.pnlButtons.Size = new Size(620, 80);
            this.pnlButtons.Location = new Point(10, 530);
            this.pnlButtons.BackColor = Color.Transparent;

            this.btnSubmit = new Button();
            this.btnSubmit.Text = "✅ Submit Report";
            this.btnSubmit.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnSubmit.Size = new Size(220, 50);
            this.btnSubmit.Location = new Point(200, 15);
            this.btnSubmit.BackColor = Color.FromArgb(200, 200, 200); // Disabled look
            this.btnSubmit.ForeColor = Color.Gray;
            this.btnSubmit.FlatStyle = FlatStyle.Flat;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.Cursor = Cursors.Default;
            this.btnSubmit.Enabled = false; // Disabled until form is complete
            this.btnSubmit.Click += new EventHandler(this.BtnSubmit_Click);

            this.btnBack = new Button();
            this.btnBack.Text = "← Back to Main Menu";
            this.btnBack.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnBack.Size = new Size(180, 40);
            this.btnBack.Location = new Point(10, 20);
            this.btnBack.BackColor = Color.White;
            this.btnBack.ForeColor = ThemeManager.TextPrimary;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            this.btnBack.FlatAppearance.BorderSize = 1;
            this.btnBack.Cursor = Cursors.Hand;
            this.btnBack.Click += new EventHandler(this.BtnBack_Click);

            this.pnlButtons.Controls.Add(this.btnSubmit);
            this.pnlButtons.Controls.Add(this.btnBack);

            // Add all panels to content
            this.pnlContent.Controls.Add(this.pnlLocation);
            this.pnlContent.Controls.Add(this.pnlCategory);
            this.pnlContent.Controls.Add(this.pnlDescription);
            this.pnlContent.Controls.Add(this.pnlAttachment);
            this.pnlContent.Controls.Add(this.pnlEngagement);
            this.pnlContent.Controls.Add(this.pnlButtons);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
        }

        // ==================== REAL-TIME PROGRESS TRACKING ====================

        private void Field_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            int newProgress = 0;
            List<string> completedSteps = new List<string>();
            List<string> pendingSteps = new List<string>();

            // Check Location (30%)
            string location = txtLocation.Text.Trim();
            if (!string.IsNullOrWhiteSpace(location) && location != LOCATION_PLACEHOLDER)
            {
                newProgress += LOCATION_WEIGHT;
                completedSteps.Add("Location");
            }
            else
            {
                pendingSteps.Add("Location");
            }

            // Check Category (15%) - must not be "-- Select Category --"
            if (cmbCategory.SelectedIndex > 0)
            {
                newProgress += CATEGORY_WEIGHT;
                completedSteps.Add("Category");
            }
            else
            {
                pendingSteps.Add("Category");
            }

            // Check Description (40%) - minimum 20 characters
            int descLength = rtbDescription.Text.Trim().Length;
            if (descLength >= 20)
            {
                newProgress += DESCRIPTION_WEIGHT;
                completedSteps.Add("Description");
            }
            else if (descLength > 0)
            {
                // Partial credit for partially filled description
                int partialCredit = (int)((descLength / 20.0) * DESCRIPTION_WEIGHT);
                newProgress += partialCredit;
                pendingSteps.Add($"Description ({descLength}/20 chars)");
            }
            else
            {
                pendingSteps.Add("Description");
            }

            // Check Attachment (15%) - optional but gives progress
            if (!string.IsNullOrEmpty(attachmentPath))
            {
                newProgress += ATTACHMENT_WEIGHT;
                completedSteps.Add("Attachment");
            }

            currentProgress = Math.Min(newProgress, 100);

            // Update UI
            if (prbEngagement != null)
            {
                prbEngagement.Value = currentProgress;
            }

            if (lblProgressPercent != null)
            {
                lblProgressPercent.Text = $"{currentProgress}%";

                // Change color based on progress
                if (currentProgress >= 85)
                    lblProgressPercent.ForeColor = ThemeManager.SuccessColor;
                else if (currentProgress >= 50)
                    lblProgressPercent.ForeColor = ThemeManager.SecondaryColor;
                else
                    lblProgressPercent.ForeColor = ThemeManager.WarningColor;
            }

            // Update status text
            if (lblProgressStatus != null)
            {
                if (currentProgress >= 85)
                {
                    lblProgressStatus.Text = "✅ Ready to submit! You can add an attachment (optional) or submit now.";
                    lblProgressStatus.ForeColor = ThemeManager.SuccessColor;
                }
                else if (currentProgress >= 50)
                {
                    lblProgressStatus.Text = $"Progress: {currentProgress}% - Almost there! Complete: {string.Join(", ", pendingSteps)}";
                    lblProgressStatus.ForeColor = ThemeManager.SecondaryColor;
                }
                else if (currentProgress > 0)
                {
                    lblProgressStatus.Text = $"Progress: {currentProgress}% - Still needed: {string.Join(", ", pendingSteps)}";
                    lblProgressStatus.ForeColor = ThemeManager.TextPrimary;
                }
                else
                {
                    lblProgressStatus.Text = "Progress: 0% - Start by entering a location";
                    lblProgressStatus.ForeColor = ThemeManager.TextPrimary;
                }
            }

            // Update engagement message with encouragement
            if (lblEngagement != null)
            {
                if (currentProgress == 100)
                {
                    lblEngagement.Text = "🎉 Excellent! Your report is complete and ready to submit!";
                    lblEngagement.ForeColor = ThemeManager.SuccessColor;
                }
                else if (currentProgress >= 85)
                {
                    lblEngagement.Text = "🌟 Almost done! Your report is looking great!";
                    lblEngagement.ForeColor = ThemeManager.SuccessColor;
                }
                else if (currentProgress >= 50)
                {
                    lblEngagement.Text = engagementMessages[random.Next(engagementMessages.Count)];
                    lblEngagement.ForeColor = ThemeManager.SecondaryColor;
                }
                else if (currentProgress > 0)
                {
                    lblEngagement.Text = "💪 Good start! Keep going to complete your report.";
                    lblEngagement.ForeColor = ThemeManager.SecondaryColor;
                }
                else
                {
                    lblEngagement.Text = "🌟 Start filling the form to see your progress!";
                    lblEngagement.ForeColor = ThemeManager.SecondaryColor;
                }
            }

            // Enable/disable submit button
            if (btnSubmit != null)
            {
                // Require at least 85% (location + category + description)
                bool canSubmit = currentProgress >= 85;
                btnSubmit.Enabled = canSubmit;

                if (canSubmit)
                {
                    btnSubmit.BackColor = ThemeManager.SuccessColor;
                    btnSubmit.ForeColor = Color.White;
                    btnSubmit.Cursor = Cursors.Hand;
                    btnSubmit.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96);
                    btnSubmit.FlatAppearance.MouseDownBackColor = Color.FromArgb(33, 150, 83);
                }
                else
                {
                    btnSubmit.BackColor = Color.FromArgb(200, 200, 200);
                    btnSubmit.ForeColor = Color.Gray;
                    btnSubmit.Cursor = Cursors.Default;
                }
            }
        }

        // ==================== EVENT HANDLERS ====================

        private void TxtLocation_Enter(object sender, EventArgs e)
        {
            if (txtLocation.Text == LOCATION_PLACEHOLDER)
            {
                txtLocation.Text = "";
                txtLocation.ForeColor = Color.Black;
            }
        }

        private void TxtLocation_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                txtLocation.Text = LOCATION_PLACEHOLDER;
                txtLocation.ForeColor = Color.Gray;
            }
        }

        private void BtnAttach_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                attachmentPath = openFileDialog1.FileName;
                string fileName = Path.GetFileName(attachmentPath);
                lblAttachment.Text = $"📎 {fileName}";
                lblAttachment.ForeColor = ThemeManager.SuccessColor;
                lblAttachment.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                // Update progress - attachment adds 15%
                UpdateProgress();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string location = txtLocation.Text.Trim();

            // Final validation
            if (string.IsNullOrWhiteSpace(location) || location == LOCATION_PLACEHOLDER)
            {
                MessageBox.Show("⚠️ Please enter a valid location for the issue.",
                    "Location Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex <= 0)
            {
                MessageBox.Show("⚠️ Please select a category for the issue.",
                    "Category Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return;
            }

            if (rtbDescription.Text.Trim().Length < 20)
            {
                MessageBox.Show("⚠️ Please provide a more detailed description (at least 20 characters).",
                    "Description Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return;
            }



            // Create issue object
            var issue = new IssueReport
            {
                Id = IssueReport.NextId++,
                Location = location,
                Category = cmbCategory.SelectedItem.ToString(),
                Description = rtbDescription.Text.Trim(),
                AttachmentPath = attachmentPath,
                ReportDate = DateTime.Now,
                Status = "Submitted",
                ProgressPercent = currentProgress,
                LastUpdated = DateTime.Now,
                StatusNotes = "Report received and pending review"
            };

            Program.IssueReports.Add(issue);

            // Show success message
            string engagementMsg = engagementMessages[random.Next(engagementMessages.Count)];

            MessageBox.Show(
                $"✅ Your report has been submitted successfully!\n\n" +
                $"📋 Report ID: #{issue.Id}\n" +
                $"📍 Location: {issue.Location}\n" +
                $"📂 Category: {issue.Category}\n" +
                $"📊 Completion: {currentProgress}%\n" +
                $"{engagementMsg}\n\n" +
                $"Thank you for helping improve our community!",
                "Report Submitted Successfully",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            ResetForm();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (currentProgress > 0 && currentProgress < 85)
            {
                DialogResult result = MessageBox.Show(
                    "⚠️ You have unsaved changes. Are you sure you want to leave?\n\nYour report will be lost.",
                    "Confirm Exit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                    return;
            }

            this.Close();
        }

        private void ResetForm()
        {
            txtLocation.Text = LOCATION_PLACEHOLDER;
            txtLocation.ForeColor = Color.Gray;
            rtbDescription.Clear();
            cmbCategory.SelectedIndex = 0;
            attachmentPath = string.Empty;
            lblAttachment.Text = "Optional - No file attached";
            lblAttachment.ForeColor = Color.Gray;
            lblAttachment.Font = new Font("Segoe UI", 10F);

            // Reset progress
            UpdateProgress();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Initial progress check
            UpdateProgress();
        }
    }
}