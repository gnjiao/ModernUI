using System.Windows.Controls;
using Platform.Main.Util;

namespace Platform.Main.Views.Main
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public static LogView Instance = new LogView();

        public LogView()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                PlatformServiceTools.Log.OnLoggingEvent += Log_OnLoggingEvent;
            };

            Unloaded += (sender, e) =>
            {
                PlatformServiceTools.Log.OnLoggingEvent -= Log_OnLoggingEvent;
            };
        }

        private static void Log_OnLoggingEvent(object sender, Core.LoggingArgs args)
        {
            //
        }
    }    
}
