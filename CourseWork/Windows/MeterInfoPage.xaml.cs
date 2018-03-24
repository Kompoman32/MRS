using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для MeterInfoPage.xaml
    /// </summary>
    public partial class MeterInfoPage : Page
    {
        public MeterInfoPage(Meter meter)
        {
            InitializeComponent();

            FillFields(meter);

            bListToExcel.Content =
                new Image() {Source = ImageConverter.convertBtmToBtmSource(Properties.Resources.excel)};
        }

        void FillFields(Meter meter)
        {
            using (var db = new ModelContainer1())
            {
                tblName.Text = meter.Name;

                tblProdID.Text = meter.ProductionId.ToString();
                tblProductionDate.Text = meter.ProductionDate.ToString("d");

                tblInstallDate.Text = meter is InstalledMeter
                    ? (meter as InstalledMeter).InstallDate.ToString("d")
                    : " Не установлен";
                tblExpiracyDate.Text = meter is InstalledMeter
                    ? (meter as InstalledMeter).ExpirationDate.ToString("d")
                    : " Не установлен";

                tblType.Text = (from m in db.MeterSet where m.ProductionId == meter.ProductionId select m.Type).First()
                    .Name;
                tblTariff.Text = (from m in db.MeterSet where m.ProductionId == meter.ProductionId select m.Tariff)
                    .First().Name;

                tblReadings.Text = meter.SumReadings.ToString() + " " +
                                   (from m in db.MeterSet where m.ProductionId == meter.ProductionId select m.Type)
                                   .First().Unit;
                tblCapacity.Text = meter.Capacity.ToString();

                FillReading(meter);
                FillParameters(meter);
                FillDocuments(meter);

                tblDiscription.Text = meter.Discription;

                if (!(meter is InstalledMeter))
                    tblWarning.Text = "Счётчик не установлен";
                else if ((meter as InstalledMeter).ExpirationDate <= DateTime.Now)
                    tblWarning.Text = "Требуется вызвать мастера для продления";
            }
        }

        void FillReading(Meter meter)
        {
            using (var db = new ModelContainer1())
            {
                List<Reading> list =
                (from r in db.ReadingSet
                    where r.Meter.ProductionId == meter.ProductionId
                    orderby r.TariffNumber
                    select r).AsParallel().ToList();

                wpReading.Children.Clear();

                int count = 1;

                foreach (var r in list)
                {
                    wpReading.Children.Add(new TextBlock()
                    {
                        Text = "Показатель " + count++ + ":",
                        Width = 175,
                        TextAlignment = TextAlignment.Right
                    });

                    wpReading.Children.Add(new TextBlock()
                    {
                        Text = r.ToString(),
                        Width = 175,
                        TextAlignment = TextAlignment.Center,
                        Foreground = tblName.Foreground
                    });

                }
            }
        }

        void FillParameters(Meter meter)
        {
            lvParameters.Items.Clear();

            using (var db = new ModelContainer1())
            {
                List<Parametr> parList =
                    (from m in db.MeterSet where m.ProductionId == meter.ProductionId select m.Parametrs).First()
                    .ToList();


                foreach (var par in parList)
                {
                    TextBlock t = new TextBlock()
                    {
                        Text = par.ToString(),
                        TextWrapping = TextWrapping.Wrap,
                        Width = 300
                    };
                    t.MouseLeftButtonUp += delegate
                    {
                        MessageBox.Show("Название:\t\t" + par.Name + "\nЗначение:\t" + par.Measure, "Параметр");
                    };
                    lvParameters.Items.Add(t);
                }
            }
        }

        void FillDocuments(Meter meter)
        {
            lvDocuments.Items.Clear();

            using (var db = new ModelContainer1())
            {
                List<Document> docList =
                    (from m in db.MeterSet where m.ProductionId == meter.ProductionId select m.Documents).First()
                    .ToList();

                foreach (var doc in docList)
                {
                    TextBlock t = new TextBlock()
                    {
                        Text = doc.ToString(),
                        TextWrapping = TextWrapping.Wrap,
                        Width = 300
                    };
                    t.MouseLeftButtonUp += delegate
                    {
                        MessageBox.Show(
                            doc.Title + "\n\n" + doc.Discription + "\n\n" + "Дата подписания:\t" +
                            doc.SigningDate.ToString("d"),
                            "Документ");
                    };

                    lvDocuments.Items.Add(t);
                }
            }
        }

        // Выгрузка в Эксель
        private DataGrid[] GetDataGrids()
        {
            DataGrid[] dgMas= new DataGrid[5];

            using (var db = new ModelContainer1())
            {
                
                long prodId = long.Parse(tblProdID.Text);

                // Счётчик
                Meter met = (from m in db.MeterSet where m.ProductionId == prodId select m).AsParallel().First();
                if (met is InstalledMeter)
                {
                    var t =
                        new
                        {
                            Заводской_Номер = met.ProductionId,
                            Название = met.Name,
                            Описание = met.Discription,
                            Название_Тарифа = met.Tariff.Name,
                            Сумма_Показаний = met.SumReadings,
                            Вместимость = met.Capacity,
                            Дата_производства = met.ProductionDate.ToString("d"),
                            Дата_Установки = (met as InstalledMeter).InstallDate,
                            Дата_Следующей_Проверки = (met as InstalledMeter).ExpirationDate,
                            Комментарий = (met as InstalledMeter).ExpirationDate <= DateTime.Now? "Трубется продление счётчика.": ""
                        };

                    dgMas[0]= new DataGrid(){ItemsSource = new object[] {t}.ToList(), Name = "Установленный_счётчик"};
                    //new Window() {Content = dgMas[0]}.ShowDialog();
                }
                else
                {
                    var t =
                        new
                        {
                            Заводской_Номер = met.ProductionId,
                            Название = met.Name,
                            Описание = met.Discription,
                            Тип_счётчика = met.Type.ToString(),
                            Название_Тарифа = met.Tariff.Name,
                            Сумма_Показаний = met.SumReadings,
                            Вместимость = met.Capacity,
                            Дата_производства = met.ProductionDate.ToString("d"),
                            Комментарий = "Счётчик не установлен."
                        };

                    dgMas[0] = new DataGrid() { ItemsSource = new object[] { t }.ToList(), Name = "Счётчик" };
                    //new Window() { Content = dgMas[0] }.ShowDialog();
                }

                // Показания
                {
                    var t = (from r in met.Readings
                        orderby r.TariffNumber
                        select new {Значение = r.Value, Номер_Тарифа = r.TariffNumber}).AsParallel();

                    dgMas[1] = new DataGrid(){ItemsSource = t.ToList(), Name = "Показания"};
                    //new Window() { Content = dgMas[1] }.ShowDialog();
                }

                // Документы
                {
                    var t = (from d in met.Documents
                        select new {Дата_подписания = d.SigningDate, Заголовок = d.Title, Описание = d.Discription});

                    dgMas[2] = new DataGrid() { ItemsSource = t.ToList(), Name = "Документы" };
                    //new Window() { Content = dgMas[2] }.ShowDialog();
                }

                // Параметры
                {
                    var t = (from p in met.Parametrs
                        select new {Название = p.Name, Значение = p.Measure});

                    dgMas[3] = new DataGrid() { ItemsSource = t.ToList(), Name = "Параметры" };
                    //new Window() { Content = dgMas[3] }.ShowDialog();
                }

                // Временные отрезки
                {
                    var t = (from ts in db.TimeSpanSet
                        where ts.Tariff.Id == met.Tariff.Id
                        orderby ts.TimeStart
                        select new { Название = ts.Name, Время_начала = ts.TimeStart, Время_окончания = ts.TimeEnd });

                    //string n = (from m in db.MeterSet where m.ProductionId == prodId select m.Tariff.Name).AsParallel().First().Replace(" ","_");
                    string n = met.Tariff.Name;
                    n = "Промежутки_тарифа_" + n;
                    dgMas[4] = new DataGrid() { ItemsSource = t.ToList(), Name = n };
                    //new Window() { Content = dgMas[4] }.ShowDialog();
                }

                return dgMas;
            }
        }
        
        private void bToExcel_Click(object sender, RoutedEventArgs e)
        {
            Export.Excel(GetDataGrids());
        }
    }
}
