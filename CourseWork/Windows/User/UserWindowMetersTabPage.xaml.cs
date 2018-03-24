using System;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для UserWindowMetersTabPage.xaml
    /// </summary>
    public partial class UserWindowMetersTabPage : Page
    {
        string login;

        public UserWindowMetersTabPage(string login)
        {
            InitializeComponent();

            this.login = login;

            InitializeFields();
        }

        private void InitializeFields()
        {
            RefreshMeterTable_OnClick(null,null);
        }

        private void RefreshMeterTable_OnClick(object sender, RoutedEventArgs e)
        {
            lbMeters.Items.Clear();
            using (var db = new ModelContainer1())
            {
                foreach (var m in (from m in db.MeterSet where m.User.Login == login select m).ToList())
                {
                    TextBlock tbl = new TextBlock() { Text = m.ToString(), Background = Brushes.LightGray, Width = 235 };
                    if (m is InstalledMeter && (m as InstalledMeter).ExpirationDate <= DateTime.Now)
                        tbl.Foreground = Brushes.Red;
                    if (!(m is InstalledMeter))
                        tbl.Foreground = Brushes.Blue;
                    tbl.MouseLeftButtonUp += delegate { ShowMeterInfo(m); };
                    lbMeters.Items.Add(tbl);
                }
            }
        }

        private void ShowMeterInfo(Meter meter)
        {
            new Window()
            {
                Content = new MeterInfoPage(meter),
                Height = 550,
                Width = 720,
                Title = "Счётчик " + meter.Name,
                ResizeMode = ResizeMode.NoResize,
                Icon = ImageConverter.convertBtmToBtmSource(Properties.Resources.logo)
            }.ShowDialog();
        }
    }
}
