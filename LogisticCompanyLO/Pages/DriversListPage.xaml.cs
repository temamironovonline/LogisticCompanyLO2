using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogisticCompanyLO
{
    /// <summary>
    /// Логика взаимодействия для DriversListPage.xaml
    /// </summary>
    public partial class DriversListPage : Page
    {
        public DriversListPage()
        {
            InitializeComponent();

            List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();

            executorFiltration.Items.Add("Не выбрано");

            foreach (Executors executor in executors)
            {
                ComboBoxItem executorItem = new ComboBoxItem()
                {
                    Content = $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}",
                    Uid = $"{executor.Code_Executor}"
                };
                executorFiltration.Items.Add(executorItem);
            }

            executorFiltration.SelectedIndex = 0;

            driversList.ItemsSource = DataBaseConnection.dataBaseEntities.Drivers.ToList();
        }

        private void clearFiltration_Click(object sender, RoutedEventArgs e)
        {
            surnameDriverFiltration.Text = "";
            executorFiltration.SelectedIndex = 0;
        }

        private void addDriver_Click(object sender, RoutedEventArgs e)
        {
            CreateDriverWindow createDriver = new CreateDriverWindow();
            createDriver.ShowDialog();
            FrameClass.frame.Navigate(new DriversListPage());
        }

        private void surnameDriverFiltration_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFiltrationDriver();
        }

        private void executorFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetFiltrationDriver();
        }

        private List<Drivers> _driverFiltrationList;
        private Regex _surnameRegex;

        private void GetFiltrationDriver()
        {
            _driverFiltrationList = DataBaseConnection.dataBaseEntities.Drivers.ToList();
            _surnameRegex = new Regex($@".*{surnameDriverFiltration.Text.ToLower()}.*");

            _driverFiltrationList = _driverFiltrationList.Where(x => _surnameRegex.IsMatch(x.Surname_Driver.ToLower())).ToList();

            if (executorFiltration.SelectedIndex > 0)
            {
                ComboBoxItem selectedExecutor = executorFiltration.SelectedItem as ComboBoxItem;
                int index = Convert.ToInt32(selectedExecutor.Uid);
                _driverFiltrationList = _driverFiltrationList.Where(x => x.Code_Executor == index).ToList();
            }

            driversList.ItemsSource = _driverFiltrationList;
        }

        private void driversList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Drivers driver = driversList.SelectedItem as Drivers;

            CreateDriverWindow updateDriver = new CreateDriverWindow(driver);

            updateDriver.ShowDialog();

            FrameClass.frame.Navigate(new DriversListPage());
        }

        private void deleteDriver_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteWindow confirmDeleteWindow = new ConfirmDeleteWindow();

            confirmDeleteWindow.ShowDialog();

            if (confirmDeleteWindow.GetReturnCode() == 1)
            {
                Drivers driver = driversList.SelectedItem as Drivers;

                Personal_Data personalDataDriver = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == driver.Code_Personal_Data);

                DataBaseConnection.dataBaseEntities.Personal_Data.Remove(personalDataDriver);

                DataBaseConnection.dataBaseEntities.SaveChanges();

                FrameClass.frame.Navigate(new DriversListPage());
            }
        }

        private void surnameDriverFiltration_GotFocus(object sender, RoutedEventArgs e)
        {
            surnameDriverHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void surnameDriverFiltration_LostFocus(object sender, RoutedEventArgs e)
        {
            surnameDriverHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void executorFiltration_DropDownOpened(object sender, System.EventArgs e)
        {
            executorHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void executorFiltration_DropDownClosed(object sender, System.EventArgs e)
        {
            executorHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }
    }
}
