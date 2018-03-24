using System.Linq;
using System.Windows;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для AdminAddNewParameter.xaml
    /// </summary>
    public partial class AddNewParameter : Window
    {
        public Parametr Parametr = null;

        public AddNewParameter()
        {
            InitializeComponent();

            Icon = ImageConverter.convertBtmToBtmSource(Properties.Resources.logo);

            tbName.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "")
            {
                MessageBox.Show("Пожалуства заполнине поле \"Название\"", "Ошибка, поле пустое", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (tbMeasure.Text == "")
            {
                MessageBox.Show("Пожалуства заполнине поле \"Значение\"", "Ошибка, поле пустое", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            using (var db = new ModelContainer1())
            {
                if ((from p in db.ParametrSet where p.Name == tbName.Text && p.Measure == tbMeasure.Text select p).AsParallel().Any())
                {
                    MessageBox.Show("Данный параметр уже присутствует в списке доступных или установленных.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            
            Parametr = new Parametr(){Name = tbName.Text, Measure = tbMeasure.Text};

            Close();
        }
    }
}
