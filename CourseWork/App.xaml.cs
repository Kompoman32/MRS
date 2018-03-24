using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void EventSetter_OnHandler(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock txt)
            {
                txt.ToolTip = new ToolTip(){Content = txt.Text};
            }
        }
    }
}
