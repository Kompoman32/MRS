using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для AdminWindowExtendMeterTabPage.xaml
    /// </summary>
    public partial class AdminWindowExtendMeterTabPage : Page
    {
        public AdminWindowExtendMeterTabPage()
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

                dpExpiracyDate.SelectedDate=DateTime.Now;
                dpExpiracyDate.DisplayDateStart = DateTime.Now;

            }
        }

        private void cbUsers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            User u = e.AddedItems[0] as User;

            using (var db = new ModelContainer1())
            {
                List<Meter> metList = (from m in db.MeterSet where m.User.Login == u.Login && m is InstalledMeter select m).ToList();


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

        // продлить
        private void bExtend_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckAllFields()) return;

            using (var db = new ModelContainer1())
            {
                Meter met = cbMeters.SelectionBoxItem as Meter;

                InstalledMeter inMet =
                ((from m in db.MeterSet
                        where m.ProductionId == met.ProductionId
                        select m).AsParallel().First() as InstalledMeter);

                inMet.ExpirationDate = (DateTime) dpExpiracyDate.SelectedDate;

                db.SaveChanges();

                int index = cbMeters.SelectedIndex;

                cbMeters.Items[index] = 
                   (from m in db.MeterSet
                    where m.ProductionId == met.ProductionId
                    select m).AsParallel().First();

                cbMeters.SelectedIndex = index;

                MessageBox.Show("Счётчик " + met.Name + " продлён до " + inMet.ExpirationDate.ToString("d"));
            }
        }

        // поверка полей
        private bool CheckAllFields()
        {
            if (cbUsers.SelectedIndex == -1)
                return Error.Show("Не выбран пользователь", "Ошибка выбора");

            if (cbMeters.SelectedIndex == -1)
                return Error.Show("Не выбран счётчик", "Ошибка выбора");

            if (!(cbMeters.SelectionBoxItem is InstalledMeter))
                return Error.Show("Счётчик не установлен", "Ошибка выбора");

            using (var db = new ModelContainer1())
            {
                Meter met = (Meter) cbMeters.SelectionBoxItem;
                InstalledMeter inMet = (from m in db.MeterSet where met.ProductionId == m.ProductionId select m).AsParallel().First() as InstalledMeter;
            

                if (dpExpiracyDate.SelectedDate < inMet.ExpirationDate || dpExpiracyDate.SelectedDate > DateTime.MaxValue)
                    return Error.Show("Дата подписания должна быть не меньше чем " + inMet.ExpirationDate.ToString("d") + " и не больше чем " + DateTime.MaxValue.ToString("d"), "Ошибка ввода");
            }
            return true;
        }
    }
}
