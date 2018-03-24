using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace CourseWork
{    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow(string login)
        {
            InitializeComponent();
            
            InitializeTabs();

            Title += "(" + login + ")";

            Icon = ImageConverter.convertBtmToBtmSource(Properties.Resources.logo);

            string FullName;
            using (var db = new ModelContainer1())
            FullName = (from u in db.UserSet where u.Login == login select u).First().FullName;

            // Часы
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += delegate {while(true) {System.Threading.Tasks.Task.Delay(100);try{Dispatcher.Invoke(delegate { tblTime.Text = FullName +" "+ System.DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"); });}catch{bw.CancelAsync();}}};
            bw.RunWorkerAsync();
        }

        private void InitializeTabs()
        {
            frSearch.Content = new AdminWindowSearchTabPage();
            FrTabAddMeter.Content =  new AdminWindowAddMeterTabPage(this);
            frDelMeter.Content = new AdminWindowDelMeterTabPage();
            frExtendMeter.Content = new AdminWindowExtendMeterTabPage();
        }

        // MENU
        private void GoToAddTab_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void GoToExtendTab_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 2;
        }

        private void GoToDeleteTab_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 3;
        }

        private void GoToSearchTab_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 4;
        }
    }
}
