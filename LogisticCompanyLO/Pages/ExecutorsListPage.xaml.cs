using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            addExecutor.Title = "FREGATE LOGISTIC - Изменение исполнителя";

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
                List<Drivers> drivers = DataBaseConnection.dataBaseEntities.Drivers.Where(x => x.Code_Executor == executor.Code_Executor).ToList(); // Находим список водителей, принадлежащих удаляемому исполнителю

                // Удаление данных происходит через таблицу Personal_Data с помощью каскадного удаления
                foreach (Drivers driver in drivers) // Удаляем водителей
                {
                    Personal_Data personalDataDriver = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == driver.Code_Personal_Data);
                    DataBaseConnection.dataBaseEntities.Personal_Data.Remove(personalDataDriver);
                }
                
                Personal_Data personalDataExecutor = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == executor.Code_Personal_Data);
                DataBaseConnection.dataBaseEntities.Personal_Data.Remove(personalDataExecutor); // Удаляем исполнителя
                DataBaseConnection.dataBaseEntities.SaveChanges();
                MessageBox.Show("Удаление прошло успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            FrameClass.frame.Navigate(new ExecutorsListPage());
        }

        private void clearFiltration_Click(object sender, RoutedEventArgs e)
        {
            surnameExecutorInput.Clear();
        }

        private void surnameExecutorInput_GotFocus(object sender, RoutedEventArgs e)
        {
            surnameExecutorHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void surnameExecutorInput_LostFocus(object sender, RoutedEventArgs e)
        {
            surnameExecutorHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }
    }
}
