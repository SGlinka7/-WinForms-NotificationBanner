using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFormsNotificationBanner
{
    public static class NotificationManager
    {
        private static NotificationTheme defaultTheme = NotificationTheme.Default;
        private static readonly List<NotificationBanner> activeBanners = new List<NotificationBanner>();
        private static int maxSimultaneousBanners = 3;

        /// <summary>
        /// Ustawia domyślny motyw dla wszystkich powiadomień
        /// </summary>
        public static void SetDefaultTheme(NotificationTheme theme)
        {
            defaultTheme = theme ?? NotificationTheme.Default;
        }

        /// <summary>
        /// Ustawia maksymalną liczbę jednocześnie wyświetlanych powiadomień
        /// </summary>
        public static void SetMaxSimultaneousBanners(int max)
        {
            maxSimultaneousBanners = Math.Max(1, max);
        }

        /// <summary>
        /// Wyświetla powiadomienie o określonym typie
        /// </summary>
        public static void ShowNotification(Form parentForm, string message, NotificationType type, int durationMs = 5000, NotificationTheme customTheme = null)
        {
            if (parentForm == null) throw new ArgumentNullException(nameof(parentForm));
            if (string.IsNullOrEmpty(message)) return;

            if (parentForm.InvokeRequired)
            {
                parentForm.Invoke(new Action(() => ShowNotification(parentForm, message, type, durationMs, customTheme)));
                return;
            }

            // Ogranicz liczbę aktywnych powiadomień
            CleanupClosedBanners();
            if (activeBanners.Count >= maxSimultaneousBanners)
            {
                activeBanners[0].HideBanner();
                activeBanners.RemoveAt(0);
            }

            var theme = customTheme ?? defaultTheme;
            var banner = new NotificationBanner(parentForm, message, type, durationMs, theme);

            // Dostosuj pozycję jeśli jest więcej powiadomień
            if (activeBanners.Count > 0)
            {
                banner.Top = activeBanners[activeBanners.Count - 1].Bottom + 10;
            }

            activeBanners.Add(banner);
            banner.FormClosed += (s, e) => activeBanners.Remove(banner);
            banner.ShowBanner();
        }

        private static void CleanupClosedBanners()
        {
            activeBanners.RemoveAll(b => b.IsDisposed);
        }

        /// <summary>
        /// Wyświetla powiadomienie o błędzie
        /// </summary>
        public static void ShowError(Form parentForm, string message, int durationMs = 7000)
        {
            ShowNotification(parentForm, message, NotificationType.Error, durationMs);
        }

        /// <summary>
        /// Wyświetla ostrzeżenie
        /// </summary>
        public static void ShowWarning(Form parentForm, string message, int durationMs = 5000)
        {
            ShowNotification(parentForm, message, NotificationType.Warning, durationMs);
        }

        /// <summary>
        /// Wyświetla powiadomienie o sukcesie
        /// </summary>
        public static void ShowSuccess(Form parentForm, string message, int durationMs = 4000)
        {
            ShowNotification(parentForm, message, NotificationType.Success, durationMs);
        }

        /// <summary>
        /// Wyświetla powiadomienie informacyjne
        /// </summary>
        public static void ShowInfo(Form parentForm, string message, int durationMs = 5000)
        {
            ShowNotification(parentForm, message, NotificationType.Info, durationMs);
        }

        /// <summary>
        /// Ukrywa wszystkie aktywne powiadomienia
        /// </summary>
        public static void HideAllNotifications()
        {
            foreach (var banner in activeBanners.ToArray())
            {
                banner.HideBanner();
            }
        }

        /// <summary>
        /// Wyświetla powiadomienie z wyjątkiem
        /// </summary>
        public static void ShowException(Form parentForm, Exception ex, string customMessage = null)
        {
            var message = customMessage ?? $"Wystąpił błąd: {ex.Message}";
            ShowError(parentForm, message);
        }
    }
}