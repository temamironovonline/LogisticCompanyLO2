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
    /// Логика взаимодействия для OrdersListPage.xaml
    /// </summary>
    public partial class OrdersListPage : Page
    {
        public OrdersListPage()
        {
            InitializeComponent();

            orderList.ItemsSource = DataBaseConnection.dataBaseEntities.Orders.ToList();

            ComboBoxItem defaultExecutor = new ComboBoxItem()
            {
                Content = "Не выбрано",
                Uid = "0"
            };

            executorFiltrationSelect.Items.Add(defaultExecutor);

            List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();

            foreach(Executors executor in executors)
            {
                ComboBoxItem executorAdd = new ComboBoxItem()
                {
                    Content = $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}",
                    Uid = $"{executor.Code_Executor}"
                };

                executorFiltrationSelect.Items.Add(executorAdd);
            }

            executorFiltrationSelect.SelectedIndex = 0;

            statusFiltrationSelect.SelectedIndex = 0;

            sortingSelect.SelectedIndex = 0;
        }

        private void addOrder_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderWindow createOrder = new CreateOrderWindow();

            createOrder.ShowDialog();

            FrameClass.frame.Navigate(new OrdersListPage());
        }

        private void orderList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Orders currentOrder = orderList.SelectedItem as Orders;

            CreateOrderWindow updateOrder = new CreateOrderWindow(currentOrder);

            updateOrder.ShowDialog();

            FrameClass.frame.Navigate(new OrdersListPage());
        }

        private void routeFiltrationInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrationInformation();
        }

        private void dateFiltrationSelect_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrationInformation();
        }

        private void statusFiltrationSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrationInformation();
        }

        private List<Orders> _orders;

        private Regex _routeRegex, _cargoNameRegex;

        private void routeFiltrationInput_GotFocus(object sender, RoutedEventArgs e)
        {
            routeFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void routeFiltrationInput_LostFocus(object sender, RoutedEventArgs e)
        {
            routeFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void cargoNameFiltrationInput_GotFocus(object sender, RoutedEventArgs e)
        {
            cargoNameFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void cargoNameFiltrationInput_LostFocus(object sender, RoutedEventArgs e)
        {
            cargoNameFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void executorFiltrationSelect_DropDownOpened(object sender, EventArgs e)
        {
            executorFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void executorFiltrationSelect_DropDownClosed(object sender, EventArgs e)
        {
            executorFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void statusFiltrationSelect_DropDownOpened(object sender, EventArgs e)
        {
            statusFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void statusFiltrationSelect_DropDownClosed(object sender, EventArgs e)
        {
            statusFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void dateFiltrationSelect_GotFocus(object sender, RoutedEventArgs e)
        {
            dateFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            dateFiltrationSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineDate.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void dateFiltrationSelect_LostFocus(object sender, RoutedEventArgs e)
        {
            dateFiltrationHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            dateFiltrationSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineDate.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void sortingSelect_DropDownOpened(object sender, EventArgs e)
        {
            sortingHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));

        }

        private void sortingSelect_DropDownClosed(object sender, EventArgs e)
        {
            sortingHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void clearFiltration_Click(object sender, RoutedEventArgs e)
        {
            routeFiltrationInput.Clear();
            cargoNameFiltrationInput.Clear();
            executorFiltrationSelect.SelectedIndex = 0;
            statusFiltrationSelect.SelectedIndex = 0;
            dateFiltrationSelect.Text = null;
            sortingSelect.SelectedIndex = 0;
        }

        private void deleteOrder_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow();

            confirmDelete.ShowDialog();

            Orders order = orderList.SelectedItem as Orders;

            if (confirmDelete.GetReturnCode() == 1)
            {
                DataBaseConnection.dataBaseEntities.Orders.Remove(order);

                DataBaseConnection.dataBaseEntities.SaveChanges();

                MessageBox.Show("Операция выполнена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            FrameClass.frame.Navigate(new OrdersListPage());
        }

        private void driverName_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock order = (TextBlock)sender;

            int index = Convert.ToInt32(order.Uid);

            Orders currentOrder = DataBaseConnection.dataBaseEntities.Orders.FirstOrDefault(x => x.Code_Order == index);

            if (currentOrder.Executor_Is_Driver == true)
            {
                Executors executor = DataBaseConnection.dataBaseEntities.Executors.FirstOrDefault(x => x.Code_Executor == currentOrder.Code_Executor);

                order.Text = $"Водитель является исполнителем";
            }
        }

        private void FiltrationInformation()
        {
            _orders = DataBaseConnection.dataBaseEntities.Orders.ToList();

            _routeRegex = new Regex($@".*{routeFiltrationInput.Text.ToLower()}.*");

            _orders = _orders.Where(x => _routeRegex.IsMatch(x.Route_Name.ToLower())).ToList();

            _cargoNameRegex = new Regex($@".*{cargoNameFiltrationInput.Text.ToLower()}.*");

            _orders = _orders.Where(x => _cargoNameRegex.IsMatch(x.Cargo_Name.ToLower())).ToList();

            if (executorFiltrationSelect.SelectedIndex != 0)
            {
                ComboBoxItem selectedExecutor = executorFiltrationSelect.SelectedItem as ComboBoxItem;

                int indexExecutor = Convert.ToInt32(selectedExecutor.Uid);

                _orders = _orders.Where(x => x.Code_Executor == indexExecutor).ToList();
            }

            if (dateFiltrationSelect.SelectedDate != null)
            {
                _orders = _orders.Where(x => x.Date_Order == dateFiltrationSelect.SelectedDate).ToList();
            }

            if (statusFiltrationSelect.SelectedIndex == 1)
            {
                _orders = _orders.Where(x => x.Status_Order == false).ToList();
            }
            else if (statusFiltrationSelect.SelectedIndex == 2)
            {
                _orders = _orders.Where(x => x.Status_Order == true).ToList();
            }

            switch (sortingSelect.SelectedIndex)
            {
                case 1:
                    _orders = _orders.OrderBy(x => x.Date_Order).ToList();
                    break;

                case 2:
                    _orders = _orders.OrderByDescending(x => x.Date_Order).ToList();
                    break;
            }

            orderList.ItemsSource = _orders;
        }
    }
}
