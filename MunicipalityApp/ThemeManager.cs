using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public static class ThemeManager
    {
        // Modern Color Palette
        public static Color PrimaryColor = Color.FromArgb(52, 73, 94);
        public static Color SecondaryColor = Color.FromArgb(41, 128, 185);
        public static Color AccentColor = Color.FromArgb(241, 196, 15);
        public static Color SuccessColor = Color.FromArgb(46, 204, 113);
        public static Color DangerColor = Color.FromArgb(231, 76, 60);
        public static Color WarningColor = Color.FromArgb(241, 196, 15);
        public static Color BackgroundColor = Color.FromArgb(236, 240, 241);
        public static Color CardColor = Color.White;
        public static Color TextPrimary = Color.FromArgb(44, 62, 80);
        public static Color TextSecondary = Color.FromArgb(149, 165, 166);

        // Fonts
        public static Font HeaderFont = new Font("Segoe UI", 18F, FontStyle.Bold);
        public static Font SubHeaderFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static Font BodyFont = new Font("Segoe UI", 11F);
        public static Font LabelFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static Font ButtonFont = new Font("Segoe UI", 11F, FontStyle.Bold);

        public static void ApplyTheme(Form form)
        {
            form.BackColor = BackgroundColor;
            form.Font = BodyFont;
            form.ForeColor = TextPrimary;
        }

        public static void StyleButton(Button btn, bool isPrimary = true)
        {
            btn.Font = ButtonFont;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Padding = new Padding(10, 5, 10, 5);

            if (isPrimary)
            {
                btn.BackColor = SecondaryColor;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 97, 141);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(21, 67, 96);
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = TextPrimary;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(220, 220, 220);
            }
        }

        public static void StyleCard(Panel panel)
        {
            panel.BackColor = CardColor;
            panel.Padding = new Padding(20);
        }
    }
}