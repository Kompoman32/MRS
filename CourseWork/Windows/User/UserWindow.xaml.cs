using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow(string login)
        {
            InitializeComponent();

            Title += "(" + login + ")";
            Icon = ImageConverter.convertBtmToBtmSource(Properties.Resources.logo);

            string FullName;
            using (var db = new ModelContainer1())
                FullName = (from u in db.UserSet where u.Login == login select u).First().FullName;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += delegate { while (true) { System.Threading.Tasks.Task.Delay(100); try { Dispatcher.Invoke(delegate { tblTime.Text = FullName + " " + System.DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"); }); } catch { bw.CancelAsync(); } } };
            bw.RunWorkerAsync();

            frMeters.Content = new UserWindowMetersTabPage(login);
            frAddMeter.Content = new UserWindowAddMeterTabPage(login, this);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((sender as TabControl)?.SelectedItem is TabItem)) return;

            switch (((sender as TabControl)?.SelectedItem as TabItem).Header)
            {
                case "Счётчики":
                    Width = 330;
                    Height = 360;
                    break;
                case "Добавить новый счётчик":
                    Width = 740;
                    Height = 650;
                    break;
                    default:
                        break;
            }
        }
    }
}
