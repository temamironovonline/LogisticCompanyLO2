using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LogisticCompanyLO
{
    /// <summary>
    /// Логика взаимодействия для ExecutorsListPage.xaml
    /// </summary>
    public partial class ExecutorsListPage : Page
    {
        public ExecutorsListPage()
        {
            InitializeComponent();

            executorsList.ItemsSource = DataBaseConnection.dataBaseEntities.Executors.ToList();
        }

        private void surnameExecutorInput_TextChanged(object sender, TextChangedEventArgs e) // Поиск по фамили исполнителя
        {
            Regex surnameRegex = new Regex($@".*{surnameExecutorInput.Text.ToLower()}.*"); // Регулярное выражение на соответствие любого введеного символа в фамилиях исполнителей
            List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();
            executorsList.ItemsSource = executors.Where(x => surnameRegex.IsMatch(x.Surname_Executor.ToLower())).ToList();
        }

        private void createExecutor_Click(object sender, System.Windows.RoutedEventArgs e) // Создание нового исполнителя
        {
            AddExecutorWindow createExecutor = new AddExecutorWindow();
            
            createExecutor.ShowDialog();
            
            FrameClass.frame.Navigate(new ExecutorsListPage());
        }

        private void executorsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) // Изменение исполнителя
        {
            Executors selectedExecutor = (Executors)executorsList.SelectedItem;

            AddExecutorWindow addExecutor = new AddExecutorWindow(selectedExecutor);

            addExecutor.Title = "CARAVELLA - Изменение исполнителя";

            addExecutor.ShowDialog();

            FrameClass.frame.Navigate(new ExecutorsListPage());
        }

        private void deleteExecutor_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Executors executor = (Executors)executorsList.SelectedItem;
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow();
            confirmDelete.ShowDialog();
            if (confirmDelete.GetReturnCode() == 1)
            {
                DataBaseConnection.dataBaseEntities.Executors.Remove(executor);
                DataBaseConnection.dataBaseEntities.SaveChanges();
                MessageBox.Show("Удаление прошло успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            FrameClass.frame.Navigate(new ExecutorsListPage());
        }
    }
}
