using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WinFormsNotificationBanner
{
    public enum NotificationType
    {
        Error,
        Warning,
        Success,
        Info
    }

    public partial class NotificationBanner : Form
    {
        private Timer slideTimer;
        private Timer hideTimer;
        private int targetY;
        private int slideSpeed = 12;
        private Form parentForm;
        private Label messageLabel;
        private Panel iconPanel;
        private bool isHiding = false;
        private NotificationTheme theme;

        public NotificationBanner(Form parent, string message, NotificationType type, int displayDurationMs = 5000, NotificationTheme customTheme = null)
        {
            parentForm = parent ?? throw new ArgumentNullException(nameof(parent));
            theme = customTheme ?? NotificationTheme.Default;
            InitializeBanner(message, type);
            SetupTimers(displayDurationMs);
        }

        private void InitializeBanner(string message, NotificationType type)
        {
            // Podstawowe ustawienia formy
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;

            // Rozmiar i pozycja
            this.Width = Math.Max(300, Math.Min(parentForm.Width - 40, 600));
            this.Height = theme.BannerHeight;
            this.Left = parentForm.Left + (parentForm.Width - this.Width) / 2; // Wyśrodkowanie
            this.Top = parentForm.Top - this.Height;

            targetY = parentForm.Top + theme.TopMargin;

            // Kolory według typu powiadomienia
            var style = theme.GetStyle(type);
            this.BackColor = style.BackgroundColor;

            // Dodanie cienia (opcjonalnie)
            if (theme.EnableShadow)
            {
                this.SetStyle(ControlStyles.UserPaint, true);
                this.Paint += DrawShadow;
            }

            // Panel z ikoną
            iconPanel = new Panel()
            {
                Width = theme.IconSize + 20,
                Height = this.Height,
                Dock = DockStyle.Left,
                BackColor = Color.Transparent
            };

            Label iconLabel = new Label()
            {
                Text = style.Icon,
                Font = new Font(theme.IconFont.FontFamily, theme.IconFont.Size, FontStyle.Bold),
                ForeColor = style.TextColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            iconPanel.Controls.Add(iconLabel);

            // Label z wiadomością
            messageLabel = new Label()
            {
                Text = message,
                Font = theme.MessageFont,
                ForeColor = style.TextColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 15, 5),
                AutoEllipsis = true
            };

            // Przycisk zamknięcia (opcjonalnie)
            Panel closePanel = null;
            if (theme.ShowCloseButton)
            {
                closePanel = new Panel()
                {
                    Width = 30,
                    Height = this.Height,
                    Dock = DockStyle.Right,
                    BackColor = Color.Transparent
                };

                Label closeLabel = new Label()
                {
                    Text = "×",
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    ForeColor = style.TextColor,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Cursor = Cursors.Hand
                };

                closeLabel.Click += (s, e) => HideBanner();
                closePanel.Controls.Add(closeLabel);
            }

            // Dodanie kontrolek
            this.Controls.Add(messageLabel);
            this.Controls.Add(iconPanel);
            if (closePanel != null)
                this.Controls.Add(closePanel);

            // Zaokrąglone rogi
            if (theme.CornerRadius > 0)
            {
                this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, theme.CornerRadius, theme.CornerRadius));
            }

            // Event handlery
            SetupEventHandlers(iconLabel);
        }

        private void SetupEventHandlers(Label iconLabel)
        {
            this.Click += (s, e) => HideBanner();
            messageLabel.Click += (s, e) => HideBanner();
            iconLabel.Click += (s, e) => HideBanner();
            iconPanel.Click += (s, e) => HideBanner();

            // Efekty hover
            var controls = new Control[] { this, messageLabel, iconLabel, iconPanel };
            foreach (var control in controls)
            {
                control.MouseEnter += (s, e) => this.Cursor = Cursors.Hand;
                control.MouseLeave += (s, e) => this.Cursor = Cursors.Default;
            }
        }

        private void DrawShadow(object sender, PaintEventArgs e)
        {
            // Prosty efekt cienia
            using (var shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(shadowBrush, 2, 2, this.Width - 2, this.Height - 2);
            }
        }

        private void SetupTimers(int displayDurationMs)
        {
            slideTimer = new Timer();
            slideTimer.Interval = 16; // ~60 FPS
            slideTimer.Tick += SlideTimer_Tick;

            hideTimer = new Timer();
            hideTimer.Interval = displayDurationMs;
            hideTimer.Tick += (s, e) => HideBanner();
        }

        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            if (!isHiding)
            {
                if (this.Top < targetY)
                {
                    this.Top += slideSpeed;
                    if (this.Top >= targetY)
                    {
                        this.Top = targetY;
                        slideTimer.Stop();
                        hideTimer.Start();
                    }
                }
            }
            else
            {
                this.Top -= slideSpeed;
                if (this.Top <= parentForm.Top - this.Height)
                {
                    slideTimer.Stop();
                    this.Close();
                }
            }
        }

        public void ShowBanner()
        {
            if (parentForm.InvokeRequired)
            {
                parentForm.Invoke(new Action(ShowBanner));
                return;
            }

            this.Show();
            slideTimer.Start();
        }

        public void HideBanner()
        {
            if (isHiding) return;

            isHiding = true;
            hideTimer?.Stop();
            slideTimer?.Start();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                slideTimer?.Dispose();
                hideTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}