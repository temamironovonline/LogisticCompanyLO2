using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LogisticCompanyLO
{
    public partial class CreateOrderWindow : Window
    {
        public CreateOrderWindow()
        {
            InitializeComponent();

            UploadInformation();

        }

        private Orders _currentOrder;

        public CreateOrderWindow(Orders order)
        {
            InitializeComponent();

            UploadInformation();

            _currentOrder = order;

            deleteOrder.Visibility = Visibility.Visible;
            convertToWord.Visibility = Visibility.Visible;

            dateOrderSelect.SelectedDate = order.Date_Order;

            if (order.Code_Executor != null)
            {
                foreach (ComboBoxItem executorItem in executorSelect.Items)
                {
                    if (Convert.ToInt32(executorItem.Uid) == order.Code_Executor)
                    {
                        executorSelect.SelectedValue = executorItem;
                    }
                }

                ExecutorChanged();

                if (order.Code_Driver != null)
                {
                    foreach (ComboBoxItem driverItem in driverSelect.Items)
                    {
                        if (Convert.ToInt32(driverItem.Uid) == order.Code_Driver)
                        {
                            driverSelect.SelectedValue = driverItem;
                        }
                    }
                }

                if (order.Executor_Is_Driver == true)
                {
                    driverSelect.SelectedIndex = 1;
                }

                if (order.Code_Vehicle != null)
                {
                    foreach (ComboBoxItem vehicleItem in vehicleSelect.Items)
                    {
                        if (Convert.ToInt32(vehicleItem.Uid) == order.Code_Vehicle)
                        {
                            vehicleSelect.SelectedValue = vehicleItem;
                        }
                    }
                }
            }

            cargoNameInput.Text = order.Cargo_Name;
            conditionTransportationInput.Text = order.Conditions_Transportation;

            dateLoadingSelect.Text = order.Date_Loading.ToString();
            placeLoadingInput.Text = order.Address_Loading;
            shipperNameInput.Text = order.Shipper_Name;
            shipperTelephoneInput.Text = order.Shipper_Telephone;

            dateUnloadingSelect.Text = order.Date_Unloading.ToString();
            placeUnloadingInput.Text = order.Address_Unloading;
            consigneeNameInput.Text = order.Consignee_Name;
            consigneeTelephoneInput.Text = order.Consignee_Telephone;

            routeInput.Text = order.Route_Name;
            routeFormatInput.Text = order.Transportation_Format;
            costPaymentInput.Text = order.Cost_Payment;

            statusOrderSelect.SelectedIndex = Convert.ToInt32(order.Status_Order);

        }

        private void UploadInformation()
        {
            ComboBoxItem defaultExecutor = new ComboBoxItem()
            {
                Content = "Не выбрано",
                Uid = "0"
            };

            executorSelect.Items.Add(defaultExecutor);

            List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();

            foreach (Executors executor in executors)
            {
                ComboBoxItem executorAdd = new ComboBoxItem()
                {
                    Content = $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}",
                    Uid = $"{executor.Code_Executor}"
                };

                executorSelect.Items.Add(executorAdd);
            }

            executorSelect.SelectedIndex = 0;
        }

        private void ExecutorChanged()
        {
            if (executorSelect.SelectedIndex > 0)
            {
                driverSelect.IsEnabled = true;
                driverSelect.Items.Clear();

                vehicleSelect.IsEnabled = true;
                vehicleSelect.Items.Clear();

                ComboBoxItem defaultDriver = new ComboBoxItem()
                {
                    Content = "Не выбрано",
                    Uid = "0"
                };

                ComboBoxItem executorDriver = new ComboBoxItem()
                {
                    Content = "Исполнитель-водитель",
                    Uid = "-1"
                };

                driverSelect.Items.Add(defaultDriver);
                driverSelect.Items.Add(executorDriver);
                driverSelect.SelectedIndex = 0;

                ComboBoxItem defaultVehicle = new ComboBoxItem()
                {
                    Content = "Не выбрано",
                    Uid = "0"
                };

                vehicleSelect.Items.Add(defaultVehicle);
                vehicleSelect.SelectedIndex = 0;

                ComboBoxItem selectedExecutor = executorSelect.SelectedItem as ComboBoxItem;

                int indexExecutor = Convert.ToInt32(selectedExecutor.Uid);

                List<Drivers> drivers = DataBaseConnection.dataBaseEntities.Drivers.Where(x => x.Code_Executor == indexExecutor).ToList();

                foreach (Drivers driver in drivers)
                {
                    ComboBoxItem driverAdd = new ComboBoxItem()
                    {
                        Content = $"{driver.Surname_Driver} {driver.Name_Driver} {driver.Midname_Driver}",
                        Uid = $"{driver.Code_Driver}"
                    };

                    driverSelect.Items.Add(driverAdd);
                }

                List<Vehicles> vehicles = DataBaseConnection.dataBaseEntities.Vehicles.Where(x => x.Code_Executor == indexExecutor).ToList();

                foreach (Vehicles vehicle in vehicles)
                {
                    ComboBoxItem vehicleAdd = new ComboBoxItem()
                    {
                        Content = $"{vehicle.Brand_Vehicle} {vehicle.Model_Vehicle} \"{vehicle.Number_Vehicle}\"",
                        Uid = $"{vehicle.Code_Vehicle}"
                    };

                    vehicleSelect.Items.Add(vehicleAdd);
                }
            }
            else
            {
                driverSelect.Items.Clear();
                driverSelect.IsEnabled = false;

                vehicleSelect.Items.Clear();
                vehicleSelect.IsEnabled = false;
            }
        }

        private void executorSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExecutorChanged();
        }

        private void addOrder_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int? indexExecutor = null;
                if (executorSelect.SelectedIndex > 0)
                {
                    ComboBoxItem currentExecutor = executorSelect.SelectedItem as ComboBoxItem;

                    indexExecutor = Convert.ToInt32(currentExecutor.Uid);
                }

                int? indexDriver = null;

                bool executorIsDriver = false;

                if (driverSelect.SelectedIndex > 1)
                {
                    ComboBoxItem currentDriver = driverSelect.SelectedItem as ComboBoxItem;

                    indexDriver = Convert.ToInt32(currentDriver.Uid);
                }
                else if (driverSelect.SelectedIndex == 1)
                {
                    executorIsDriver = true;
                }

                int? indexVehicle = null;

                if (vehicleSelect.SelectedIndex > 0)
                {
                    ComboBoxItem currentVehicle = vehicleSelect.SelectedItem as ComboBoxItem;

                    indexVehicle = Convert.ToInt32(currentVehicle.Uid);
                }

                string telephoneShipper = null;

                if (shipperTelephoneInput.IsMaskCompleted)
                {
                    telephoneShipper = shipperTelephoneInput.Text;
                }

                string telephoneConsignee = null;

                if (consigneeTelephoneInput.IsMaskCompleted)
                {
                    telephoneConsignee = consigneeTelephoneInput.Text;
                }

                if (_currentOrder == null)
                {
                    Orders orderCreate = new Orders()
                    {
                        Date_Order = dateOrderSelect.SelectedDate,
                        Code_Executor = indexExecutor,
                        Code_Driver = indexDriver,
                        Code_Vehicle = indexVehicle,
                        Date_Loading = dateLoadingSelect.Value,
                        Address_Loading = placeLoadingInput.Text,
                        Shipper_Name = shipperNameInput.Text,
                        Shipper_Telephone = telephoneShipper,
                        Cargo_Name = cargoNameInput.Text,
                        Conditions_Transportation = conditionTransportationInput.Text,
                        Date_Unloading = dateUnloadingSelect.Value,
                        Address_Unloading = placeUnloadingInput.Text,
                        Consignee_Name = consigneeNameInput.Text,
                        Consignee_Telephone = telephoneConsignee,
                        Route_Name = routeInput.Text,
                        Transportation_Format = conditionTransportationInput.Text,
                        Cost_Payment = costPaymentInput.Text,
                        Status_Order = Convert.ToBoolean(statusOrderSelect.SelectedIndex),
                        Executor_Is_Driver = executorIsDriver
                    };

                    DataBaseConnection.dataBaseEntities.Orders.Add(orderCreate);
                }
                else
                {
                    _currentOrder.Date_Order = dateOrderSelect.SelectedDate;
                    _currentOrder.Code_Executor = indexExecutor;
                    _currentOrder.Code_Driver = indexDriver;
                    _currentOrder.Code_Vehicle = indexVehicle;
                    _currentOrder.Date_Loading = Convert.ToDateTime(dateLoadingSelect.Text);
                    _currentOrder.Address_Loading = placeLoadingInput.Text;
                    _currentOrder.Shipper_Name = shipperNameInput.Text;
                    _currentOrder.Shipper_Telephone = telephoneShipper;
                    _currentOrder.Cargo_Name = cargoNameInput.Text;
                    _currentOrder.Conditions_Transportation = conditionTransportationInput.Text;
                    _currentOrder.Date_Unloading = Convert.ToDateTime(dateUnloadingSelect.Text);
                    _currentOrder.Address_Unloading = placeUnloadingInput.Text;
                    _currentOrder.Consignee_Name = consigneeNameInput.Text;
                    _currentOrder.Consignee_Telephone = telephoneConsignee;
                    _currentOrder.Route_Name = routeInput.Text;
                    _currentOrder.Transportation_Format = conditionTransportationInput.Text;
                    _currentOrder.Cost_Payment = costPaymentInput.Text;
                    _currentOrder.Status_Order = Convert.ToBoolean(statusOrderSelect.SelectedIndex);
                    _currentOrder.Executor_Is_Driver = executorIsDriver;
                }
                DataBaseConnection.dataBaseEntities.SaveChanges();


                MessageBox.Show("Операция выполнена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при добавлении. Проверьте вводимые данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteOrder_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow();

            confirmDelete.ShowDialog();

            if (confirmDelete.GetReturnCode() == 1)
            {
                DataBaseConnection.dataBaseEntities.Orders.Remove(_currentOrder);

                DataBaseConnection.dataBaseEntities.SaveChanges();

                MessageBox.Show("Операция выполнена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
        }
        private void convertToWord_Click(object sender, RoutedEventArgs e)
        {
            if (executorSelect.SelectedIndex != 0)
            {
                if (driverSelect.SelectedIndex != 0)
                {
                    if (vehicleSelect.SelectedIndex != 0)
                    {
                        string path = Environment.CurrentDirectory;
                        path = path.Replace("\\bin\\Debug", "\\maket.doc");
                        var helper = new WordHelper(path);

                        DateTime dateOrder = Convert.ToDateTime(dateOrderSelect.SelectedDate);

                        ComboBoxItem selectedExecutor = executorSelect.SelectedItem as ComboBoxItem;
                        int indexExecutor = Convert.ToInt32(selectedExecutor.Uid);
                        Executors executor = DataBaseConnection.dataBaseEntities.Executors.FirstOrDefault(x => x.Code_Executor == indexExecutor);

                        Personal_Data executorPersonalData = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == executor.Code_Personal_Data);

                        ComboBoxItem selectedVehicle = vehicleSelect.SelectedItem as ComboBoxItem;

                        int indexVehicle = Convert.ToInt32(selectedVehicle.Uid);

                        Vehicles vehicle = DataBaseConnection.dataBaseEntities.Vehicles.FirstOrDefault(x => x.Code_Vehicle == indexVehicle);

                        DateTime dateLoadingTime = Convert.ToDateTime(dateLoadingSelect.Value);
                        DateTime dateUnloadingTime = Convert.ToDateTime(dateLoadingSelect.Value);

                        var items = new Dictionary<string, string>();

                        if (driverSelect.SelectedIndex == 1)
                        {
                            DateTime dateExecutorPassport = Convert.ToDateTime(executorPersonalData.Passport_Date_Issued);
                            DateTime dateExecutorLicense = Convert.ToDateTime(executorPersonalData.Driver_License_Date_Issued);

                            items = new Dictionary<string, string>
                            {
                                { "<DATE_ORDER>", dateOrder.ToShortDateString() },
                                { "<EXECUTOR_NAME>", $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}" },
                                { "<DRIVER_NAME>", $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}"},
                                { "<TELEPHONE_DRIVER>", $"Телефон водителя: {executorPersonalData.Telephone_Number}"},
                                { "<DRIVER_PASSPORT_NUMBERS>", $"Паспорт: {executorPersonalData.Passport_Numbers}" },
                                { "<DRIVER_PASSPORT_DATE_ISSUED>", $"Выдан: {dateExecutorPassport.ToShortDateString()}"},
                                { "<DRIVER_PASSPORT_PLACE_ISSUED>", executorPersonalData.Passport_Issued},
                                { "<DRIVER_LICENSE_NUMBERS>", $"Водительское удостоверение: {executorPersonalData.Driver_License_Numbers}"},
                                { "<DRIVER_LICENSE_DATE_ISSUED>", $"Выдано: {dateExecutorLicense.ToShortDateString()}"},
                                { "<VEHICLE_BRAND>", vehicle.Brand_Vehicle},
                                { "<VEHICLE_MODEL>", vehicle.Model_Vehicle},
                                { "<VEHICLE_NUMBER>", $"Гос. номер: {vehicle.Number_Vehicle}"},
                                { "<DATE_LOADING>", $"{dateLoadingTime.ToShortDateString()} до {dateLoadingTime.ToShortTimeString()}"},
                                { "<ADDRESS_LOADING>", $"{placeLoadingInput.Text}"},
                                { "<SHIPPER_NAME>", $"{shipperNameInput.Text}"},
                                { "<TELEPHONE_SHIPPER>", $"{shipperTelephoneInput.Text}"},
                                { "<CARGO_NAME>", $"{cargoNameInput.Text}"},
                                { "<CONDITIONS_TRANSPORTATION>", $"{conditionTransportationInput.Text}"},
                                { "<DATE_UNLOADING>", $"{dateUnloadingTime.ToShortDateString()} к {dateUnloadingTime.ToShortTimeString()}"},
                                { "<ADDRESS_UNLOADING>", $"{placeUnloadingInput.Text}"},
                                { "<CONSIGNEE_NAME>", $"{consigneeNameInput.Text}"},
                                { "<TELEPHONE_CONSIGNEE>", $"{consigneeTelephoneInput.Text}"},
                                { "<ROUTE>", $"{routeInput.Text}"},
                                { "<TRANSPORTATION_FORMAT>", $"{routeFormatInput.Text}"},
                                { "<COST_PAYMENT>", $"{costPaymentInput.Text}"},
                                { "<TIN_NUMBER>", $"{executorPersonalData.Tin_Number}"},
                                { "<CODE_ATI>", $"{executorPersonalData.Id_Ati}"}
                            };
                        }
                        else if (driverSelect.SelectedIndex != 0)
                        {
                            ComboBoxItem selectedDriver = driverSelect.SelectedItem as ComboBoxItem;
                            int indexDriver = Convert.ToInt32(selectedDriver.Uid);
                            Drivers driver = DataBaseConnection.dataBaseEntities.Drivers.FirstOrDefault(x => x.Code_Driver == indexDriver);

                            Personal_Data personalDataDriver = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == driver.Code_Personal_Data);

                            DateTime dateDriverPassport = Convert.ToDateTime(personalDataDriver.Passport_Date_Issued);
                            DateTime dateDriverLicense = Convert.ToDateTime(personalDataDriver.Driver_License_Date_Issued);

                            items = new Dictionary<string, string>
                            {
                                { "<DATE_ORDER>", dateOrder.ToShortDateString() },
                                { "<EXECUTOR_NAME>", $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}" },
                                { "<DRIVER_NAME>", $"{driver.Surname_Driver} {driver.Name_Driver} {driver.Midname_Driver}"},
                                { "<TELEPHONE_DRIVER>", $"Телефон водителя: {personalDataDriver.Telephone_Number}"},
                                { "<DRIVER_PASSPORT_NUMBERS>", $"Паспорт: {personalDataDriver.Passport_Numbers}" },
                                { "<DRIVER_PASSPORT_DATE_ISSUED>", $"Выдан: {dateDriverPassport.ToShortDateString()}"},
                                { "<DRIVER_PASSPORT_PLACE_ISSUED>", personalDataDriver.Passport_Issued},
                                { "<DRIVER_LICENSE_NUMBERS>", $"Водительское удостоверение: {personalDataDriver.Driver_License_Numbers}"},
                                { "<DRIVER_LICENSE_DATE_ISSUED>", $"Выдано: {dateDriverLicense.ToShortDateString()}"},
                                { "<VEHICLE_BRAND>", $"Автомобиль {vehicle.Brand_Vehicle}" },
                                { "<VEHICLE_MODEL>", vehicle.Model_Vehicle},
                                { "<VEHICLE_NUMBER>", $"Гос. номер: {vehicle.Number_Vehicle}"},
                                { "<DATE_LOADING>", $"{dateLoadingTime.ToShortDateString()} до {dateLoadingTime.ToShortTimeString()}"},
                                { "<ADDRESS_LOADING>", $"{placeLoadingInput.Text}"},
                                { "<SHIPPER_NAME>", $"{shipperNameInput.Text}"},
                                { "<TELEPHONE_SHIPPER>", $"{shipperTelephoneInput.Text}"},
                                { "<CARGO_NAME>", $"{cargoNameInput.Text}"},
                                { "<CONDITIONS_TRANSPORTATION>", $"{conditionTransportationInput.Text}"},
                                { "<DATE_UNLOADING>", $"{dateUnloadingTime.ToShortDateString()} к {dateUnloadingTime.ToShortTimeString()}"},
                                { "<ADDRESS_UNLOADING>", $"{placeUnloadingInput.Text}"},
                                { "<CONSIGNEE_NAME>", $"{consigneeNameInput.Text}"},
                                { "<TELEPHONE_CONSIGNEE>", $"{consigneeTelephoneInput.Text}"},
                                { "<ROUTE>", $"{routeInput.Text}"},
                                { "<TRANSPORTATION_FORMAT>", $"{routeFormatInput.Text}"},
                                { "<COST_PAYMENT>", $"{costPaymentInput.Text}"},
                                { "<TIN_NUMBER>", $"{executorPersonalData.Tin_Number}"},
                                { "<CODE_ATI>", $"{executorPersonalData.Id_Ati}"}
                            };

                        }
                        string routeOrder = routeInput.Text;

                        helper.CreateWordDocument(items, routeOrder);
                    }
                    else
                    {
                        MessageBox.Show("Необходимо выбрать автомобиль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать водителя", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать исполнителя", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            


        }

        private void dateOrderSelect_GotFocus(object sender, RoutedEventArgs e)
        {
            dateOrderSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineDateOrder.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void dateOrderSelect_LostFocus(object sender, RoutedEventArgs e)
        {
            dateOrderSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineDateOrder.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void dateLoadingSelect_GotFocus(object sender, RoutedEventArgs e)
        {
            dateLoadingSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineLoading.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void dateLoadingSelect_LostFocus(object sender, RoutedEventArgs e)
        {
            dateLoadingSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineLoading.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void dateUnloadingSelect_GotFocus(object sender, RoutedEventArgs e)
        {
            dateUnloadingSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineUnloading.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void dateUnloadingSelect_LostFocus(object sender, RoutedEventArgs e)
        {
            dateUnloadingSelect.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineUnloading.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }
    }
}
