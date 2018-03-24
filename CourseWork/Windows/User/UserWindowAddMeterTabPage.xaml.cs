using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для UserWindowAddMeterTabPage.xaml
    /// </summary>
    public partial class UserWindowAddMeterTabPage : Page
    {
        UserWindow owner;
        string login;

        public UserWindowAddMeterTabPage(string login, UserWindow parent)
        {
            InitializeComponent();

            owner = parent;

            InitializeFields();

            this.login = login;
        }

        Meter NewMeter;

        private void InitializeFields()
        {
            NewMeter = new Meter();

            // очистка полей
            ClearFields();
        }

        // Чистка полей
        private void ClearFields()
        {
            using (var db = new ModelContainer1())
            {
                // Типы
                var req2 = from t in db.TypeSet select t;

                cbTypes.Items.Clear();
                foreach (var t in req2.ToList())
                    cbTypes.Items.Add(t);

                // Название, Заводской номер, Описание, Тариф Документы
                tbName.Text = "";
                tbProdNum.Text = "";
                tbDescription.Text = "";
                tblTariffs.Text = "";
                tblDocsCount.Text = "Кол-во: 0";

                // дата производства и дата следдующей проверки
                dpProdDate.SelectedDate = DateTime.Now;
                dpProdDate.DisplayDateStart = new DateTime(1900, 1, 1);
                dpProdDate.DisplayDateEnd = DateTime.Now;

                // параметры
                var req3 = from p in db.ParametrSet select p;

                lbParametersMeter.Items.Clear();
                lbParametersAvailable.Items.Clear();
                foreach (var p in req3.ToList())
                    lbParametersAvailable.Items.Add(p);
            }

        }

        // Параметры <-
        private void ParAddToMeterClick(object sender, RoutedEventArgs e)
        {
            List<Parametr> list = new List<Parametr>(lbParametersAvailable.SelectedItems.Cast<Parametr>());
            foreach (var o in list)
            {
                lbParametersAvailable.Items.Remove(o);
                lbParametersMeter.Items.Add(o);
            }
        }

        // Параметры ->
        private void ParDelFromMeter_OnClick(object sender, RoutedEventArgs e)
        {
            List<Parametr> list = new List<Parametr>(lbParametersMeter.SelectedItems.Cast<Parametr>());
            foreach (var o in list)
            {
                lbParametersMeter.Items.Remove(o);
                lbParametersAvailable.Items.Add(o);
            }
        }

        // Параметры +
        private void AddNewPar_Click(object sender, RoutedEventArgs e)
        {
            AddNewParameter win = new AddNewParameter();
            win.ShowDialog();

            if (win.Parametr == null) return;

            using (var db = new ModelContainer1())
            {
                lbParametersMeter.Items.Add(win.Parametr);

                db.ParametrSet.Add(win.Parametr);
                db.SaveChanges();
            }
        }

        // выбрать Тариф
        private void ChooseTariff_Click(object sender, RoutedEventArgs e)
        {
            ChooseTariff win = new ChooseTariff(NewMeter.Tariff, NewMeter.Capacity, NewMeter.Readings.ToArray()) {Owner = owner };
            win.ShowDialog();

            // нажат крестик
            if (win.Tariff == null) return;

            tblTariffs.Text = win.Tariff.Name;

            NewMeter.Tariff = win.Tariff;

            NewMeter.Capacity = win.Capacity;

            int count = 1;
            NewMeter.Readings.Clear();
            double sum = 0;
            foreach (var r in win.Readings)
            {
                sum += r;

                Reading red = new Reading() { Meter = NewMeter, TariffNumber = count++, Value = r };

                NewMeter.Readings.Add(red);
            }

            NewMeter.SumReadings = sum;
        }

        // выбрать Документы
        private void AddDocuments_Click(object sender, RoutedEventArgs e)
        {
            AddDocuments win = new AddDocuments(NewMeter.Documents.ToList()) { Owner = owner };
            win.ShowDialog();

            tblDocsCount.Text = "Кол-во: " + win.Documents.Count;

            NewMeter.Documents.Clear();

            foreach (var doc in win.Documents)
            {
                doc.Meter = NewMeter;
                NewMeter.Documents.Add(doc);
            }
        }

        // Заводской номер фильтр текста
        private void tbProdNum_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Заводской номер фильтр кнопок
        private void tbProdNum_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        // Проверка всех полей
        private bool CheckAllFields()
        {
            if (cbTypes.SelectedIndex == -1)
                return Error.Show("Не выбран тип счётчика", "Ошибка выбора");

            if (tbName.Text == "")
                return Error.Show("Название не заполнено", "Ошибка ввода");

            if (tbProdNum.Text == "")
                return Error.Show("Номер счётчика не заполнен", "Ошибка ввода");

            long prodID;
            if (!long.TryParse(tbProdNum.Text, out prodID))
                return Error.Show("Заводской номер должен быть целым числом", "Ошибка ввода");

            if (tblTariffs.Text == "")
                return Error.Show("Не выбран Тариф", "Ошибка выбора");

            if (dpProdDate.SelectedDate < new DateTime(1900, 1, 1) || dpProdDate.SelectedDate > DateTime.Now)
                return Error.Show("Дата производства должна быть не меньше чем " + new DateTime(1900, 1, 1).ToString("d") + " и не больше чем " + DateTime.Now.ToString("d"), "Ошибка ввода");

            if (tbDescription.Text == "")
                return Error.Show("Описание не заполнено", "Ошибка ввода");

            using (var db = new ModelContainer1())
                if ((from m in db.MeterSet where m.ProductionId == prodID select m).Any())
                    return Error.Show("Счётчик с таким номером уже существует", "Ошибка ввода");

            return true;
        }

        // Зарегестрировать счётчик
        private void bAddMeter_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAllFields()) return;
            using (var db = new ModelContainer1())
            {
                // User
                NewMeter.User = (from u in db.UserSet where u.Login == this.login select u).AsParallel().First();
                NewMeter.User.Meters.Add(NewMeter);
                // Name
                NewMeter.Name = tbName.Text;
                // ProdID
                NewMeter.ProductionId = long.Parse(tbProdNum.Text);
                // Type
                Type ty = cbTypes.SelectionBoxItem as Type;
                NewMeter.Type = (from t in db.TypeSet where t.Id == ty.Id select t).AsParallel().First();
                (from t in db.TypeSet where t.Id == ty.Id select t).AsParallel().First().Meters.Add(NewMeter);

                // Tariff
                NewMeter.Tariff = (from t in db.TariffSet where t.Id == NewMeter.Tariff.Id select t).AsParallel().First();
                // Discription
                NewMeter.Discription = tbDescription.Text;
                // ProdDate
                NewMeter.ProductionDate = dpProdDate.DisplayDate;
                // Parameters
                NewMeter.Parametrs.Clear();
                foreach (var par in lbParametersMeter.Items.Cast<Parametr>())
                    NewMeter.Parametrs.Add((from p in db.ParametrSet where par.Id == p.Id select p).AsParallel().First());
                // Readings
                foreach (var red in NewMeter.Readings)
                    db.ReadingSet.Add(red);
                // Documents
                foreach (var doc in NewMeter.Documents)
                    db.DocumentSet.Add(doc);
                // NewMeter
                db.MeterSet.Add(NewMeter);

                db.SaveChanges();
            }

            MessageBox.Show("Счётчик " + NewMeter.Name + " зарегестрирован");
        }
    }
}
