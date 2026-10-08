using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public class EventsForm : Form
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;

        // Search panel
        private Panel pnlSearch;
        private TextBox txtSearch;
        private ComboBox cmbCategory;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private CheckBox chkUseDateFilter;
        private Button btnSearch;
        private Button btnClear;

        // Main content split
        private SplitContainer splitContainer;

        // Events list
        private FlowLayoutPanel flowEvents;
        private Label lblResultsCount;

        // Recommendations panel
        private Panel pnlRecommendations;
        private Label lblRecommendationsTitle;
        private FlowLayoutPanel flowRecommendations;

        // Bottom buttons
        private Panel pnlBottom;
        private Button btnShowAll;
        private Button btnShowAnnouncements;
        private Button btnBack;

        private EventManager eventManager;

        public EventsForm()
        {
            eventManager = new EventManager();
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1400, 800);
            this.MinimumSize = new Size(1200, 700);

            LoadAllEvents();
            LoadRecommendations();
        }

        private void InitializeComponent()
        {
            this.Text = "📅 Local Events & Announcements";
            this.BackColor = ThemeManager.BackgroundColor;

            // pnlHeader
            this.pnlHeader = new Panel();
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 80;
            this.pnlHeader.BackColor = ThemeManager.PrimaryColor;

            this.pnlHeader.Paint += (sender, e) =>
            {
                Rectangle rect = new Rectangle(0, 0, pnlHeader.Width, pnlHeader.Height);
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    ThemeManager.PrimaryColor,
                    ThemeManager.AccentColor,
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            };

            this.lblTitle = new Label();
            this.lblTitle.Text = "📅 Local Events & Announcements";
            this.lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(30, 10);
            this.lblTitle.Size = new Size(500, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.lblSubtitle = new Label();
            this.lblSubtitle.Text = "Discover what's happening in your community";
            this.lblSubtitle.Font = new Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = Color.FromArgb(220, 230, 240);
            this.lblSubtitle.Location = new Point(35, 50);
            this.lblSubtitle.Size = new Size(500, 25);
            this.lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // pnlSearch
            this.pnlSearch = new Panel();
            this.pnlSearch.Dock = DockStyle.Top;
            this.pnlSearch.Height = 110;
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Padding = new Padding(20);

            // Search label
            Label lblSearch = new Label();
            lblSearch.Text = "🔍 Search Events:";
            lblSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSearch.Location = new Point(20, 10);
            lblSearch.Size = new Size(120, 25);

            // txtSearch
            this.txtSearch = new TextBox();
            this.txtSearch.Font = new Font("Segoe UI", 11F);
            this.txtSearch.Location = new Point(20, 35);
            this.txtSearch.Size = new Size(250, 28);
            this.txtSearch.BorderStyle = BorderStyle.FixedSingle;

            // Category label
            Label lblCategory = new Label();
            lblCategory.Text = "📂 Category:";
            lblCategory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategory.Location = new Point(290, 10);
            lblCategory.Size = new Size(120, 25);

            // cmbCategory
            this.cmbCategory = new ComboBox();
            this.cmbCategory.Font = new Font("Segoe UI", 11F);
            this.cmbCategory.Location = new Point(290, 35);
            this.cmbCategory.Size = new Size(200, 28);
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.Items.Add("-- All Categories --");
            foreach (var cat in eventManager.GetUniqueCategories())
            {
                this.cmbCategory.Items.Add(cat);
            }
            this.cmbCategory.SelectedIndex = 0;

            // Date filter label
            Label lblDateFilter = new Label();
            lblDateFilter.Text = "📅 Filter by Date:";
            lblDateFilter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDateFilter.Location = new Point(510, 10);
            lblDateFilter.Size = new Size(120, 25);

            // chkUseDateFilter
            this.chkUseDateFilter = new CheckBox();
            this.chkUseDateFilter.Text = "Enable";
            this.chkUseDateFilter.Font = new Font("Segoe UI", 9F);
            this.chkUseDateFilter.Location = new Point(630, 10);
            this.chkUseDateFilter.Size = new Size(80, 25);
            this.chkUseDateFilter.CheckedChanged += (s, e) =>
            {
                dtpStartDate.Enabled = chkUseDateFilter.Checked;
                dtpEndDate.Enabled = chkUseDateFilter.Checked;
            };

            // dtpStartDate
            this.dtpStartDate = new DateTimePicker();
            this.dtpStartDate.Font = new Font("Segoe UI", 11F);
            this.dtpStartDate.Location = new Point(510, 35);
            this.dtpStartDate.Size = new Size(140, 28);
            this.dtpStartDate.Format = DateTimePickerFormat.Short;
            this.dtpStartDate.Value = DateTime.Now;
            this.dtpStartDate.Enabled = false;

            // dtpEndDate
            this.dtpEndDate = new DateTimePicker();
            this.dtpEndDate.Font = new Font("Segoe UI", 11F);
            this.dtpEndDate.Location = new Point(660, 35);
            this.dtpEndDate.Size = new Size(140, 28);
            this.dtpEndDate.Format = DateTimePickerFormat.Short;
            this.dtpEndDate.Value = DateTime.Now.AddMonths(1);
            this.dtpEndDate.Enabled = false;

            // btnSearch
            this.btnSearch = new Button();
            this.btnSearch.Text = "🔍 Search";
            this.btnSearch.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnSearch.Size = new Size(120, 35);
            this.btnSearch.Location = new Point(830, 32);
            this.btnSearch.BackColor = ThemeManager.SecondaryColor;
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.Cursor = Cursors.Hand;
            this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);

            // btnClear
            this.btnClear = new Button();
            this.btnClear.Text = "🔄 Clear";
            this.btnClear.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnClear.Size = new Size(100, 35);
            this.btnClear.Location = new Point(960, 32);
            this.btnClear.BackColor = Color.White;
            this.btnClear.ForeColor = ThemeManager.TextPrimary;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            this.btnClear.FlatAppearance.BorderSize = 1;
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            this.pnlSearch.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, lblCategory, cmbCategory,
                lblDateFilter, chkUseDateFilter, dtpStartDate, dtpEndDate,
                btnSearch, btnClear
            });

            // splitContainer
            this.splitContainer = new SplitContainer();
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Orientation = Orientation.Vertical;
            this.splitContainer.SplitterDistance = 900;
            this.splitContainer.BackColor = ThemeManager.BackgroundColor;
            this.splitContainer.Panel1.Padding = new Padding(10);
            this.splitContainer.Panel2.Padding = new Padding(10);

            // Results count label
            this.lblResultsCount = new Label();
            this.lblResultsCount.Text = "Showing all events";
            this.lblResultsCount.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblResultsCount.ForeColor = ThemeManager.TextPrimary;
            this.lblResultsCount.Dock = DockStyle.Top;
            this.lblResultsCount.Height = 30;
            this.lblResultsCount.Padding = new Padding(10, 5, 0, 0);

            // flowEvents
            this.flowEvents = new FlowLayoutPanel();
            this.flowEvents.Dock = DockStyle.Fill;
            this.flowEvents.AutoScroll = true;
            this.flowEvents.BackColor = ThemeManager.BackgroundColor;
            this.flowEvents.Padding = new Padding(10);
            this.flowEvents.FlowDirection = FlowDirection.TopDown;
            this.flowEvents.WrapContents = false;

            this.splitContainer.Panel1.Controls.Add(this.flowEvents);
            this.splitContainer.Panel1.Controls.Add(this.lblResultsCount);

            // Recommendations panel
            this.pnlRecommendations = new Panel();
            this.pnlRecommendations.Dock = DockStyle.Fill;
            this.pnlRecommendations.BackColor = Color.White;
            this.pnlRecommendations.Padding = new Padding(15);

            this.lblRecommendationsTitle = new Label();
            this.lblRecommendationsTitle.Text = "⭐ Recommended For You";
            this.lblRecommendationsTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblRecommendationsTitle.ForeColor = ThemeManager.AccentColor;
            this.lblRecommendationsTitle.Dock = DockStyle.Top;
            this.lblRecommendationsTitle.Height = 40;
            this.lblRecommendationsTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.flowRecommendations = new FlowLayoutPanel();
            this.flowRecommendations.Dock = DockStyle.Fill;
            this.flowRecommendations.AutoScroll = true;
            this.flowRecommendations.FlowDirection = FlowDirection.TopDown;
            this.flowRecommendations.WrapContents = false;
            this.flowRecommendations.BackColor = Color.White;

            this.pnlRecommendations.Controls.Add(this.flowRecommendations);
            this.pnlRecommendations.Controls.Add(this.lblRecommendationsTitle);

            this.splitContainer.Panel2.Controls.Add(this.pnlRecommendations);


            // pnlBottom
            this.pnlBottom = new Panel();
            this.pnlBottom.Dock = DockStyle.Bottom;
            this.pnlBottom.Height = 70;
            this.pnlBottom.BackColor = Color.White;
            this.pnlBottom.Padding = new Padding(20);

            // Left side buttons panel
            Panel pnlLeftButtons = new Panel();
            pnlLeftButtons.Dock = DockStyle.Left;
            pnlLeftButtons.Width = 420;
            pnlLeftButtons.BackColor = Color.Transparent;

            this.btnShowAll = new Button();
            this.btnShowAll.Text = "📋 Show All Events";
            this.btnShowAll.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnShowAll.Size = new Size(180, 40);
            this.btnShowAll.Location = new Point(0, 5);
            this.btnShowAll.BackColor = ThemeManager.SecondaryColor;
            this.btnShowAll.ForeColor = Color.White;
            this.btnShowAll.FlatStyle = FlatStyle.Flat;
            this.btnShowAll.FlatAppearance.BorderSize = 0;
            this.btnShowAll.Cursor = Cursors.Hand;
            this.btnShowAll.Click += (s, e) => LoadAllEvents();

            this.btnShowAnnouncements = new Button();
            this.btnShowAnnouncements.Text = "📢 Announcements Only";
            this.btnShowAnnouncements.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnShowAnnouncements.Size = new Size(210, 40);
            this.btnShowAnnouncements.Location = new Point(195, 5);
            this.btnShowAnnouncements.BackColor = ThemeManager.AccentColor;
            this.btnShowAnnouncements.ForeColor = Color.White;
            this.btnShowAnnouncements.FlatStyle = FlatStyle.Flat;
            this.btnShowAnnouncements.FlatAppearance.BorderSize = 0;
            this.btnShowAnnouncements.Cursor = Cursors.Hand;
            this.btnShowAnnouncements.Click += (s, e) => LoadAnnouncements();

            pnlLeftButtons.Controls.Add(this.btnShowAll);
            pnlLeftButtons.Controls.Add(this.btnShowAnnouncements);

            // Right side - Back button panel
            Panel pnlRightButtons = new Panel();
            pnlRightButtons.Dock = DockStyle.Right;
            pnlRightButtons.Width = 220;
            pnlRightButtons.BackColor = Color.Transparent;

            this.btnBack = new Button();
            this.btnBack.Text = "← Back to Main Menu";
            this.btnBack.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnBack.Size = new Size(200, 40);
            this.btnBack.Location = new Point(0, 5);
            this.btnBack.BackColor = Color.FromArgb(231, 76, 60); // Red for back
            this.btnBack.ForeColor = Color.White;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.Cursor = Cursors.Hand;
            this.btnBack.Click += new EventHandler(this.BtnBack_Click);

            // Hover effect
            this.btnBack.MouseEnter += (s, e) =>
            {
                btnBack.BackColor = Color.FromArgb(200, 60, 45);
            };
            this.btnBack.MouseLeave += (s, e) =>
            {
                btnBack.BackColor = Color.FromArgb(231, 76, 60);
            };

            pnlRightButtons.Controls.Add(this.btnBack);

            // Add panels to bottom
            this.pnlBottom.Controls.Add(pnlLeftButtons);
            this.pnlBottom.Controls.Add(pnlRightButtons);

            // Add to form
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlHeader);
        }

        private void LoadAllEvents()
        {
            flowEvents.Controls.Clear();
            var events = eventManager.GetAllEventsSortedByDate();
            lblResultsCount.Text = $"Showing all {events.Count} events and announcements";
            DisplayEvents(events);
        }

        private void LoadAnnouncements()
        {
            flowEvents.Controls.Clear();
            var announcements = eventManager.GetHighPriorityAnnouncements(20);
            lblResultsCount.Text = $"Showing {announcements.Count} high-priority announcements";
            DisplayEvents(announcements);
        }

        private void DisplayEvents(List<LocalEvent> events)
        {
            if (events.Count == 0)
            {
                Label lblNoResults = new Label();
                lblNoResults.Text = "😔 No events found matching your criteria.\nTry adjusting your search filters.";
                lblNoResults.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
                lblNoResults.ForeColor = ThemeManager.TextSecondary;
                lblNoResults.Size = new Size(flowEvents.Width - 30, 80);
                lblNoResults.TextAlign = ContentAlignment.MiddleCenter;
                flowEvents.Controls.Add(lblNoResults);
                return;
            }

            foreach (var evt in events)
            {
                flowEvents.Controls.Add(CreateEventCard(evt));
            }
        }

        private Panel CreateEventCard(LocalEvent evt)
        {
            Panel card = new Panel();
            card.Size = new Size(flowEvents.Width - 30, 150);
            card.BackColor = Color.White;
            card.Margin = new Padding(5, 5, 5, 10);
            card.Padding = new Padding(15);
            card.Cursor = Cursors.Hand;

            // Border paint
            card.Paint += (sender, e) =>
            {
                Control pnl = sender as Control;
                Color borderColor = evt.IsAnnouncement ? ThemeManager.AccentColor :
                                   evt.Priority >= 4 ? ThemeManager.DangerColor :
                                   ThemeManager.SecondaryColor;

                using (Pen pen = new Pen(borderColor, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
                }

                // Left accent bar
                using (SolidBrush brush = new SolidBrush(borderColor))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, 5, pnl.Height);
                }
            };

            // Click to view details
            EventHandler clickHandler = (s, e) =>
            {
                eventManager.AddToRecentlyViewed(evt);
                ShowEventDetails(evt);
            };
            card.Click += clickHandler;

            // Category badge
            Label lblCategory = new Label();
            lblCategory.Text = evt.IsAnnouncement ? $"📢 {evt.Category}" : $"📂 {evt.Category}";
            lblCategory.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCategory.ForeColor = Color.White;
            lblCategory.BackColor = evt.IsAnnouncement ? ThemeManager.AccentColor :
                                    evt.Priority >= 4 ? ThemeManager.DangerColor :
                                    ThemeManager.SecondaryColor;
            lblCategory.Location = new Point(15, 10);
            lblCategory.Size = new Size(120, 22);
            lblCategory.TextAlign = ContentAlignment.MiddleCenter;
            lblCategory.Click += clickHandler;

            // Priority indicator
            if (evt.Priority >= 4)
            {
                Label lblPriority = new Label();
                lblPriority.Text = "⭐ HIGH PRIORITY";
                lblPriority.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                lblPriority.ForeColor = ThemeManager.DangerColor;
                lblPriority.Location = new Point(145, 13);
                lblPriority.Size = new Size(110, 18);
                lblPriority.Click += clickHandler;
                card.Controls.Add(lblPriority);
            }

            // Title
            Label lblEventTitle = new Label();
            lblEventTitle.Text = evt.Title;
            lblEventTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblEventTitle.ForeColor = ThemeManager.TextPrimary;
            lblEventTitle.Location = new Point(15, 38);
            lblEventTitle.Size = new Size(card.Width - 200, 28);
            lblEventTitle.Click += clickHandler;
            card.Controls.Add(lblEventTitle);

            // Date and location
            Label lblDateTime = new Label();
            lblDateTime.Text = $"📅 {evt.EventDate:dddd, MMMM dd, yyyy} at {evt.EventDate:HH:mm}";
            lblDateTime.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDateTime.ForeColor = ThemeManager.SecondaryColor;
            lblDateTime.Location = new Point(15, 68);
            lblDateTime.Size = new Size(card.Width - 30, 20);
            lblDateTime.Click += clickHandler;
            card.Controls.Add(lblDateTime);

            Label lblLocation = new Label();
            lblLocation.Text = $"📍 {evt.Location}";
            lblLocation.Font = new Font("Segoe UI", 10F);
            lblLocation.ForeColor = ThemeManager.TextSecondary;
            lblLocation.Location = new Point(15, 90);
            lblLocation.Size = new Size(card.Width - 30, 20);
            lblLocation.Click += clickHandler;
            card.Controls.Add(lblLocation);

            // Description preview
            Label lblDesc = new Label();
            string desc = evt.Description.Length > 80 ? evt.Description.Substring(0, 80) + "..." : evt.Description;
            lblDesc.Text = desc;
            lblDesc.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblDesc.ForeColor = ThemeManager.TextSecondary;
            lblDesc.Location = new Point(15, 112);
            lblDesc.Size = new Size(card.Width - 180, 25);
            lblDesc.Click += clickHandler;
            card.Controls.Add(lblDesc);

            // Price badge
            Label lblPrice = new Label();
            lblPrice.Text = evt.IsFree ? "FREE" : $"R{evt.TicketPrice:F2}";
            lblPrice.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPrice.ForeColor = Color.White;
            lblPrice.BackColor = evt.IsFree ? ThemeManager.SuccessColor : ThemeManager.SecondaryColor;
            lblPrice.Location = new Point(card.Width - 130, 38);
            lblPrice.Size = new Size(110, 30);
            lblPrice.TextAlign = ContentAlignment.MiddleCenter;
            lblPrice.Click += clickHandler;
            card.Controls.Add(lblPrice);

            // Days until label
            int daysUntil = (evt.EventDate.Date - DateTime.Now.Date).Days;
            Label lblDaysUntil = new Label();
            if (daysUntil == 0)
                lblDaysUntil.Text = "🎯 TODAY!";
            else if (daysUntil == 1)
                lblDaysUntil.Text = "⏰ TOMORROW";
            else if (daysUntil > 0)
                lblDaysUntil.Text = $"⏳ In {daysUntil} days";
            else
                lblDaysUntil.Text = "✓ Past event";

            lblDaysUntil.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDaysUntil.ForeColor = daysUntil <= 2 && daysUntil >= 0 ? ThemeManager.DangerColor : ThemeManager.TextSecondary;
            lblDaysUntil.Location = new Point(card.Width - 130, 75);
            lblDaysUntil.Size = new Size(110, 20);
            lblDaysUntil.TextAlign = ContentAlignment.MiddleCenter;
            lblDaysUntil.Click += clickHandler;
            card.Controls.Add(lblDaysUntil);

            card.Controls.Add(lblCategory);

            return card;
        }

        private void ShowEventDetails(LocalEvent evt)
        {
            string details =
                $"📌 {evt.Title}\n\n" +
                $"📂 Category: {evt.Category}\n" +
                $"📅 Date: {evt.EventDate:dddd, MMMM dd, yyyy}\n" +
                $"⏰ Time: {evt.EventDate:HH:mm}\n" +
                $"📍 Location: {evt.Location}\n\n" +
                $"📝 Description:\n{evt.Description}\n\n" +
                $"👤 Organizer: {evt.Organizer}\n" +
                $"📞 Contact: {evt.ContactInfo}\n" +
                $"💰 Price: {(evt.IsFree ? "FREE" : $"R{evt.TicketPrice:F2}")}\n" +
                $"⭐ Priority: {evt.Priority}/5";

            MessageBox.Show(details, "Event Details",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            EventCategory? category = null;

            if (cmbCategory.SelectedIndex > 0)
            {
                category = (EventCategory)cmbCategory.SelectedItem;
            }

            DateTime? startDate = chkUseDateFilter.Checked ? dtpStartDate.Value.Date : (DateTime?)null;
            DateTime? endDate = chkUseDateFilter.Checked ? dtpEndDate.Value.Date : (DateTime?)null;

            var results = eventManager.SearchEvents(searchTerm, category, startDate, endDate);

            flowEvents.Controls.Clear();
            lblResultsCount.Text = $"🔍 Found {results.Count} event(s) matching your search";
            DisplayEvents(results);

            // Refresh recommendations based on new search
            LoadRecommendations();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbCategory.SelectedIndex = 0;
            chkUseDateFilter.Checked = false;
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now.AddMonths(1);
            LoadAllEvents();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to return to the Main Menu?\nAny unsaved data will be lost.",
                "Confirm Navigation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void LoadRecommendations()
        {
            flowRecommendations.Controls.Clear();
            var recommendations = eventManager.GetRecommendations(6);

            if (recommendations.Count == 0)
            {
                Label lblNoRecs = new Label();
                lblNoRecs.Text = "🔍 Search for events to get personalized recommendations!";
                lblNoRecs.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
                lblNoRecs.ForeColor = ThemeManager.TextSecondary;
                lblNoRecs.Size = new Size(flowRecommendations.Width - 30, 60);
                lblNoRecs.TextAlign = ContentAlignment.MiddleCenter;
                flowRecommendations.Controls.Add(lblNoRecs);
                return;
            }

            foreach (var rec in recommendations)
            {
                flowRecommendations.Controls.Add(CreateRecommendationCard(rec));
            }
        }

        private Panel CreateRecommendationCard(EventRecommendation rec)
        {
            Panel card = new Panel();
            card.Size = new Size(flowRecommendations.Width - 30, 120);
            card.BackColor = Color.FromArgb(255, 250, 240);
            card.Margin = new Padding(5);
            card.Padding = new Padding(12);
            card.Cursor = Cursors.Hand;

            card.Paint += (sender, e) =>
            {
                Control pnl = sender as Control;
                using (Pen pen = new Pen(ThemeManager.AccentColor, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
                }
            };

            EventHandler clickHandler = (s, e) =>
            {
                eventManager.AddToRecentlyViewed(rec.Event);
                ShowEventDetails(rec.Event);
            };
            card.Click += clickHandler;

            // Score badge
            Label lblScore = new Label();
            lblScore.Text = $"⭐ {rec.Score:F1}";
            lblScore.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblScore.ForeColor = Color.White;
            lblScore.BackColor = ThemeManager.AccentColor;
            lblScore.Location = new Point(12, 10);
            lblScore.Size = new Size(60, 20);
            lblScore.TextAlign = ContentAlignment.MiddleCenter;
            lblScore.Click += clickHandler;
            card.Controls.Add(lblScore);

            // Category badge
            Label lblCat = new Label();
            lblCat.Text = rec.Event.Category.ToString();
            lblCat.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblCat.ForeColor = ThemeManager.SecondaryColor;
            lblCat.Location = new Point(80, 12);
            lblCat.Size = new Size(120, 18);
            lblCat.Click += clickHandler;
            card.Controls.Add(lblCat);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = rec.Event.Title;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.ForeColor = ThemeManager.TextPrimary;
            lblTitle.Location = new Point(12, 35);
            lblTitle.Size = new Size(card.Width - 24, 22);
            lblTitle.Click += clickHandler;
            card.Controls.Add(lblTitle);

            // Date
            Label lblDate = new Label();
            lblDate.Text = $"📅 {rec.Event.EventDate:MMM dd, yyyy}";
            lblDate.Font = new Font("Segoe UI", 9F);
            lblDate.ForeColor = ThemeManager.SecondaryColor;
            lblDate.Location = new Point(12, 60);
            lblDate.Size = new Size(card.Width - 24, 18);
            lblDate.Click += clickHandler;
            card.Controls.Add(lblDate);

            // Reason
            Label lblReason = new Label();
            string reason = rec.Reason.Length > 55 ? rec.Reason.Substring(0, 55) + "..." : rec.Reason;
            lblReason.Text = $"💡 {reason}";
            lblReason.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblReason.ForeColor = ThemeManager.TextSecondary;
            lblReason.Location = new Point(12, 82);
            lblReason.Size = new Size(card.Width - 24, 30);
            lblReason.Click += clickHandler;
            card.Controls.Add(lblReason);

            return card;
        }
    }
}