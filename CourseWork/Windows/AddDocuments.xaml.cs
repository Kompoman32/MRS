using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для AdminAddDocuments.xaml
    /// </summary>
    public partial class AddDocuments : Window
    {
        public AddDocuments(System.Collections.Generic.List<Document> documents = null)
        {
            InitializeComponent();

            Icon = ImageConverter.convertBtmToBtmSource(Properties.Resources.logo);

            dpSignDate.DisplayDate=DateTime.Now;
            dpSignDate.SelectedDate=DateTime.Now;
            dpSignDate.DisplayDateEnd = DateTime.Now;
            dpSignDate.DisplayDateStart = new DateTime(1900,1,1);

            if (documents != null)
                foreach (var doc in documents)
                    lbDocuments.Items.Insert(lbDocuments.Items.Count-1,doc);
        }

        private void bNewDoc_Click(object sender, RoutedEventArgs e)
        {
            bDelDoc.IsEnabled = false;
            bSaveDoc.IsEnabled = false;
            tbTitle.Text = "";
            tbDiscription.Text = "";
        }

        // заблокировать кнопку удалить (файл изменён)
        private void TexBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            bDelDoc.IsEnabled = false;
            bSaveDoc.IsEnabled = true;
        }

        // заблокировать кнопку удалить (файл изменён)
        private void dpSignDate_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            bDelDoc.IsEnabled = false;
            bSaveDoc.IsEnabled = true;
        }

        private void lbDocuments_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || e.AddedItems[0] is Button) return;

            Document doc = e.AddedItems[0] as Document;
            tbTitle.Text = doc.Title;
            tbDiscription.Text = doc.Discription;
            dpSignDate.SelectedDate = doc.SigningDate;
            bDelDoc.IsEnabled = true;
            bSaveDoc.IsEnabled = false;
        }

        // Проверка полей
        private bool CheckFields()
        {
            if (tbTitle.Text == "")
                return Error.Show("Заголовок не заполнен", "Ошибка ввода");
            if (tbDiscription.Text == "")
                return Error.Show("Описание не заполнено", "Ошибка ввода");

            for (int i = 0; i < lbDocuments.Items.Count - 1; i++)
            {
                Document doc = lbDocuments.Items[i] as Document;
                if (tbTitle.Text == doc.Title && tbDiscription.Text == doc.Discription && dpSignDate.SelectedDate == doc.SigningDate)
                    return Error.Show("Данный документ уже присутствует", "Ошибка ввода");
            }

            if (dpSignDate.SelectedDate < new DateTime(1900,1,1) || dpSignDate.SelectedDate > DateTime.Now)
                return Error.Show("Дата подписания должна быть не меньше чем " + new DateTime(1900, 1, 1).ToString("d")+" и не больше чем " + DateTime.Now.ToString("d"), "Ошибка ввода");

            return true;
        }

        // Сохранить Док
        private void bSaveDoc_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckFields()) return;

            Document doc = new Document()
            {
                Discription = tbDiscription.Text,
                Title = tbTitle.Text,
                SigningDate = (DateTime) dpSignDate.SelectedDate
            };

            lbDocuments.Items.Insert(lbDocuments.Items.Count - 1, doc);

            bSaveDoc.IsEnabled = false;
            bDelDoc.IsEnabled = true;
        }

        // Удалить Док
        private void bDelDoc_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lbDocuments.Items.Count - 1; i++)
            {
                Document doc = lbDocuments.Items[i] as Document;
                if (tbTitle.Text == doc.Title && tbDiscription.Text == doc.Discription && dpSignDate.SelectedDate == doc.SigningDate)
                    lbDocuments.Items.RemoveAt(i);
            }

            bNewDoc_Click(null,null);
        }

        public System.Collections.Generic.List<Document> Documents=new System.Collections.Generic.List<Document>();

        private void AddDocuments_OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lbDocuments.Items.Count > 0)
                if (MessageBox.Show("Желаете сохранить " + (lbDocuments.Items.Count - 1) + " документов?", "Сохранение",
                        MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                { e.Cancel=true; return;}

            for (int i = 0; i < lbDocuments.Items.Count - 1; i++)
                Documents.Add(lbDocuments.Items[i] as Document);
        }
    }
}
