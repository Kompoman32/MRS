using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для AdminWindowSearchTabPage.xaml
    /// </summary>
    public partial class AdminWindowSearchTabPage : Page
    {
        public AdminWindowSearchTabPage()
        {
            InitializeComponent();

            InitializeFields();
        }

        class Entity
        {
            public string Name;
            public string[] Parameters;

            public Entity(string name, params string[] parametrs)
            {
                Name = name;
                Parameters = parametrs;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private void InitializeFields()
        {
            bListToExcel.Content = new Image() { Source = ImageConverter.convertBtmToBtmSource(Properties.Resources.excel) };
            
            spParams.Children.Clear();
            cbEntity.Items.Clear();

            cbEntity.Items.Add(new Entity("Счётчик", "Название", "Описание", "Сумма показаний", "Вместимость","Заводской номер", "Дата производства"));
            cbEntity.Items.Add(new Entity("Установленный счётчик", "Заводской номер", "Дата установки","Дата следующей проверки"));
            cbEntity.Items.Add(new Entity("Пользователь", "Логин", "Полное имя", "Привилегии админа"));
            cbEntity.Items.Add(new Entity("Тип", "Название", "Ед. измерения"));
            cbEntity.Items.Add(new Entity("Параметр", "Название", "Значение"));
            cbEntity.Items.Add(new Entity("Тариф", "Название"));
            cbEntity.Items.Add(new Entity("Временной отрезок", "Название", "Время начала", "Время окончания"));
            cbEntity.Items.Add(new Entity("Показатель", "Значение", "Номер тарифа"));
            cbEntity.Items.Add(new Entity("Документ", "Заголовок", "Описание", "Дата подписания"));
        }

        // количество параметров изменяется
        private void cbEntityCount_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            int count = (int)e.AddedItems[0];
            Entity en = cbEntity.SelectionBoxItem as Entity;

            // стак панель для всех
            spParams.Children.Clear();

            for (int i = 0; i < count; i++)
            {
                // стак панель для каждого отдельно
                StackPanel sp = new StackPanel() ;
                sp.Children.Add(new TextBlock() {Text = "Показатель " + (i + 1)});

                // для параметра и условия
                WrapPanel wp = new WrapPanel();

                // выбор параметра
                ComboBox cb = new ComboBox() {Margin = new Thickness(0, 10, 0, 10)};

                foreach (var p in from p in en.Parameters select p)
                {
                    cb.Items.Add(p);
                }

                cb.SelectionChanged += delegate(object o, SelectionChangedEventArgs args)
                {
                    if (args.AddedItems.Count == 0) return;


                    wp.Children.Clear();

                    wp.Children.Add(new ComboBox() {Width = 100, Items = {"==",">", "<", ">=", "<=", "!="}, SelectedIndex = 0});

                    wp.Children.Add(new TextBox() {Width = 180});
                };

                sp.Children.Add(cb);

                sp.Children.Add(wp);

                spParams.Children.Add(sp);
            }
        }

        private void cbEntity_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            tblEntityCount.Visibility = Visibility.Visible;
            cbEntityCount.Visibility = Visibility.Visible;
            spParams.Children.Clear();
            cbEntityCount.Items.Clear();
            for (int i = 0; i < (e.AddedItems[0] as Entity).Parameters.Length; i++)
                cbEntityCount.Items.Add(i+1);
            
        }

        // Проверка полей
        private bool CheckFields()
        {
            if (cbEntity.SelectedIndex == -1)
                return Error.Show("Не выбрана сущность", "Ошибка выбора");

            if (cbEntityCount.SelectedIndex == -1)
                return Error.Show("Не выбрано количество параметров", "Ошибка выбора");
            

            List<string> parameters = (cbEntity.SelectionBoxItem as Entity).Parameters.ToList();

            foreach (var child in spParams.Children )
            {
                StackPanel spParam = child as StackPanel;

                string numParam =
                    (spParam.Children[0] as TextBlock).Text.Substring((spParam.Children[0] as TextBlock).Text.IndexOf(' '));

                ComboBox cbParamName = spParam.Children[1] as ComboBox;

                if (cbParamName == null) return false;

                if (cbParamName.SelectedIndex == -1)
                    return Error.Show("Не выбран параметр "+ numParam, "Ошибка выбора");

                string parName = cbParamName.SelectionBoxItem as string;

                if (!parameters.Contains(parName))
                    return Error.Show("Параметр"+ parName + " выбран несколько раз", "Ошибка выбора");

                parameters.Remove(parName);

                TextBox tb =(spParam.Children[2] as WrapPanel).Children[1] as TextBox;

                if (tb.Text == "")
                    return Error.Show("Параметр"+numParam+" не заполнен", "Ошибка ввода");

                if (parName.ToLower().Contains("дата"))
                    if (!DateTime.TryParse(tb.Text, out DateTime _))
                        return Error.Show("Параметр" + numParam + " должен иметь формат вида 01/01/2018","Ошибка ввода");

                if (parName == "Сумма показаний" || parName == "Вместимость" )
                    if (!double.TryParse(tb.Text, out double _))
                        return Error.Show("Параметр" + numParam + " должен быть число с плавающей точкой","Ошибка ввода");

                if (parName.ToLower().Contains("номер"))
                    if (!long.TryParse(tb.Text, out long _))
                        return Error.Show("Параметр" + numParam + " должен быть целым число", "Ошибка ввода");

                if (parName.ToLower().Contains("время"))
                    if (!System.TimeSpan.TryParse(tb.Text, out System.TimeSpan _) || !tb.Text.Contains(":"))
                        return Error.Show("Параметр" + numParam + " должен иметь формат вида 00:00", "Ошибка ввода");

                if (parName == "Привилегии админа")
                    if (!bool.TryParse(tb.Text, out bool _))
                        return Error.Show("Параметр" + numParam + " должен быть true или false", "Ошибка ввода");
            }

            return true;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckFields()) return;

            DoSearch();
        }
        
        // Поиск
        private void DoSearch()
        {
            Entity en = cbEntity.SelectionBoxItem as Entity;
            
            //using (var db = new ModelContainer1())
            switch (en.Name)
            {
                case "Счётчик":
                {
                    List<Meter> list = MakeIntersection<Meter>();

                    using(var db = new ModelContainer1())
                    dgList.ItemsSource = (from m in list select new { Заводской_Номер = m.ProductionId, Пользователь = (from met in db.MeterSet where met.ProductionId == m.ProductionId select met).AsParallel().First().User.FullName, Название = m.Name, Описание = m.Discription, Сумма_Показаний = m.SumReadings, Вместимость = m.Capacity, Дата_производства = m.ProductionDate.ToString("d") });
                        
                }
                        break;
                case "Установленный счётчик":
                {
                    List<InstalledMeter> list = MakeIntersection<InstalledMeter>();

                    dgList.ItemsSource = (from m in list select new {Название = m.Name, Описание = m.Discription, Сумма_Показаний = m.SumReadings, Вместимость = m.Capacity, Заводской_Номер = m.ProductionId, Дата_производства = m.ProductionDate.ToString("d"), Дата_Установки = m.InstallDate, Дата_Следующей_Проверки = m.ExpirationDate });
                }
                    break;
                case "Пользователь":
                {
                    List<User> list = MakeIntersection<User>();
                    dgList.ItemsSource = (from u in list select new {ID = u.Id, Логин = u.Login, Полное_Имя = u.FullName, Админ = u.AdminPrivileges });
                }
                    break;
                case "Тип":
                {
                    List<Type> list = MakeIntersection<Type>();
                    dgList.ItemsSource = (from t in list select new { ID = t.Id, Название = t.Name, Еденица_Измерения = t.Unit });
                }
                    break;
                case "Параметр":
                {
                    List<Parametr> list = MakeIntersection<Parametr>();
                    dgList.ItemsSource = (from p in list select new { ID = p.Id, Название = p.Name, Значение = p.Measure });
                }
                    break;
                case "Тариф":
                {
                    List<Tariff> list = MakeIntersection<Tariff>();
                    dgList.ItemsSource = (from t in list select new { ID = t.Id, Название = t.Name });
                }
                    break;
                case "Временной отрезок":
                {
                    List<TimeSpan> list = MakeIntersection<TimeSpan>();
                    dgList.ItemsSource = (from t in list select new { ID = t.Id, Название = t.Name, Начало_Периода = t.TimeStart, Окончание_Периода = t.TimeEnd });
                }
                    break;
                case "Показатель":
                {
                    List<Reading> list = MakeIntersection<Reading>();
                    dgList.ItemsSource = (from r in list select new { ID = r.Id, Значение = r.Value, Номер_тарифа = r.TariffNumber });
                }
                    break;
                case "Документ":
                {
                    List<Document> list = MakeIntersection<Document>();
                    dgList.ItemsSource = (from d in list select new { ID = d.Id, Заголовок = d.Title, Описание = d.Discription, Дата_Подписания = d.SigningDate.ToString("d") });
                }
                    break;
                default:
                    break;
            }

            if (dgList.Items.Count==0)
                MessageBox.Show("Не найдено никаких элементов в базе данных");

        }

        // возвращает лист  - применение всех параметров  (пересечение листов)
        private List<T> MakeIntersection<T>()
        {
            int count = int.Parse(cbEntityCount.Text);

            List<string> parameters = new List<string>();
            List<string> conditions = new List<string>();
            List<string> values = new List<string>();

            foreach (var o in spParams.Children)
            {
                StackPanel sp = o as StackPanel;

                // название параметра
                parameters.Add((sp.Children[1] as ComboBox).Text);

                conditions.Add(((sp.Children[2] as WrapPanel).Children[0] as ComboBox).Text);

                values.Add(((sp.Children[2] as WrapPanel).Children[1] as TextBox).Text);
            }

            Entity en = cbEntity.SelectionBoxItem as Entity;

            IEnumerable<T> listT = null;

            using (var db = new ModelContainer1())
            {
                for (int i = 0; i < count; i++)
                {
                    int index = Array.IndexOf(en.Parameters, parameters[i]);
                    IEnumerable<T> tempListT = MakeList<T>(db, index, values[i], conditions[i]);

                    if (listT == null) listT = tempListT;
                    else
                    {
                        listT = listT.Intersect(tempListT).ToList();
                    }
                }

                return listT.ToList();
            }
        }

        // switch для типа листа на выходе (делает один лист)
        private IEnumerable<T> MakeList<T>(ModelContainer1 db,int index, string val, string cond)
        {
            if (typeof(T) == typeof(Meter))
                return MakeMeterList(db, index, val, cond) as IEnumerable<T>;

            if (typeof(T) == typeof(InstalledMeter))
                return MakeInstalledMeterList(db, index, val, cond) as IEnumerable<T>;

            if (typeof(T) == typeof(User))
                return MakeUserList(db, index, val, cond) as IEnumerable<T>;

            if (typeof(T) == typeof(Type))
                return MakeTypeList(db, index, val, cond) as IEnumerable<T>;

            if (typeof(T) == typeof(Parametr))
                return MakeParametrList(db, index, val, cond) as IEnumerable<T>;

            if (typeof(T) == typeof(Tariff))
                return MakeTariffList(db, index, val, cond) as IEnumerable<T>;

            if (typeof(T) == typeof(TimeSpan))
                return MakeTimeSpanList(db, index, val, cond) as IEnumerable<T>;
            
            if (typeof(T) == typeof(Reading))
                return MakeReadingList(db, index, val, cond) as IEnumerable<T>;

            if (typeof(T) == typeof(Document))
                return MakeDocumentList(db, index, val, cond) as IEnumerable<T>;

            return null;
        }

        // Делает листы ( LINQ запросы )
        private IEnumerable<Meter> MakeMeterList(ModelContainer1 db, int index, string val, string cond)
        {
            //using (var db = new ModelContainer1())
            {
                if (index == 0)
                {
                    switch (cond)
                    {
                        case "==": return (from m in db.MeterSet where m.Name.CompareTo(val) == 0 select m);
                        case  ">": return (from m in db.MeterSet where m.Name.CompareTo(val)  > 0 select m);
                        case  "<": return (from m in db.MeterSet where m.Name.CompareTo(val)  < 0 select m);
                        case ">=": return (from m in db.MeterSet where m.Name.CompareTo(val) >= 0 select m);
                        case "<=": return (from m in db.MeterSet where m.Name.CompareTo(val) <= 0 select m);
                        case "!=": return (from m in db.MeterSet where m.Name.CompareTo(val) != 0 select m);
                    }
                }

                if (index == 1)
                {
                    switch (cond)
                    {
                        case "==": return (from m in db.MeterSet where m.Discription.CompareTo(val) == 0 select m);
                        case  ">": return (from m in db.MeterSet where m.Discription.CompareTo(val)  > 0 select m);
                        case  "<": return (from m in db.MeterSet where m.Discription.CompareTo(val)  < 0 select m);
                        case ">=": return (from m in db.MeterSet where m.Discription.CompareTo(val) >= 0 select m);
                        case "<=": return (from m in db.MeterSet where m.Discription.CompareTo(val) <= 0 select m);
                        case "!=": return (from m in db.MeterSet where m.Discription.CompareTo(val) != 0 select m);
                    }
                }

                if (index == 2)
                {
                    double v = double.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from m in db.MeterSet where m.SumReadings.CompareTo(v) == 0 select m);
                        case  ">": return (from m in db.MeterSet where m.SumReadings.CompareTo(v)  > 0 select m);
                        case  "<": return (from m in db.MeterSet where m.SumReadings.CompareTo(v)  < 0 select m);
                        case ">=": return (from m in db.MeterSet where m.SumReadings.CompareTo(v) >= 0 select m);
                        case "<=": return (from m in db.MeterSet where m.SumReadings.CompareTo(v) <= 0 select m);
                        case "!=": return (from m in db.MeterSet where m.SumReadings.CompareTo(v) != 0 select m);
                    }
                }

                if (index == 3)
                {
                    double v = double.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from m in db.MeterSet where m.Capacity.CompareTo(v) == 0 select m);
                        case  ">": return (from m in db.MeterSet where m.Capacity.CompareTo(v)  > 0 select m);
                        case  "<": return (from m in db.MeterSet where m.Capacity.CompareTo(v)  < 0 select m);
                        case ">=": return (from m in db.MeterSet where m.Capacity.CompareTo(v) >= 0 select m);
                        case "<=": return (from m in db.MeterSet where m.Capacity.CompareTo(v) <= 0 select m);
                        case "!=": return (from m in db.MeterSet where m.Capacity.CompareTo(v) != 0 select m);
                    }
                }

                if (index == 4)
                {
                    int v = int.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from m in db.MeterSet where m.ProductionId.CompareTo(v) == 0 select m);
                        case  ">": return (from m in db.MeterSet where m.ProductionId.CompareTo(v)  > 0 select m);
                        case  "<": return (from m in db.MeterSet where m.ProductionId.CompareTo(v)  < 0 select m);
                        case ">=": return (from m in db.MeterSet where m.ProductionId.CompareTo(v) >= 0 select m);
                        case "<=": return (from m in db.MeterSet where m.ProductionId.CompareTo(v) <= 0 select m);
                        case "!=": return (from m in db.MeterSet where m.ProductionId.CompareTo(v) != 0 select m);
                    }
                }

                if (index == 5)
                {
                    DateTime v = DateTime.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from m in db.MeterSet where m.ProductionDate.CompareTo(v) == 0 select m);
                        case  ">": return (from m in db.MeterSet where m.ProductionDate.CompareTo(v)  > 0 select m);
                        case  "<": return (from m in db.MeterSet where m.ProductionDate.CompareTo(v)  < 0 select m);
                        case ">=": return (from m in db.MeterSet where m.ProductionDate.CompareTo(v) >= 0 select m);
                        case "<=": return (from m in db.MeterSet where m.ProductionDate.CompareTo(v) <= 0 select m);
                        case "!=": return (from m in db.MeterSet where m.ProductionDate.CompareTo(v) != 0 select m);
                    }
                }

                return null;
            }
        }

        private IEnumerable<InstalledMeter> MakeInstalledMeterList(ModelContainer1 db, int index, string val, string cond)
        {
            //using (var db = new ModelContainer1())
            {
                var req = from m in db.MeterSet where m is InstalledMeter select m as InstalledMeter;

                if (index == 0)
                {
                    int v = int.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from mi in req where mi.ProductionId.CompareTo(v) == 0 select mi);
                        case  ">": return (from mi in req where mi.ProductionId.CompareTo(v)  > 0 select mi);
                        case  "<": return (from mi in req where mi.ProductionId.CompareTo(v)  < 0 select mi);
                        case ">=": return (from mi in req where mi.ProductionId.CompareTo(v) >= 0 select mi);
                        case "<=": return (from mi in req where mi.ProductionId.CompareTo(v) <= 0 select mi);
                        case "!=": return (from mi in req where mi.ProductionId.CompareTo(v) != 0 select mi);
                    }
                }

                if (index == 1)
                {
                    DateTime v = DateTime.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from mi in req where mi.InstallDate.CompareTo(v) == 0 select mi);
                        case  ">": return (from mi in req where mi.InstallDate.CompareTo(v)  > 0 select mi);
                        case  "<": return (from mi in req where mi.InstallDate.CompareTo(v)  < 0 select mi);
                        case ">=": return (from mi in req where mi.InstallDate.CompareTo(v) >= 0 select mi);
                        case "<=": return (from mi in req where mi.InstallDate.CompareTo(v) <= 0 select mi);
                        case "!=": return (from mi in req where mi.InstallDate.CompareTo(v) != 0 select mi);
                    }
                }

                if (index == 2)
                {
                    DateTime v = DateTime.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from mi in req where mi.ExpirationDate.CompareTo(v) == 0 select mi);
                        case  ">": return (from mi in req where mi.ExpirationDate.CompareTo(v)  > 0 select mi);
                        case  "<": return (from mi in req where mi.ExpirationDate.CompareTo(v)  < 0 select mi);
                        case ">=": return (from mi in req where mi.ExpirationDate.CompareTo(v) >= 0 select mi);
                        case "<=": return (from mi in req where mi.ExpirationDate.CompareTo(v) <= 0 select mi);
                        case "!=": return (from mi in req where mi.ExpirationDate.CompareTo(v) != 0 select mi);
                    }
                }

                return null;
            }

        }

        private IEnumerable<User> MakeUserList(ModelContainer1 db, int index, string val, string cond)
        {
            //using (var db = new ModelContainer1())
            {
                if (index == 0)
                {
                    switch (cond)
                    {
                        case "==": return (from u in db.UserSet where u.Login.CompareTo(val) == 0 select u);
                        case  ">": return (from u in db.UserSet where u.Login.CompareTo(val)  > 0 select u);
                        case  "<": return (from u in db.UserSet where u.Login.CompareTo(val)  < 0 select u);
                        case ">=": return (from u in db.UserSet where u.Login.CompareTo(val) >= 0 select u);
                        case "<=": return (from u in db.UserSet where u.Login.CompareTo(val) <= 0 select u);
                        case "!=": return (from u in db.UserSet where u.Login.CompareTo(val) != 0 select u);
                    }
                }

                if (index == 1)
                {
                    switch (cond)
                    {
                        case "==": return (from u in db.UserSet where u.FullName.CompareTo(val) == 0 select u);
                        case  ">": return (from u in db.UserSet where u.FullName.CompareTo(val)  > 0 select u);
                        case  "<": return (from u in db.UserSet where u.FullName.CompareTo(val)  < 0 select u);
                        case ">=": return (from u in db.UserSet where u.FullName.CompareTo(val) >= 0 select u);
                        case "<=": return (from u in db.UserSet where u.FullName.CompareTo(val) <= 0 select u);
                        case "!=": return (from u in db.UserSet where u.FullName.CompareTo(val) != 0 select u);
                    }
                }

                if (index == 2)
                {
                    bool v = bool.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from u in db.UserSet where u.AdminPrivileges.CompareTo(val) == 0 select u);
                        case  ">": return (from u in db.UserSet where u.AdminPrivileges.CompareTo(val)  > 0 select u);
                        case  "<": return (from u in db.UserSet where u.AdminPrivileges.CompareTo(val)  < 0 select u);
                        case ">=": return (from u in db.UserSet where u.AdminPrivileges.CompareTo(val) >= 0 select u);
                        case "<=": return (from u in db.UserSet where u.AdminPrivileges.CompareTo(val) <= 0 select u);
                        case "!=": return (from u in db.UserSet where u.AdminPrivileges.CompareTo(val) != 0 select u);
                    }
                }
            }
            return null;
        }

        private IEnumerable<Type> MakeTypeList(ModelContainer1 db, int index, string val, string cond)
        {
            //using (var db = new ModelContainer1())
            {
                if (index == 0)
                {
                    switch (cond)
                    {
                        case "==": return (from t in db.TypeSet where t.Name.CompareTo(val) == 0 select t);
                        case  ">": return (from t in db.TypeSet where t.Name.CompareTo(val)  > 0 select t);
                        case  "<": return (from t in db.TypeSet where t.Name.CompareTo(val)  < 0 select t);
                        case ">=": return (from t in db.TypeSet where t.Name.CompareTo(val) >= 0 select t);
                        case "<=": return (from t in db.TypeSet where t.Name.CompareTo(val) <= 0 select t);
                        case "!=": return (from t in db.TypeSet where t.Name.CompareTo(val) != 0 select t);
                    }
                }

                if (index == 1)
                {
                    switch (cond)
                    {
                        case "==": return (from t in db.TypeSet where t.Unit.CompareTo(val) == 0 select t);
                        case  ">": return (from t in db.TypeSet where t.Unit.CompareTo(val)  > 0 select t);
                        case  "<": return (from t in db.TypeSet where t.Unit.CompareTo(val)  < 0 select t);
                        case ">=": return (from t in db.TypeSet where t.Unit.CompareTo(val) >= 0 select t);
                        case "<=": return (from t in db.TypeSet where t.Unit.CompareTo(val) <= 0 select t);
                        case "!=": return (from t in db.TypeSet where t.Unit.CompareTo(val) != 0 select t);
                    }
                }
            }
            return null;
        }

        private IEnumerable<Parametr> MakeParametrList(ModelContainer1 db, int index, string val, string cond)
        {
            if (index == 0)
            {
                switch (cond)
                {
                    case "==": return (from t in db.ParametrSet where t.Name.CompareTo(val) == 0 select t);
                    case  ">": return (from t in db.ParametrSet where t.Name.CompareTo(val)  > 0 select t);
                    case  "<": return (from t in db.ParametrSet where t.Name.CompareTo(val)  < 0 select t);
                    case ">=": return (from t in db.ParametrSet where t.Name.CompareTo(val) >= 0 select t);
                    case "<=": return (from t in db.ParametrSet where t.Name.CompareTo(val) <= 0 select t);
                    case "!=": return (from t in db.ParametrSet where t.Name.CompareTo(val) != 0 select t);
                }
            }

            if (index == 1)
            {
                switch (cond)
                {
                    case "==": return (from t in db.ParametrSet where t.Measure.CompareTo(val) == 0 select t);
                    case  ">": return (from t in db.ParametrSet where t.Measure.CompareTo(val)  > 0 select t);
                    case  "<": return (from t in db.ParametrSet where t.Measure.CompareTo(val)  < 0 select t);
                    case ">=": return (from t in db.ParametrSet where t.Measure.CompareTo(val) >= 0 select t);
                    case "<=": return (from t in db.ParametrSet where t.Measure.CompareTo(val) <= 0 select t);
                    case "!=": return (from t in db.ParametrSet where t.Measure.CompareTo(val) != 0 select t);
                }
            }
            return null;
        }

        private IEnumerable<Tariff> MakeTariffList(ModelContainer1 db, int index, string val, string cond)
        {
            {
                if (index == 0)
                {
                    switch (cond)
                    {
                        case "==": return (from t in db.TariffSet where t.Name.CompareTo(val) == 0 select t);
                        case  ">": return (from t in db.TariffSet where t.Name.CompareTo(val)  > 0 select t);
                        case  "<": return (from t in db.TariffSet where t.Name.CompareTo(val)  < 0 select t);
                        case ">=": return (from t in db.TariffSet where t.Name.CompareTo(val) >= 0 select t);
                        case "<=": return (from t in db.TariffSet where t.Name.CompareTo(val) <= 0 select t);
                        case "!=": return (from t in db.TariffSet where t.Name.CompareTo(val) != 0 select t);
                    }
                }
            }
            return null;
        }

        private IEnumerable<TimeSpan> MakeTimeSpanList(ModelContainer1 db, int index, string val, string cond)
        {
            //using (var db = new ModelContainer1())
            {
                if (index == 0)
                {
                    switch (cond)
                    {
                        case "==": return (from t in db.TimeSpanSet where t.Name.CompareTo(val) == 0 select t);
                        case  ">": return (from t in db.TimeSpanSet where t.Name.CompareTo(val)  > 0 select t);
                        case  "<": return (from t in db.TimeSpanSet where t.Name.CompareTo(val)  < 0 select t);
                        case ">=": return (from t in db.TimeSpanSet where t.Name.CompareTo(val) >= 0 select t);
                        case "<=": return (from t in db.TimeSpanSet where t.Name.CompareTo(val) <= 0 select t);
                        case "!=": return (from t in db.TimeSpanSet where t.Name.CompareTo(val) != 0 select t);
                    }
                }

                if (index == 1)
                {
                    System.TimeSpan v = System.TimeSpan.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from t in db.TimeSpanSet where t.TimeStart.CompareTo(v) == 0 select t);
                        case  ">": return (from t in db.TimeSpanSet where t.TimeStart.CompareTo(v)  > 0 select t);
                        case  "<": return (from t in db.TimeSpanSet where t.TimeStart.CompareTo(v)  < 0 select t);
                        case ">=": return (from t in db.TimeSpanSet where t.TimeStart.CompareTo(v) >= 0 select t);
                        case "<=": return (from t in db.TimeSpanSet where t.TimeStart.CompareTo(v) <= 0 select t);
                        case "!=": return (from t in db.TimeSpanSet where t.TimeStart.CompareTo(v) != 0 select t);
                    }
                }

                if (index == 2)
                {
                    System.TimeSpan v = System.TimeSpan.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from t in db.TimeSpanSet where t.TimeEnd.CompareTo(v) == 0 select t);
                        case  ">": return (from t in db.TimeSpanSet where t.TimeEnd.CompareTo(v)  > 0 select t);
                        case  "<": return (from t in db.TimeSpanSet where t.TimeEnd.CompareTo(v)  < 0 select t);
                        case ">=": return (from t in db.TimeSpanSet where t.TimeEnd.CompareTo(v) >= 0 select t);
                        case "<=": return (from t in db.TimeSpanSet where t.TimeEnd.CompareTo(v) <= 0 select t);
                        case "!=": return (from t in db.TimeSpanSet where t.TimeEnd.CompareTo(v) != 0 select t);
                    }
                }
            }

            return null;
        }

        private IEnumerable<Reading> MakeReadingList(ModelContainer1 db, int index, string val, string cond)
        {
            //using (var db = new ModelContainer1())
            {
                if (index == 0)
                {
                    double v = double.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from r in db.ReadingSet where r.Value.CompareTo(v) == 0 select r);
                        case  ">": return (from r in db.ReadingSet where r.Value.CompareTo(v)  > 0 select r);
                        case  "<": return (from r in db.ReadingSet where r.Value.CompareTo(v)  < 0 select r);
                        case ">=": return (from r in db.ReadingSet where r.Value.CompareTo(v) >= 0 select r);
                        case "<=": return (from r in db.ReadingSet where r.Value.CompareTo(v) <= 0 select r);
                        case "!=": return (from r in db.ReadingSet where r.Value.CompareTo(v) != 0 select r);
                    }
                }

                if (index == 1)
                {
                    int v = int.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from r in db.ReadingSet where r.TariffNumber.CompareTo(v) == 0 select r);
                        case  ">": return (from r in db.ReadingSet where r.TariffNumber.CompareTo(v)  > 0 select r);
                        case  "<": return (from r in db.ReadingSet where r.TariffNumber.CompareTo(v)  < 0 select r);
                        case ">=": return (from r in db.ReadingSet where r.TariffNumber.CompareTo(v) >= 0 select r);
                        case "<=": return (from r in db.ReadingSet where r.TariffNumber.CompareTo(v) <= 0 select r);
                        case "!=": return (from r in db.ReadingSet where r.TariffNumber.CompareTo(v) != 0 select r);
                    }
                }
            }

            return null;
        }

        private IEnumerable<Document> MakeDocumentList(ModelContainer1 db, int index, string val, string cond)
        {
            //using (var db = new ModelContainer1())
            {
                if (index == 0)
                {
                    switch (cond)
                    {
                        case "==": return (from d in db.DocumentSet where d.Title.CompareTo(val) == 0 select d);
                        case  ">": return (from d in db.DocumentSet where d.Title.CompareTo(val)  > 0 select d);
                        case  "<": return (from d in db.DocumentSet where d.Title.CompareTo(val)  < 0 select d);
                        case ">=": return (from d in db.DocumentSet where d.Title.CompareTo(val) >= 0 select d);
                        case "<=": return (from d in db.DocumentSet where d.Title.CompareTo(val) <= 0 select d);
                        case "!=": return (from d in db.DocumentSet where d.Title.CompareTo(val) != 0 select d);
                    }
                }

                if (index == 1)
                {
                    switch (cond)
                    {
                        case "==": return (from d in db.DocumentSet where d.Discription.CompareTo(val) == 0 select d);
                        case  ">": return (from d in db.DocumentSet where d.Discription.CompareTo(val)  > 0 select d);
                        case  "<": return (from d in db.DocumentSet where d.Discription.CompareTo(val)  < 0 select d);
                        case ">=": return (from d in db.DocumentSet where d.Discription.CompareTo(val) >= 0 select d);
                        case "<=": return (from d in db.DocumentSet where d.Discription.CompareTo(val) <= 0 select d);
                        case "!=": return (from d in db.DocumentSet where d.Discription.CompareTo(val) != 0 select d);
                    }
                }

                if (index == 2)
                {
                    DateTime v = DateTime.Parse(val);

                    switch (cond)
                    {
                        case "==": return (from d in db.DocumentSet where d.SigningDate.CompareTo(v) == 0 select d);
                        case  ">": return (from d in db.DocumentSet where d.SigningDate.CompareTo(v)  > 0 select d);
                        case  "<": return (from d in db.DocumentSet where d.SigningDate.CompareTo(v)  < 0 select d);
                        case ">=": return (from d in db.DocumentSet where d.SigningDate.CompareTo(v) >= 0 select d);
                        case "<=": return (from d in db.DocumentSet where d.SigningDate.CompareTo(v) <= 0 select d);
                        case "!=": return (from d in db.DocumentSet where d.SigningDate.CompareTo(v) != 0 select d);
                    }
                }
            }

            return null;
        }

        // Выгрузка в Эксель
        private void bToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (dgList.Items.Count == 0)
            {
                MessageBox.Show("Нечего выгружать", "Ошибка выгрузки", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Export.Excel(new DataGrid() { ItemsSource = dgList.ItemsSource, Name = cbEntity.Text.Replace(" ","_") });
        }
    }
}
