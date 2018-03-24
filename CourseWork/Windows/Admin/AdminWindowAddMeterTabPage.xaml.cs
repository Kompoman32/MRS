using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для AdminWindowAddMeterTabPage.xaml
    /// </summary>
    public partial class AdminWindowAddMeterTabPage : Page
    {
        AdminWindow owner;

        Meter NewMeter;

        public AdminWindowAddMeterTabPage(AdminWindow parent)
        {
            InitializeComponent();
            owner = parent;
            InitializeFields();
        }

        private void InitializeFields()
        {
            NewMeter = new InstalledMeter();

            using (var db = new ModelContainer1())
            {
                // Пользователи (не админы) combobox
                var req = from u in db.UserSet where !u.AdminPrivileges select u;

                cbUsers.Items.Clear();
                foreach (var u in req.ToList())
                    cbUsers.Items.Add(u);

                // очистка полей
                ClearFields();
            }
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

                dpNextCheck.SelectedDate = DateTime.Now;
                dpNextCheck.DisplayDateStart = DateTime.Now;

                // параметры
                var req3 = from p in db.ParametrSet select p;

                lbParametersMeter.Items.Clear();
                lbParametersAvailable.Items.Clear();
                foreach (var p in req3.ToList())
                    lbParametersAvailable.Items.Add(p);
            }

        }

        // combobox Пользователь
        private void CbUsers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUnMetersComBox((e.AddedItems[0] as User)?.Login);
            ClearFields();
        }

        // обновление combobox неустановленный счётчик
        private void UpdateUnMetersComBox(string usrLogin)
        {
            //Если не выбран никакой пользователь
            if (cbUsers.SelectedIndex == -1) return;

            NewMeter = new InstalledMeter();

            using (var db = new ModelContainer1())
            {
                // неустановленные счётчики
                cbUninstMeters.Items.Clear();
                cbUninstMeters.Items.Add("<очистить поля>");
                var req = from m in (from u in db.UserSet where u.Login == usrLogin select u).First().Meters
                    where !(m is InstalledMeter)
                    select m;

                foreach (var m in req.ToList())
                    cbUninstMeters.Items.Add(m);
            }
        }

        // combobox неустановленный счётчик
        private void cbUninstMeters_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
                if (e.AddedItems[0] is Meter)
                    PasteMeter(e.AddedItems[0] as Meter);
                else ClearFields();
        }

        void PasteMeter(Meter meter)
        {
            ClearFields();

            using (var db = new ModelContainer1())
            {
                meter = (from m in db.MeterSet where m.ProductionId == meter.ProductionId select m).AsParallel()
                    .First();

                cbTypes.SelectedIndex = (from t in db.TypeSet where t.Name == meter.Type.Name select t).AsParallel().First().Id - 1;
                tbName.Text = meter.Name;
                tbDescription.Text = meter.Discription;
                tbProdNum.Text = meter.ProductionId.ToString();
                dpProdDate.SelectedDate = meter.ProductionDate;
                NewMeter.Tariff = meter.Tariff;
                tblTariffs.Text = meter.Tariff.Name;
                NewMeter.Capacity = meter.Capacity;

                // Readings
                int count = 1;
                NewMeter.Readings.Clear();
                double sum = 0;
                foreach (var r in from m in meter.Readings select m.Value)
                {
                    sum += r;

                    Reading red = new Reading() { Meter = NewMeter, TariffNumber = count++, Value = r };

                    NewMeter.Readings.Add(red);
                }

                NewMeter.SumReadings = sum;

                // Documents
                NewMeter.Documents.Clear();
                foreach (var doc in meter.Documents)
                {
                    doc.Meter = NewMeter;
                    NewMeter.Documents.Add(doc);
                }

                tblDocsCount.Text = "Кол-во: " + meter.Documents.Count;

                // Parameters
                lbParametersAvailable.Items.Clear();
                List<int> list = (from p in (from m in db.MeterSet where m.ProductionId == meter.ProductionId select m).AsParallel().First().Parametrs select p.Id).AsParallel().ToList();
                List<Parametr> list2 = (from par in db.ParametrSet where !list.Contains(par.Id) select par).ToList();
                foreach (var parameter in list2)
                    lbParametersAvailable.Items.Add(parameter);

                foreach (var par in meter.Parametrs)
                    lbParametersMeter.Items.Add(par);

            }

        }

        // Параметры <-
        private void ParAddToMeterAmClick(object sender, RoutedEventArgs e)
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
                Parametr par = win.Parametr;

                db.ParametrSet.Add(par);

                lbParametersMeter.Items.Add(par);

                db.SaveChanges();
            }
        }

        // выбрать Тариф
        private void ChooseTariff_Click(object sender, RoutedEventArgs e)
        {
            ChooseTariff win = new ChooseTariff(NewMeter.Tariff, NewMeter.Capacity, NewMeter.Readings.ToArray()) {Owner = owner};
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
            AddDocuments win = new AddDocuments(NewMeter.Documents.ToList()) {Owner = owner};
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
            if (cbUsers.SelectedIndex == -1)
                return Error.Show("Не выбран пользователь", "Ошибка выбора");

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

            if (dpNextCheck.SelectedDate < DateTime.Now || dpNextCheck.SelectedDate > DateTime.MaxValue)
                return Error.Show("Дата следующей проверки должна быть не меньше чем " + DateTime.Now.ToString("d") + " и не больше чем " + DateTime.MaxValue.ToString("d"), "Ошибка ввода");

            if (tbDescription.Text == "")
                return Error.Show("Описание не заполнено", "Ошибка ввода");

            using (var db = new ModelContainer1())
            {
                if ((from m in db.MeterSet where m.ProductionId == prodID && m is InstalledMeter select m).Any())
                    return Error.Show("Счётчик с таким номером уже существует", "Ошибка ввода");

                var req = from m in db.MeterSet where m.ProductionId == prodID select m;

                if (req.Any())
                    AdminWindowDelMeterTabPage.DeleteMeter(req.First(), db); 
            }
            
            return true;
        }

        // Зарегестрировать счётчик
        private void bAddMeter_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAllFields()) return;
            using (var db = new ModelContainer1())
            {
                // User
                User us = (cbUsers.SelectionBoxItem as User);
                NewMeter.User = (from u in db.UserSet where u.Login == us.Login select u).AsParallel().First();
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
                // InstallDate
                ((InstalledMeter)NewMeter).InstallDate = DateTime.Now;
                // ExpirationDate
                ((InstalledMeter)NewMeter).ExpirationDate = dpNextCheck.DisplayDate;
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
                // Meter

                db.MeterSet.Add(NewMeter);

                db.SaveChanges();
            }

            MessageBox.Show("Счётчик " + NewMeter.Name + " зарегестрирован");
            owner.frDelMeter.Content=new AdminWindowDelMeterTabPage();
            owner.frExtendMeter.Content=new AdminWindowExtendMeterTabPage();
        }
    }
}
