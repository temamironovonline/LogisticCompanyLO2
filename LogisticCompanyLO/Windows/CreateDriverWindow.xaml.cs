using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace LogisticCompanyLO
{
    public partial class CreateDriverWindow : Window
    {
        public CreateDriverWindow()
        {
            InitializeComponent();

            UploadStartUpInfo();

        }

        private Drivers _currentDriver;
        private Personal_Data _currentDriverPersonalData;

        public CreateDriverWindow(Drivers driver)
        {
            InitializeComponent();

            UploadStartUpInfo();

            headerWindow.Text = "ИЗМЕНЕНИЕ ВОДИТЕЛЯ";

            deleteDriver.Visibility = Visibility.Visible;

            _currentDriver = driver;

            surnameDriverInput.Text = _currentDriver.Surname_Driver;
            nameDriverInput.Text = _currentDriver.Name_Driver;
            midnameDriverInput.Text = _currentDriver.Midname_Driver;
            boundariesWorkDriverInput.Text = _currentDriver.Boundaries_Work;
            foreach (ComboBoxItem executorItem in executorSelect.Items)
            {
                if (_currentDriver.Code_Executor == Convert.ToInt32(executorItem.Uid))
                {
                    executorSelect.SelectedItem = executorItem;
                    break;
                }
            }

            _currentDriverPersonalData = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == _currentDriver.Code_Personal_Data);

            telephoneDriverInput.Text = _currentDriverPersonalData.Telephone_Number;
            idAtiDriverInput.Text = Convert.ToString(_currentDriverPersonalData.Id_Ati);
            numbersPassportDriverInput.Text = _currentDriverPersonalData.Passport_Numbers;
            divisionCodePassportDriverInput.Text = _currentDriverPersonalData.Passport_Division_Code;
            dateIssuedPassportDriverInput.SelectedDate = _currentDriverPersonalData.Passport_Date_Issued;
            placeIssuedPassportDriverInput.Text = _currentDriverPersonalData.Passport_Issued;
            driverLicenseDriverInput.Text = _currentDriverPersonalData.Driver_License_Numbers;
            dateIssuedDriverLicenseDriverInput.SelectedDate = _currentDriverPersonalData.Driver_License_Date_Issued;
        }

        private ListView _currentDriverListView;

        public CreateDriverWindow(ListView driverList)
        {
            InitializeComponent();

            UploadStartUpInfo();

            executorPanel.Visibility = Visibility.Collapsed;

            surnameDriverPanel.Margin = new Thickness(5, 5, 5, 5);

            _currentDriverListView = driverList;

            this.Width = 610;
            this.Height = 550;

        }

        public CreateDriverWindow(ListView driverList, Drivers driver)
        {
            InitializeComponent();

            UploadStartUpInfo();

            this.Width = 610;
            this.Height = 550;

            executorPanel.Visibility = Visibility.Collapsed;

            surnameDriverPanel.Margin = new Thickness(5, 5, 5, 5);

            _currentDriverListView = driverList;

            _currentDriver = driver;

            _currentDriverPersonalData = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == _currentDriver.Code_Personal_Data);

            headerWindow.Text = "ИЗМЕНЕНИЕ ВОДИТЕЛЯ";

            surnameDriverInput.Text = _currentDriver.Surname_Driver;
            nameDriverInput.Text = _currentDriver.Name_Driver;
            midnameDriverInput.Text = _currentDriver.Midname_Driver;
            telephoneDriverInput.Text = _currentDriver.Telephone_Number_Driver;
            idAtiDriverInput.Text = Convert.ToString(_currentDriver.Id_Ati_Driver);
            numbersPassportDriverInput.Text = _currentDriver.Passport_Numbers_Driver;
            divisionCodePassportDriverInput.Text = _currentDriver.Passport_Division_Code_Driver;
            dateIssuedPassportDriverInput.SelectedDate = _currentDriver.Passport_Date_Issued_Driver;
            placeIssuedPassportDriverInput.Text = _currentDriver.Passport_Place_Issued_Driver;
            driverLicenseDriverInput.Text = _currentDriver.Driver_License_Driver;
            dateIssuedDriverLicenseDriverInput.SelectedDate = _currentDriver.Driver_License_Date_Issued_Driver;
            boundariesWorkDriverInput.Text = _currentDriver.Boundaries_Work;
        }

        private void UploadStartUpInfo()
        {
            ComboBoxItem defaultSelect = new ComboBoxItem()
            {
                Content = "Не выбрано",
                Uid = "0"
            };

            executorSelect.Items.Add(defaultSelect);

            List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();

            foreach (Executors executor in executors)
            {
                ComboBoxItem executorItem = new ComboBoxItem()
                {
                    Content = $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}",
                    Uid = $"{executor.Code_Executor}"
                };
                executorSelect.Items.Add(executorItem);
            }

            executorSelect.SelectedIndex = 0;
        }
        private void addDriver_Click(object sender, RoutedEventArgs e)
        {
            string numbersPassportDriver = null;

            if (numbersPassportDriverInput.IsMaskCompleted)
            {
                numbersPassportDriver = numbersPassportDriverInput.Text;
            }

            string telephoneDriver = null;

            if (telephoneDriverInput.IsMaskCompleted)
            {
                telephoneDriver = telephoneDriverInput.Text;
            }

            int? idAtiDriver = null;

            if (idAtiDriverInput.Text != "")
            {
                idAtiDriver = Convert.ToInt32(idAtiDriverInput.Text);
            }

            string driverLicense = null;

            if (driverLicenseDriverInput.IsMaskCompleted)
            {
                driverLicense = driverLicenseDriverInput.Text;
            }

            int returnCode;
            
            if (_currentDriver == null)
            {
                returnCode = SameDrivers.CheckSameDriversCreate(numbersPassportDriver, idAtiDriver, telephoneDriver, driverLicense); // Проверка имеющейся записи при добавлении
            }
            else
            {
                returnCode = SameDrivers.CheckSameDriversUpdate(_currentDriver, numbersPassportDriver, idAtiDriver, telephoneDriver, driverLicense); // Проверка имеющейся записи при изменении
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

                default: // Если похожих записей не было найдено

                    string passportDivisionCode = null;

                    if (divisionCodePassportDriverInput.IsMaskCompleted) // Если маска с кодом подразделения паспорта заполнена полностью, то присваиваем его переменной, иначе переменная остается null
                    {
                        passportDivisionCode = divisionCodePassportDriverInput.Text;
                    }

                    if (_currentDriver == null)
                    {
                        CreateDriver(numbersPassportDriver, passportDivisionCode, idAtiDriver, telephoneDriver, driverLicense);
                    }
                    else
                    {
                        UpdateDriver(numbersPassportDriver, passportDivisionCode, idAtiDriver, telephoneDriver, driverLicense);
                    }
                   
                    break;
            }
        }

        private void CreateDriver(string passportNumbers, string passportDivisionCode, int? idAti, string telephoneDriver, string driverLicense)
        {
            if (_currentDriverListView == null)
            {
                Personal_Data personalData = new Personal_Data()
                {
                    Passport_Numbers = passportNumbers,
                    Passport_Division_Code = passportDivisionCode,
                    Passport_Date_Issued = dateIssuedPassportDriverInput.SelectedDate,
                    Passport_Issued = placeIssuedPassportDriverInput.Text,
                    Id_Ati = idAti,
                    Telephone_Number = telephoneDriver,
                    Driver_License_Numbers = driverLicense,
                    Driver_License_Date_Issued = dateIssuedDriverLicenseDriverInput.SelectedDate,
                };

                DataBaseConnection.dataBaseEntities.Personal_Data.Add(personalData);
                DataBaseConnection.dataBaseEntities.SaveChanges();

                int? indexExecutor = null;

                if (executorSelect.SelectedIndex > 0)
                {
                    ComboBoxItem executorItem = executorSelect.SelectedItem as ComboBoxItem;
                    indexExecutor = Convert.ToInt32(executorItem.Uid);
                }

                Drivers driver = new Drivers()
                {
                    Surname_Driver = surnameDriverInput.Text,
                    Name_Driver = nameDriverInput.Text,
                    Midname_Driver = midnameDriverInput.Text,
                    Code_Personal_Data = personalData.Code_Personal_Data,
                    Code_Executor = indexExecutor,
                    Boundaries_Work = boundariesWorkDriverInput.Text
                };

                DataBaseConnection.dataBaseEntities.Drivers.Add(driver);
                DataBaseConnection.dataBaseEntities.SaveChanges();
                MessageBox.Show("Операция выполнена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Drivers driver = new Drivers()
                {
                    Surname_Driver = surnameDriverInput.Text,
                    Name_Driver = nameDriverInput.Text,
                    Midname_Driver = midnameDriverInput.Text,
                    Passport_Numbers_Driver = passportNumbers,
                    Passport_Division_Code_Driver = passportDivisionCode,
                    Passport_Date_Issued_Driver = dateIssuedPassportDriverInput.SelectedDate,
                    Passport_Place_Issued_Driver = placeIssuedPassportDriverInput.Text,
                    Telephone_Number_Driver = telephoneDriver,
                    Id_Ati_Driver = idAti,
                    Driver_License_Driver = driverLicense,
                    Driver_License_Date_Issued_Driver = dateIssuedDriverLicenseDriverInput.SelectedDate,
                    Boundaries_Work = boundariesWorkDriverInput.Text
                };
                _currentDriverListView.Items.Add(driver);
            }
            this.Close();
        }

        private void UpdateDriver(string passportNumbers, string passportDivisionCode, int? idAti, string telephoneDriver, string driverLicense)
        {
            if (_currentDriverListView == null)
            {
                _currentDriverPersonalData.Passport_Numbers = passportNumbers;
                _currentDriverPersonalData.Passport_Division_Code = passportDivisionCode;
                _currentDriverPersonalData.Passport_Date_Issued = dateIssuedPassportDriverInput.SelectedDate;
                _currentDriverPersonalData.Passport_Issued = placeIssuedPassportDriverInput.Text;
                _currentDriverPersonalData.Id_Ati = idAti;
                _currentDriverPersonalData.Telephone_Number = telephoneDriver;
                _currentDriverPersonalData.Driver_License_Numbers = driverLicense;
                _currentDriverPersonalData.Driver_License_Date_Issued = dateIssuedDriverLicenseDriverInput.SelectedDate;

                int? indexExecutor = null;

                if (executorSelect.SelectedIndex > 0)
                {
                    ComboBoxItem executorItem = executorSelect.SelectedItem as ComboBoxItem;
                    indexExecutor = Convert.ToInt32(executorItem.Uid);
                }

                _currentDriver.Surname_Driver = surnameDriverInput.Text;
                _currentDriver.Name_Driver = nameDriverInput.Text;
                _currentDriver.Midname_Driver = midnameDriverInput.Text;
                _currentDriver.Code_Executor = indexExecutor;
                _currentDriver.Boundaries_Work = boundariesWorkDriverInput.Text;

                DataBaseConnection.dataBaseEntities.SaveChanges();
                MessageBox.Show("Операция выполнена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _currentDriver.Surname_Driver = surnameDriverInput.Text;
                _currentDriver.Name_Driver = nameDriverInput.Text;
                _currentDriver.Midname_Driver = midnameDriverInput.Text;
                _currentDriver.Passport_Numbers_Driver = passportNumbers;
                _currentDriver.Passport_Division_Code_Driver = passportDivisionCode;
                _currentDriver.Passport_Date_Issued_Driver = dateIssuedPassportDriverInput.SelectedDate;
                _currentDriver.Passport_Place_Issued_Driver = placeIssuedPassportDriverInput.Text;
                _currentDriver.Id_Ati_Driver = idAti;
                _currentDriver.Telephone_Number_Driver = telephoneDriver;
                _currentDriver.Driver_License_Driver = driverLicense;
                _currentDriver.Driver_License_Date_Issued_Driver = dateIssuedDriverLicenseDriverInput.SelectedDate;
                _currentDriver.Boundaries_Work = boundariesWorkDriverInput.Text;

                if (_currentDriverPersonalData != null)
                {
                    _currentDriverPersonalData.Passport_Numbers = _currentDriver.Passport_Numbers_Driver;
                    _currentDriverPersonalData.Passport_Division_Code = _currentDriver.Passport_Division_Code_Driver;
                    _currentDriverPersonalData.Passport_Date_Issued = _currentDriver.Passport_Date_Issued_Driver;
                    _currentDriverPersonalData.Passport_Issued = _currentDriver.Passport_Place_Issued_Driver;
                    _currentDriverPersonalData.Id_Ati = _currentDriver.Id_Ati_Driver;
                    _currentDriverPersonalData.Telephone_Number = _currentDriver.Telephone_Number_Driver;
                    _currentDriverPersonalData.Driver_License_Numbers = _currentDriver.Driver_License_Driver;
                    _currentDriverPersonalData.Driver_License_Date_Issued = _currentDriver.Driver_License_Date_Issued_Driver;
                }
            }
            this.Close();
        }

        private void idAtiDriverInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;    
            }
        }

        private void deleteDriver_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow();

            confirmDelete.ShowDialog();

            if (confirmDelete.GetReturnCode() == 1)
            {
                Personal_Data personalDataDriver = DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Code_Personal_Data == _currentDriver.Code_Personal_Data);

                DataBaseConnection.dataBaseEntities.Personal_Data.Remove(personalDataDriver);

                DataBaseConnection.dataBaseEntities.SaveChanges();

                MessageBox.Show("Удаление прошло успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
        }

        private void dateIssuedPassportDriverInput_GotFocus(object sender, RoutedEventArgs e)
        {
            dateIssuedPassportDriverInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineDateIssuedPassportDriver.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void dateIssuedPassportDriverInput_LostFocus(object sender, RoutedEventArgs e)
        {
            dateIssuedPassportDriverInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineDateIssuedPassportDriver.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void dateIssuedDriverLicenseDriverInput_GotFocus(object sender, RoutedEventArgs e)
        {
            dateIssuedDriverLicenseDriverInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            underlineDriverLicenseDriver.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void dateIssuedDriverLicenseDriverInput_LostFocus(object sender, RoutedEventArgs e)
        {
            dateIssuedDriverLicenseDriverInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            underlineDriverLicenseDriver.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }
    }
}
