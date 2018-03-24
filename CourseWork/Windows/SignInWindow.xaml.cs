using System.Linq;
using System.Windows;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Icon = ImageConverter.convertBtmToBtmSource(Properties.Resources.logo);
        }

        //LOGIN
        void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ModelContainer1()) // создание подключения к БД
            {
                // выборка всех пользователей с таким логином
                var req = from u in db.UserSet where u.Login == tbLoginL.Text select u;
                
                User user = req.Any() ? req.First() : null;

                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка ввода", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                if (user.Password != pbPasswordL.Password)
                {
                    MessageBox.Show("Введён неверный пароль!", "Ошибка ввода", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                // В зависимости от привилегий разный доступ
                if (user.AdminPrivileges)
                    ShowAdmin(user.Login);
                else 
                    ShowUser(user.Login);
            }           
        }

        void ShowUser(string login)
        {
            UserWindow uw=new UserWindow(login);
            this.Hide();
            uw.ShowDialog();
            this.Show();
        }

        void ShowAdmin(string login)
        {
            AdminWindow aw = new AdminWindow(login);
            this.Hide();
            aw.ShowDialog();
            this.Show();
        }

       //REGISTARTION
       bool CheckFieldsR()
        {
            if (tbFullNameR.Text == "")
            return Error.Show("Поле ФИО не заполнено", "Ошибка ввода");

             if (tbLoginR.Text == "")
                 return Error.Show("Поле Логин не заполнено", "Ошибка ввода");

             if (tbPasswordR.Text == "")
                return Error.Show("Поле Пароль не заполнено", "Ошибка ввода");

            using (var db = new ModelContainer1())
                 if ((from u in db.UserSet where u.Login == tbLoginR.Text select u).Any())
                     return Error.Show("Данный логин уже занят, попробуйте другой","Ошибка ввода");

            return true;
        }
        
        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFieldsR()) return;

            using (var db = new ModelContainer1())
            {
                db.UserSet.Add(new User()
                {
                    AdminPrivileges = false,
                    FullName = tbFullNameR.Text,
                    Login = tbLoginR.Text,
                    Password = tbPasswordR.Text
                });

                db.SaveChanges();

                ShowUser(tbLoginR.Text);
            }
        }
    }
}
