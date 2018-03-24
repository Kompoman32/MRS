using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для AdminChooseTariff.xaml
    /// </summary>
    public partial class ChooseTariff : Window
    {
        public ChooseTariff(Tariff tariff , double capacity, Reading[] readings)
        {
            InitializeComponent();

            Icon = ImageConverter.convertBtmToBtmSource(Properties.Resources.logo);

            // Тарифы -> комбобокс
            using (var db = new  ModelContainer1())
            {
                var req=from t in db.TariffSet select t;
                List<Tariff> list = req.ToList();
                cbTariff.Items.Clear();
                foreach (var t in list)
                    cbTariff.Items.Add(t);
            }

            // вставка значений
            if (tariff != null)
            {
                cbTariff.SelectedIndex = tariff.Id - 1;

                NumAftComma = capacity.ToString().TrimStart('9').TrimStart(',').Length;
                
                NumBefComma = capacity.ToString().IndexOf(",")>-1? capacity.ToString().TrimEnd('9').TrimEnd(',').Length: capacity.ToString().Length;

                for (int i = 1; i < wpReadings.Children.Count; i += 2)
                    ((TextBox) wpReadings.Children[i]).Text=readings[i/2].Value.ToString();
            }
        }

        const int MaxNum = 7;

        int numBefComma = 1;
        private int NumBefComma
        {
            get => numBefComma;
            set
            {
                numBefComma = value > MaxNum ? MaxNum : value < 1 ? 1 : value;
                tbBefComma.Text = numBefComma.ToString();
                UpdateCapacity();
            }
        }

        private void bBefComNumUp_OnClick(object sender, RoutedEventArgs e)
        {
            NumBefComma++;
        }
        private void bBefComNumDown_OnClick(object sender, RoutedEventArgs e)
        {
            NumBefComma--;
        }

        int numAftComma = 0;
        private int NumAftComma
        {
            get => numAftComma;
            set
            {
                numAftComma = value > MaxNum ? MaxNum : value < 0 ? 0 : value;
                tbAftComma.Text = numAftComma.ToString();
                UpdateCapacity();
            }
        }

        private void bAftComNumUp_OnClick(object sender, RoutedEventArgs e)
        {
            NumAftComma++;
        }
        private void bAftComNumDown_OnClick(object sender, RoutedEventArgs e)
        {
            NumAftComma--;
        }

        private void UpdateCapacity()
        {
            string s = ",";
            for (int i = 0; i < numBefComma; i++)
                s = "9" + s;
            for (int i = 0; i < numAftComma; i++)
                s += "9";
            tblCapacity.Text = s.Trim(',');
        }

        // Задаёт количество текстбоксов для показателей
        void UpdateReadingsPanel(int index)
        {
            wpReadings.Children.Clear();

            for (int i = 1; i <= index+1; i++)
            {
                wpReadings.Children.Add(
                    new TextBlock() {Width = 120, Height = 25, Text = "Показатель " + i + ": ", TextAlignment = TextAlignment.Right});

                TextBox t = new TextBox()
                {
                    Width = 150,
                    Height = 25,
                    ContextMenu = null
                };

                t.PreviewTextInput += tbOnlyDouble_OnPreviewTextInput;
                t.PreviewKeyDown += tbonlyDouble_OnPreviewKeyDown;
                //t.LostFocus += tb_OnLostFocus;

                wpReadings.Children.Add(t);
            }
        }

        private void cbTariff_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateReadingsPanel(cbTariff.Items.IndexOf(e.AddedItems[0]));
        }

        // BUG: Работает ctrl+v
        private void tbOnlyDouble_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9,]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void tbonlyDouble_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }


        /*
        private void tb_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;

            if (t == null) return;

            t.Text = t.Text.TrimEnd(',');
            //t.Text = t.Text == "" ? "0" : t.Text;

            double num;
            if (!double.TryParse(t.Text, out num))
            {
                Error.Show("Ошибка, введено не число", "Ошибка ввода");
                return;
            }

            if (num >= Math.Floor(double.Parse(tblCapacity.Text) + 1))
                Error.Show("Ошибка, введённое число больше размерности", "Ошибка ввода");
            
        }*/

        public List<double> Readings=new List<double>();
        public double Capacity = 0;
        public Tariff Tariff = null;

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            // Проверить все поля
            if (!ChecAllFields()) return;

            Capacity = double.Parse(tblCapacity.Text);
            Tariff = cbTariff.SelectionBoxItem as Tariff;
            for (int i = 1; i < wpReadings.Children.Count; i+=2)
                Readings.Add(Math.Round(double.Parse((wpReadings.Children[i] as TextBox)?.Text),numAftComma));

            Close();
        }

        private bool ChecAllFields()
        {
            if (cbTariff.SelectedIndex == -1)
                return Error.Show("Ошибка, не выбран тариф", "Нет тарифа");
            

            for (int i = 1; i < wpReadings.Children.Count; i += 2)
            {
                TextBox t = wpReadings.Children[i] as TextBox;

                double num;
                if (!double.TryParse(t?.Text, out num))
                    return Error.Show("Одно из полей не заполнено, либо заполнено неправильно", "Ошибка ввода");
                

                if (num > Math.Floor(double.Parse(tblCapacity.Text) + 1))
                    return Error.Show("Ошибка, введённое число больше размерности", "Ошибка ввода");
                
            }

            return true;
        }
    }
}
