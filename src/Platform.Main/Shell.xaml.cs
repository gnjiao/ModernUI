using System;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Platform.Main.Themes;
using Platform.Main.Util;
using Platform.Main.Views;

namespace Platform.Main
{
    /// <inheritdoc cref="ModernWindow" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : ModernWindow
    {
        public Shell()
        {
            InitializeComponent();

            InitializeMainWindow();                  
        }

        private void InitializeMainWindow()
        {
            Title = PlatformServiceTools.ResourceService.GetString("Base.WindowName");
            WindowState = WindowState.Maximized;
//            Width = 1024;
//            Height = 768;
            AppearanceManager.Current.ThemeSource = new Uri(ThemesPath.Light, UriKind.Relative);
            //AppearanceManager.Current.AccentColor = Colors.Coral;      

            PreviewKeyDown += (sender, e) =>
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        WindowState = System.Windows.WindowState.Normal;
                        WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                        break;
                    case Key.F11:
                        Topmost = true;
                        WindowStyle = System.Windows.WindowStyle.None;
                        WindowState = System.Windows.WindowState.Maximized;
                        break;
                }
            };

            DesignPage.StartUpMainWindow();

            ContentSource = GetUri(typeof(AoiPage));
        }
        

        #region MenuLinkGroup

        public void AddLinkGroups(LinkGroupCollection linkGroupCollection)
        {
            CreateMenuLinkGroup();

            foreach (var linkGroup in linkGroupCollection)
            {
                MenuLinkGroups.Add(linkGroup);
            }
        }

        private static Uri GetUri(Type viewType)
        {
            return new Uri($"/Views/{viewType.Name}.xaml", UriKind.Relative);
        }

        private void CreateMenuLinkGroup()
        {
            this.MenuLinkGroups.Clear();

            var linkGroup = new LinkGroup
            {
                DisplayName = "AOI检测",
            };

            linkGroup.Links.Add(new Link
            {
                DisplayName = "检测",
                Source = GetUri(typeof(AoiPage))
            });

            linkGroup.Links.Add(new Link
            {
                DisplayName = "编辑",
                Source = GetUri(typeof(DesignPage))
            });

            MenuLinkGroups.Add(linkGroup);

            linkGroup = new LinkGroup
            {
                DisplayName = "设置",
                GroupKey = "settings" 
            };

            linkGroup.Links.Add(new Link
            {
                DisplayName = "个性化",
                Source = GetUri(typeof(SettingsPage))
            });


            MenuLinkGroups.Add(linkGroup);
        }

        #endregion


    }
}
