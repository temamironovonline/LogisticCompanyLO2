using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace LogisticCompanyLO
{
    public partial class AddExecutorWindow : Window
    {
        public AddExecutorWindow() // Конструктор при добавлении исполнителя
        {
            InitializeComponent();
        }

        private Executors _currentExecutor;
        private Personal_Data _personalData;

        public AddExecutorWindow(Executors executor) // Конструктор при изменении исполнителя
        {
            InitializeComponent();

            this.Closing += AddExecutorWindow_Closing;

            _currentExecutor = executor;

            executorSurnameInput.Text = _currentExecutor.Surname_Executor;
            executorNameInput.Text = _currentExecutor.Name_Executor;
            executorMidnameInput.Text = _currentExecutor.Midname_Executor;
            executorBoundariesWorkInput.Text = _currentExecutor.Boundaries_Work;

            //Поскольку персональные данные по водителям и исполнителям вынесены в отдельную таблицу, их нужно подгрузить из БД
            _personalData = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == _currentExecutor.Code_Personal_Data);

            if (_personalData != null)
            {
                executorIdAti.Text = _personalData.Id_Ati.ToString();
                executorTelephoneNumberInput.Text = _personalData.Telephone_Number;
                executorPassportNumbers.Text = _personalData.Passport_Numbers;
                executorPassportDivisionCode.Text = _personalData.Passport_Division_Code;
                executorPassportDateIssued.SelectedDate = _personalData.Passport_Date_Issued;
                executorPassportIssued.Text = _personalData.Passport_Issued;
                executorDriverLicenseInput.Text = _personalData.Driver_License_Numbers;
                executorTinNumberInput.Text = _personalData.Tin_Number;
                executorDriverLicenseDateIssued.SelectedDate = _personalData.Driver_License_Date_Issued;
            }

            // Подгружаем все автомобили выбранного исполнителя и добавляем в ListView
            List<Vehicles> vehiclesCurrentExecutor = DataBaseConnection.dataBaseEntities.Vehicles.Where(x => x.Code_Executor == _currentExecutor.Code_Executor).ToList();

            foreach(Vehicles currentVehicle in vehiclesCurrentExecutor)
            {
                vehiclesList.Items.Add(currentVehicle);
            }

            // Подгружаем всех водителей выбранного исполнителя и добавляем в ListView
            List<Drivers> driversCurrentExecutor = DataBaseConnection.dataBaseEntities.Drivers.Where(x => x.Code_Executor == _currentExecutor.Code_Executor).ToList();

            // В ListView с водителями нужно отображать данные из двух таблиц - Водители и Персональные данные. Соответственно, таблица Водители связана с таблицей Персональные данные, и если не добавить
            // запись по персональным данным в БД и не сохранить эти изменения, то данные в ListView, где есть связь, не будут отображаться. Поэтому был создан частичный класс (PersonalDataDrivers),
            // в котором для Водителя были продублированы поля из таблицы с персональными данными. И все манипуляции происходят с помощью этих полей из частичного класса (Passport_Numbers_Driver,
            // Passport_Division_Code_Driver, Passport_Date_Issued_Driver, Passport_Place_Issued_Driver, Telephone_Number_Driver, Id_Ati_Driver, Driver_License_Numbers_Driver)
            foreach(Drivers currentDriver in driversCurrentExecutor)
            {
                Personal_Data personalDataCurrentDriver = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == currentDriver.Code_Personal_Data);
                currentDriver.Passport_Numbers_Driver = personalDataCurrentDriver.Passport_Numbers;
                currentDriver.Passport_Division_Code_Driver = personalDataCurrentDriver.Passport_Division_Code;
                currentDriver.Passport_Date_Issued_Driver = personalDataCurrentDriver.Passport_Date_Issued;
                currentDriver.Passport_Place_Issued_Driver = personalDataCurrentDriver.Passport_Issued;
                currentDriver.Telephone_Number_Driver = personalDataCurrentDriver.Telephone_Number;
                currentDriver.Id_Ati_Driver = personalDataCurrentDriver.Id_Ati;
                currentDriver.Driver_License_Driver = personalDataCurrentDriver.Driver_License_Numbers;
                currentDriver.Driver_License_Date_Issued_Driver = personalDataCurrentDriver.Driver_License_Date_Issued;
                driversList.Items.Add(currentDriver);
            }

            title.Text = "ИЗМЕНЕНИЕ ИСПОЛНИТЕЛЯ"; // Изменение заголовка
            
            deleteExecutor.Visibility = Visibility.Visible;
        }

        private void AddExecutorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataBaseConnection.dataBaseEntities.SaveChanges();
        }

        private void addVehicle_Click(object sender, RoutedEventArgs e) // Добавление автомобиля
        {
            CreateVehicleWindow createVehicle = new CreateVehicleWindow(vehiclesList);

            createVehicle.ShowDialog();

            vehiclesList.Items.Refresh();
        }

        private void vehiclesList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) // Изменение добавленного автомобиля
        {
            Vehicles vehicle = (Vehicles)vehiclesList.SelectedItem;

            CreateVehicleWindow createVehicle = new CreateVehicleWindow(vehiclesList, vehicle);
            
            createVehicle.ShowDialog();
            
            vehiclesList.Items.Refresh();
        }

        private void createExecutor_Click(object sender, RoutedEventArgs e) // Создание исполнителя
        {
            // Сначала необходимо проверить значения, которые не могут быть одинаковыми - Серия-номер паспорта, код АТИ и номер телефона исполнителя
            // Для этого их нужно присвоить отдельным переменным
            
            string passportNumbers = null; // Серия номер паспорта. По умолчанию null

            if (executorPassportNumbers.IsMaskCompleted)
            {
                passportNumbers = executorPassportNumbers.Text;
            }

            int? idAti = null; // Код АТИ исполнителя. По умолчанию null

            if (executorIdAti.Text != "")
            {
                idAti = Convert.ToInt32(executorIdAti.Text);
            }

            string telephoneExecutor = null; // Телефон исполнителя. По умолчанию null

            // Если маска не заполнена, то присваивается значение null
            if (executorTelephoneNumberInput.IsMaskCompleted)
            {
                telephoneExecutor = executorTelephoneNumberInput.Text;
            }

            string driverLicense = null;

            if (executorDriverLicenseInput.IsMaskCompleted)
            {
                driverLicense = executorDriverLicenseInput.Text;
            }

            string tinNumber = null;

            if (executorTinNumberInput.IsMaskCompleted)
            {
                tinNumber = executorTinNumberInput.Text;
            }

            int returnCode; // Возвращаемое значение для проверки похожих значений

            if (_currentExecutor == null) // Если исполнитель добавляется, то вызываем метод из другого класса для проверки при добавлении
            {
                returnCode = SameExecutors.CheckSameExecutorsCreate(passportNumbers, idAti, telephoneExecutor, driverLicense, tinNumber);
            }
            else // Если исполнитель изменяется, то вызываем метод из другого класса для проверки при изменении
            {
                returnCode = SameExecutors.CheckSameExecutorsUpdate(_currentExecutor, passportNumbers, idAti, telephoneExecutor, driverLicense, tinNumber);
            }

            switch (returnCode) // В зависимости от возвращаемого значения либо выдаст ошибку о том, что запись с такими данными уже имеется в бд,
                                // либо продолжит свое выполнение в одной из двух ветвей (добавление или изменение) (см. блок default)
            {
                case 1:
                    MessageBox.Show("Паспорт с такой серией и номером уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case 2:
                    MessageBox.Show("Код АТИ уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case 3:
                    MessageBox.Show("Номер телефона уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case 4:
                    MessageBox.Show("Водительское удостоверение уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case 5:
                    MessageBox.Show("Номер ИНН уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                default: // Если похожих записей не было найдено

                    string passportDivisionCode = null;

                    if (executorPassportDivisionCode.IsMaskCompleted) // Если маска с кодом подразделения паспорта заполнена полностью, то присваиваем его переменной, иначе переменная остается null
                    {
                        passportDivisionCode = executorPassportDivisionCode.Text;
                    }
                    if (_currentExecutor == null) // Если исполнитель добавляется в бд
                    {
                        CreateExecutor(passportNumbers, passportDivisionCode, idAti, telephoneExecutor, driverLicense, tinNumber);
                    }
                    else // Если изменяются данные об исполнителе в бд
                    {
                        UpdateExecutor(passportNumbers, passportDivisionCode, idAti, telephoneExecutor, driverLicense, tinNumber);
                    }
                    break;
            }
        }

        private void CreateExecutor(string passportNumbers, string passportDivisionCode, int? idAti, string telephoneNumber, string driverLicense, string tinNumber)
        {
            Personal_Data personalData = new Personal_Data()
            {
                Passport_Numbers = passportNumbers,
                Passport_Division_Code = passportDivisionCode,
                Passport_Date_Issued = executorPassportDateIssued.SelectedDate,
                Passport_Issued = executorPassportIssued.Text,
                Id_Ati = idAti,
                Telephone_Number = telephoneNumber,
                Driver_License_Numbers = driverLicense,
                Driver_License_Date_Issued = executorDriverLicenseDateIssued.SelectedDate,
                Tin_Number = tinNumber
            };

            DataBaseConnection.dataBaseEntities.Personal_Data.Add(personalData);
            DataBaseConnection.dataBaseEntities.SaveChanges();

            Executors executor = new Executors()
            {
                Surname_Executor = executorSurnameInput.Text,
                Name_Executor = executorNameInput.Text,
                Midname_Executor = executorMidnameInput.Text,
                Code_Personal_Data = personalData.Code_Personal_Data,
                Boundaries_Work = executorBoundariesWorkInput.Text
            };

            DataBaseConnection.dataBaseEntities.Executors.Add(executor);
            DataBaseConnection.dataBaseEntities.SaveChanges();

            foreach (Vehicles vehicle in vehiclesList.Items) // Добавление автомобилей с присваиванием им номера исполнителя
            {
                vehicle.Code_Executor = executor.Code_Executor;
                DataBaseConnection.dataBaseEntities.Vehicles.Add(vehicle);
            }

            foreach (Drivers driver in driversList.Items) // Добавляем водителей и их персональные данные
            {
                Personal_Data personalDataDriver = new Personal_Data()
                {
                    Passport_Numbers = driver.Passport_Numbers_Driver,
                    Passport_Division_Code = driver.Passport_Division_Code_Driver,
                    Passport_Date_Issued = driver.Passport_Date_Issued_Driver,
                    Passport_Issued = driver.Passport_Place_Issued_Driver,
                    Telephone_Number = driver.Telephone_Number_Driver,
                    Id_Ati = driver.Id_Ati_Driver,
                    Driver_License_Numbers = driver.Driver_License_Driver,
                    Driver_License_Date_Issued = driver.Driver_License_Date_Issued_Driver
                };

                DataBaseConnection.dataBaseEntities.Personal_Data.Add(personalDataDriver);
                DataBaseConnection.dataBaseEntities.SaveChanges();

                driver.Code_Executor = executor.Code_Executor;
                driver.Code_Personal_Data = personalDataDriver.Code_Personal_Data;

                DataBaseConnection.dataBaseEntities.Drivers.Add(driver);
            }

            DataBaseConnection.dataBaseEntities.SaveChanges();
            MessageBox.Show("Добавление прошло успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void UpdateExecutor(string passportNumbers, string passportDivisionCode, int? idAti, string telephoneNumber, string driverLicense, string tinNumber) // Изменение исполнителя
        {
            _personalData.Passport_Numbers = passportNumbers;
            _personalData.Passport_Division_Code = passportDivisionCode;
            _personalData.Passport_Date_Issued = executorPassportDateIssued.SelectedDate;
            _personalData.Passport_Issued = executorPassportIssued.Text;
            _personalData.Id_Ati = idAti;
            _personalData.Telephone_Number = telephoneNumber;
            _personalData.Driver_License_Numbers = driverLicense;
            _personalData.Tin_Number = tinNumber;
            _personalData.Driver_License_Date_Issued = executorDriverLicenseDateIssued.SelectedDate;

            _currentExecutor.Surname_Executor = executorSurnameInput.Text;
            _currentExecutor.Name_Executor = executorNameInput.Text;
            _currentExecutor.Midname_Executor = executorMidnameInput.Text;
            _currentExecutor.Boundaries_Work = executorBoundariesWorkInput.Text;

            List<Vehicles> vehicles = DataBaseConnection.dataBaseEntities.Vehicles.Where(x => x.Code_Executor == _currentExecutor.Code_Executor).ToList();

            foreach (Vehicles vehicle in vehiclesList.Items)
            {
                if (vehicles.FirstOrDefault(x => x.Code_Vehicle == vehicle.Code_Vehicle) == null) // Если автомобиля нет в уже имеющихся у исполнителя, то он добавляется
                {
                    vehicle.Code_Executor = _currentExecutor.Code_Executor;
                    DataBaseConnection.dataBaseEntities.Vehicles.Add(vehicle);
                }
            }

            List<Drivers> drivers = DataBaseConnection.dataBaseEntities.Drivers.Where(x => x.Code_Executor == _currentExecutor.Code_Executor).ToList();

            foreach(Drivers driver in driversList.Items)
            {
                if (drivers.FirstOrDefault(x => x.Code_Driver == driver.Code_Driver) == null) // Если водителя нет в уже имеющихся у исполнителя, то он добавляется
                {
                    Personal_Data personalDataCurrentDriver = new Personal_Data()
                    {
                        Passport_Numbers = driver.Passport_Numbers_Driver,
                        Passport_Division_Code = driver.Passport_Division_Code_Driver,
                        Passport_Date_Issued = driver.Passport_Date_Issued_Driver,
                        Passport_Issued = driver.Passport_Place_Issued_Driver,
                        Telephone_Number = driver.Telephone_Number_Driver,
                        Id_Ati = driver.Id_Ati_Driver,
                        Driver_License_Numbers = driver.Driver_License_Driver,
                        Driver_License_Date_Issued = driver.Driver_License_Date_Issued_Driver
                    };

                    DataBaseConnection.dataBaseEntities.Personal_Data.Add(personalDataCurrentDriver);
                    DataBaseConnection.dataBaseEntities.SaveChanges();

                    driver.Code_Executor = _currentExecutor.Code_Executor;
                    driver.Code_Personal_Data = personalDataCurrentDriver.Code_Personal_Data;
                    DataBaseConnection.dataBaseEntities.Drivers.Add(driver);
                }
            }
            DataBaseConnection.dataBaseEntities.SaveChanges();
            MessageBox.Show("Изменение прошло успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void executorIdAti_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) // Разрешен ввод только чисел
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void deleteExecutor_Click(object sender, RoutedEventArgs e) // Удаление исполнителя
        {
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow();
            confirmDelete.ShowDialog();
            if (confirmDelete.GetReturnCode() == 1)
            {
                if (confirmDelete.GetReturnCode() == 1)
                {
                    List<Drivers> drivers = DataBaseConnection.dataBaseEntities.Drivers.Where(x => x.Code_Executor == _currentExecutor.Code_Executor).ToList(); // Находим список водителей, принадлежащих удаляемому исполнителю

                    // Удаление данных происходит через таблицу Personal_Data с помощью каскадного удаления
                    foreach (Drivers driver in drivers) // Удаляем водителей
                    {
                        Personal_Data personalDataDriver = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == driver.Code_Personal_Data);
                        DataBaseConnection.dataBaseEntities.Personal_Data.Remove(personalDataDriver);
                    }

                    Personal_Data personalDataExecutor = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == _currentExecutor.Code_Personal_Data);
                    DataBaseConnection.dataBaseEntities.Personal_Data.Remove(personalDataExecutor); // Удаляем исполнителя
                    DataBaseConnection.dataBaseEntities.SaveChanges();
                    MessageBox.Show("Удаление прошло успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Question);
                }
            }
            this.Close();
        }

        private void deleteVehicle_Click(object sender, RoutedEventArgs e)
        {
            Vehicles vehicleDelete = (Vehicles)vehiclesList.SelectedItem;
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow(); // Окно с подтверждением удаления
            confirmDelete.ShowDialog();
            if (confirmDelete.GetReturnCode() == 1) // Если возвращаемое значение == 1 (т.е удаление было подтверждено), то запись удаляется 
            {
                try
                {
                    if (DataBaseConnection.dataBaseEntities.Vehicles.FirstOrDefault(x => x.Code_Vehicle == vehicleDelete.Code_Vehicle) != null)
                    {
                        DataBaseConnection.dataBaseEntities.Vehicles.Remove(vehicleDelete);
                    }
                    vehiclesList.Items.Remove(vehicleDelete);
                    DataBaseConnection.dataBaseEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка во время удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void addDriver_Click(object sender, RoutedEventArgs e)
        {
            CreateDriverWindow createDriverWindow = new CreateDriverWindow(driversList);

            createDriverWindow.ShowDialog();

            driversList.Items.Refresh();
        }

        private void driversList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Drivers driver = (Drivers)driversList.SelectedItem;

            CreateDriverWindow createDriverWindow = new CreateDriverWindow(driversList, driver);

            createDriverWindow.ShowDialog();

            driversList.Items.Refresh();
        }

        private void deleteDriver_Click(object sender, RoutedEventArgs e)
        {
            Drivers driverDelete = (Drivers)driversList.SelectedItem;
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow(); // Окно с подтверждением удаления
            confirmDelete.ShowDialog();
            if (confirmDelete.GetReturnCode() == 1) // Если возвращаемое значение == 1 (т.е удаление было подтверждено), то запись удаляется 
            {
                try
                {
                    if (DataBaseConnection.dataBaseEntities.Drivers.FirstOrDefault(x => x.Code_Driver == driverDelete.Code_Driver) != null)
                    { 
                        DataBaseConnection.dataBaseEntities.Drivers.Remove(driverDelete);
                    }
                    driversList.Items.Remove(driverDelete);
                    DataBaseConnection.dataBaseEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка во время удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void executorDriverLicenseDateIssued_GotFocus(object sender, RoutedEventArgs e)
        {
            executorDriverLicenseDateIssued.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineExecutorDriverLicenseDateIssed.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void executorDriverLicenseDateIssued_LostFocus(object sender, RoutedEventArgs e)
        {
            executorDriverLicenseDateIssued.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineExecutorDriverLicenseDateIssed.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void executorPassportDateIssued_GotFocus(object sender, RoutedEventArgs e)
        {
            executorPassportDateIssued.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineExecutorPassportDateIssed.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void executorPassportDateIssued_LostFocus(object sender, RoutedEventArgs e)
        {
            executorPassportDateIssued.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineExecutorPassportDateIssed.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }
    }
}
