WinForms Notification Banner
An elegant notification banner system for Windows Forms applications with smooth animations and full customization.

🚀 Installation
bashInstall-Package WinFormsNotificationBanner
or through Package Manager in Visual Studio.

📋 Features
✅ 4 notification types: Errors, warnings, success, info
✅ Smooth animations: Slides down from top of screen
✅ Auto-hide: After specified time or on click
✅ Responsive: Adapts to window size
✅ Customizable themes: Colors, fonts, icons
✅ Thread-safe: Works correctly with multiple threads
✅ Rounded corners: Modern appearance

🎯 Quick Start
csharp
using WinFormsNotificationBanner;

// In your form code
private void Button_Click(object sender, EventArgs e)
{
    // Error
    NotificationManager.ShowError(this, "Cannot connect to database!");
    
    // Warning
    NotificationManager.ShowWarning(this, "Email field is required.");
    
    // Success
    NotificationManager.ShowSuccess(this, "Data saved successfully!");
    
    // Info
    NotificationManager.ShowInfo(this, "New version available for download.");
}

🎨 Theme Customization
Using predefined themes:

csharp
// Set default theme for entire application
NotificationManager.SetDefaultTheme(NotificationTheme.Dark);

// Or use theme for single notification
NotificationManager.ShowError(this, "Error!", 5000, NotificationTheme.Minimal);

Creating custom theme:
csharpvar customTheme = new NotificationTheme()
{
    BannerHeight = 70,
    CornerRadius = 15,
    MessageFont = new Font("Arial", 12),
    ShowCloseButton = true,
    EnableShadow = true
}
.SetErrorStyle(Color.DarkRed, Color.White, "❌")
.SetSuccessStyle(Color.DarkGreen, Color.White, "✅");

NotificationManager.SetDefaultTheme(customTheme);

📖 Complete API
NotificationManager
csharp
// Basic methods
NotificationManager.ShowError(Form parent, string message, int duration = 7000);
NotificationManager.ShowWarning(Form parent, string message, int duration = 5000);
NotificationManager.ShowSuccess(Form parent, string message, int duration = 4000);
NotificationManager.ShowInfo(Form parent, string message, int duration = 5000);

// Advanced
NotificationManager.ShowNotification(Form parent, string message, NotificationType type, int duration, NotificationTheme theme);
NotificationManager.ShowException(Form parent, Exception ex, string customMessage = null);

// Management
NotificationManager.SetDefaultTheme(NotificationTheme theme);
NotificationManager.SetMaxSimultaneousBanners(int max);
NotificationManager.HideAllNotifications();

NotificationTheme
csharpvar theme = new NotificationTheme()
{
    MessageFont = new Font("Segoe UI", 10),     // Message font
    IconFont = new Font("Segoe UI Symbol", 16), // Icon font
    BannerHeight = 60,                          // Banner height
    TopMargin = 20,                            // Top margin
    CornerRadius = 8,                          // Corner rounding
    ShowCloseButton = true,                    // Close button
    EnableShadow = false                       // Shadow effect
};

// Predefined themes
NotificationTheme.Default    // Standard
NotificationTheme.Dark       // Dark theme
NotificationTheme.Minimal    // Minimalistic

🛠️ Error Handling in Applications
csharp
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        
        // Set global handler for unhandled exceptions
        Application.ThreadException += (s, e) => 
            NotificationManager.ShowException(this, e.Exception);
            
        AppDomain.CurrentDomain.UnhandledException += (s, e) => 
            NotificationManager.ShowException(this, (Exception)e.ExceptionObject);
    }
    
    private async void SaveData()
    {
        try
        {
            await SomeAsyncOperation();
            NotificationManager.ShowSuccess(this, "Data saved successfully!");
        }
        catch (Exception ex)
        {
            NotificationManager.ShowException(this, ex, "Error saving data");
        }
    }
}
🎯 Usage Examples

Form validation:
csharpprivate void ValidateForm()
{
    if (string.IsNullOrEmpty(textBoxEmail.Text))
    {
        NotificationManager.ShowWarning(this, "Email field is required!");
        return;
    }
    
    if (!IsValidEmail(textBoxEmail.Text))
    {
        NotificationManager.ShowError(this, "Invalid email format!");
        return;
    }
    
    NotificationManager.ShowSuccess(this, "Form completed successfully!");
}

Async operations:
csharpprivate async void DownloadFile()
{
    NotificationManager.ShowInfo(this, "Starting download...");
    
    try
    {
        await DownloadFileAsync();
        NotificationManager.ShowSuccess(this, "File downloaded successfully!");
    }
    catch (Exception ex)
    {
        NotificationManager.ShowError(this, $"Download error: {ex.Message}");
    }
}

File operations:
csharpprivate void SaveFile()
{
    try
    {
        File.WriteAllText("data.txt", GetData());
        NotificationManager.ShowSuccess(this, "File saved to data.txt");
    }
    catch (UnauthorizedAccessException)
    {
        NotificationManager.ShowError(this, "Access denied. Check file permissions.");
    }
    catch (IOException ex)
    {
        NotificationManager.ShowError(this, $"File operation failed: {ex.Message}");
    }
}

Network operations:
csharpprivate async void CheckConnection()
{
    NotificationManager.ShowInfo(this, "Checking connection...");
    
    using (var client = new HttpClient())
    {
        try
        {
            var response = await client.GetAsync("https://api.example.com/health");
            if (response.IsSuccessStatusCode)
            {
                NotificationManager.ShowSuccess(this, "Connection successful!");
            }
            else
            {
                NotificationManager.ShowWarning(this, $"Server returned: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            NotificationManager.ShowError(this, "Network connection failed!");
        }
    }
}


🔧 Requirements
.NET Framework 4.8+ or .NET 6.0+ (Windows)
Windows Forms

🌟 Advanced Features
Multiple notifications management:
csharp
// Limit simultaneous notifications
NotificationManager.SetMaxSimultaneousBanners(3);

// Hide all active notifications
NotificationManager.HideAllNotifications();
Custom duration per type:
csharpNotificationManager.ShowError(this, "Critical error!", 10000);   // 10 seconds
NotificationManager.ShowWarning(this, "Warning", 5000);         // 5 seconds
NotificationManager.ShowSuccess(this, "Success", 3000);         // 3 seconds
NotificationManager.ShowInfo(this, "Info", 4000);              // 4 seconds
Responsive design:
The notifications automatically adapt to the parent form's width and position themselves correctly even when the form is resized or moved.
📝 License
MIT License - free to use in commercial and non-commercial projects.
🔄 Changelog
v1.0.0

Initial release
4 notification types with smooth animations
Customizable themes and styling
Thread-safe operations
Multiple notifications management