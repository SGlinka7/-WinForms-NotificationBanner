using System;
using System.Drawing;

namespace WinFormsNotificationBanner
{
    public class NotificationStyle
    {
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public string Icon { get; set; }
    }

    public class NotificationTheme
    {
        public Font MessageFont { get; set; } = new Font("Segoe UI", 10, FontStyle.Regular);
        public Font IconFont { get; set; } = new Font("Segoe UI Symbol", 16);
        public int BannerHeight { get; set; } = 60;
        public int TopMargin { get; set; } = 20;
        public int CornerRadius { get; set; } = 8;
        public int IconSize { get; set; } = 20;
        public bool ShowCloseButton { get; set; } = true;
        public bool EnableShadow { get; set; } = false;

        private NotificationStyle errorStyle;
        private NotificationStyle warningStyle;
        private NotificationStyle successStyle;
        private NotificationStyle infoStyle;

        public NotificationTheme()
        {
            SetDefaultStyles();
        }

        private void SetDefaultStyles()
        {
            errorStyle = new NotificationStyle
            {
                BackgroundColor = Color.FromArgb(244, 67, 54),
                TextColor = Color.White,
                Icon = "✖"
            };

            warningStyle = new NotificationStyle
            {
                BackgroundColor = Color.FromArgb(255, 193, 7),
                TextColor = Color.Black,
                Icon = "⚠"
            };

            successStyle = new NotificationStyle
            {
                BackgroundColor = Color.FromArgb(76, 175, 80),
                TextColor = Color.White,
                Icon = "✓"
            };

            infoStyle = new NotificationStyle
            {
                BackgroundColor = Color.FromArgb(33, 150, 243),
                TextColor = Color.White,
                Icon = "ℹ"
            };
        }

        public NotificationStyle GetStyle(NotificationType type)
        {
            return type switch
            {
                NotificationType.Error => errorStyle,
                NotificationType.Warning => warningStyle,
                NotificationType.Success => successStyle,
                NotificationType.Info => infoStyle,
                _ => infoStyle
            };
        }

        // Predefiniowane motywy
        public static NotificationTheme Default => new NotificationTheme();

        public static NotificationTheme Dark => new NotificationTheme
        {
            MessageFont = new Font("Segoe UI", 10, FontStyle.Regular),
            BannerHeight = 55,
            CornerRadius = 6,
            ShowCloseButton = false,
            EnableShadow = true
        };

        public static NotificationTheme Minimal => new NotificationTheme
        {
            MessageFont = new Font("Segoe UI", 9, FontStyle.Regular),
            BannerHeight = 40,
            CornerRadius = 4,
            ShowCloseButton = false,
            EnableShadow = false
        };

        // Metody do customizacji
        public NotificationTheme SetErrorStyle(Color backgroundColor, Color textColor, string icon = "✖")
        {
            errorStyle = new NotificationStyle { BackgroundColor = backgroundColor, TextColor = textColor, Icon = icon };
            return this;
        }

        public NotificationTheme SetWarningStyle(Color backgroundColor, Color textColor, string icon = "⚠")
        {
            warningStyle = new NotificationStyle { BackgroundColor = backgroundColor, TextColor = textColor, Icon = icon };
            return this;
        }

        public NotificationTheme SetSuccessStyle(Color backgroundColor, Color textColor, string icon = "✓")
        {
            successStyle = new NotificationStyle { BackgroundColor = backgroundColor, TextColor = textColor, Icon = icon };
            return this;
        }

        public NotificationTheme SetInfoStyle(Color backgroundColor, Color textColor, string icon = "ℹ")
        {
            infoStyle = new NotificationStyle { BackgroundColor = backgroundColor, TextColor = textColor, Icon = icon };
            return this;
        }
    }
}
