using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для AddWindowDelMeterTabPage.xaml
    /// </summary>
    public partial class AdminWindowDelMeterTabPage : Page
    {
        public AdminWindowDelMeterTabPage()
        {
            InitializeComponent();

            InitializeFields();
        }

        void InitializeFields()
        {
            using (var db = new ModelContainer1())
            {
                List<User> list = (from u in db.UserSet where !u.AdminPrivileges select u).ToList();

                cbUsers.Items.Clear();

                foreach (var u in list)
                    cbUsers.Items.Add(u);
                
            }
        }

        private void cbUsers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count==0) return;

            UpdateComboboxMeter(e.AddedItems[0] as User);
        }

        private void UpdateComboboxMeter(User user)
        {
            using (var db = new ModelContainer1())
            {
                List<Meter> metList = (from m in db.MeterSet where m.User.Login == user.Login select m).ToList();
                
                cbMeters.Items.Clear();

                foreach (var m in metList)
                    cbMeters.Items.Add(m);
            }

            frMeterInfo.Content = new TextBlock() { Text = "Выберите пользователя и счётчик", FontSize = 40, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        }

        private void cbMeters_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            frMeterInfo.Content = new MeterInfoPage(e.AddedItems[0] as Meter);
        }

        // 
        private void bDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckAllFields()) return;


            Meter met = cbMeters.SelectedItem as Meter;

            MessageBoxResult mbRes = MessageBox.Show("Действительно удалить " + met.Name + " ?", "Удалить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (mbRes == MessageBoxResult.Cancel || mbRes == MessageBoxResult.No)
                return;

            // Удалить счётчик
            using (var db = new ModelContainer1())
                DeleteMeter(met, db);

            MessageBox.Show("Счётчик "+met.Name + " удалён :)");

            UpdateComboboxMeter(cbUsers.SelectionBoxItem as User);
        }

        // Удаление
        public static void DeleteMeter(Meter met, ModelContainer1 db)
        {
            // Показатели
            db.ReadingSet.RemoveRange(from r in db.ReadingSet
                where met.ProductionId == r.Meter.ProductionId
                select r);
            // Документы
            db.DocumentSet.RemoveRange((from m in db.MeterSet
                where m.ProductionId == met.ProductionId
                select m).AsParallel().First().Documents);
            // поиск счётчика
            met = (from m in db.MeterSet
                where m.ProductionId == met.ProductionId
                select m).AsParallel().First();
            // разрыв связи между параметром и счётчиком
            foreach (var p in met.Parametrs)
                p.Meters.Remove(met);

            db.MeterSet.Remove(met);

            db.SaveChanges();
        }

        // Проверка полей
        private bool CheckAllFields()
        {
            if (cbUsers.SelectedIndex == -1)
                return Error.Show("Не выбран пользователь", "Ошибка выбора");

            if (cbMeters.SelectedIndex == -1)
                return Error.Show("Не выбран счётчик", "Ошибка выбора");

            return true;
        }
    }
}
