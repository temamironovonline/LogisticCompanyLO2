using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;

namespace LogisticCompanyLO
{
    public partial class CreateVehicleWindow : Window
    {
        public CreateVehicleWindow() // Конструктор при добавлении автомобиля со страницы со списком автомобилей
        {
            InitializeComponent();

            SetInformationToComboBox();
        }

        public CreateVehicleWindow(Vehicles vehicle) // Конструктор при изменении автомобиля со страницы со списком автомобилей
        {
            InitializeComponent();

            deleteVehicle.Visibility = Visibility.Visible; // Видимость кнопки удаления

            SetInformationToComboBox();

            SetVehicleInformationForUpdate(vehicle);

        }

        private ListView _vehicleListFromExecutors;

        public CreateVehicleWindow(ListView vehiclesList) // Конструктор добавления автомобиля при добавлении/редактировании исполнителя
        {
            InitializeComponent();

            SetInformationToComboBox();

            _vehicleListFromExecutors = vehiclesList;

            executorStackPanel.Visibility = Visibility.Collapsed; // Поскольку добавление автомобиля происходит из окна создания исполнителя, то combobox с выбором исполнителя (владельца) автомобиля не нужен, его можно скрыть

            categoryOfTrailerPanel.Margin = new Thickness(5, 5, 5, 5);
        }

        public CreateVehicleWindow(ListView vehicleList, Vehicles vehicle) // Конструктор добавления автомобиля при изменении автомобиля из окна добавления/редактирования исполнителя 
        {
            InitializeComponent();

            SetInformationToComboBox();

            _vehicleListFromExecutors = vehicleList;

            executorStackPanel.Visibility = Visibility.Collapsed; // Поскольку добавление автомобиля происходит из окна создания исполнителя, то combobox с выбором исполнителя (владельца) автомобиля не нужен, его можно скрыть

            categoryOfTrailerPanel.Margin = new Thickness(5, 5, 5, 5);

            SetVehicleInformationForUpdate(vehicle);
        }

        private void SetInformationToComboBox() // Подрузка данных из БД в ComboBoxes
        {
            ComboBoxItem defaultItemExecutor = new ComboBoxItem() // Нулевое значение для combobox с исполнителями
            {
                Content = "Не выбрано",
                Uid = "0"
            };

            executorBox.Items.Add(defaultItemExecutor);

            List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();

            foreach (Executors executor in executors) // Элементы в Combobox добавляются с помощью ComboBoxItems, в которых значение Content == ФИО исполнителя, а Uid == ID записи в БД
            {
                ComboBoxItem executorAdd = new ComboBoxItem()
                {
                    Content = $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}",
                    Uid = $"{executor.Code_Executor}"
                };
                executorBox.Items.Add(executorAdd);
            }
            executorBox.SelectedIndex = 0;

            //
            // Заполнение остальных ComboBoxes работает по тому же принципу
            //

            //
            // Заполнение CB с категориями прицепов
            //
            ComboBoxItem defaultItemCategory = new ComboBoxItem()
            {
                Content = "Не выбрано",
                Uid = "0"
            };

            categoryOfTrailerBox.Items.Add(defaultItemCategory);

            List<Category_Trailer> categoryTrailers = DataBaseConnection.dataBaseEntities.Category_Trailer.ToList();

            foreach (Category_Trailer category in categoryTrailers)
            {
                ComboBoxItem categoryAdd = new ComboBoxItem()
                {
                    Content = $"{category.Name_Trailer}",
                    Uid = $"{category.Code_Trailer}"
                };

                categoryOfTrailerBox.Items.Add(categoryAdd);
            }
            categoryOfTrailerBox.SelectedIndex = 0;

            //
            // Заполнение CB с типами загрузки
            //
            ComboBoxItem defaultItemDownloadType = new ComboBoxItem()
            {
                Content = "Не выбрано",
                Uid = "0"
            };

            downloadTypeBox.Items.Add(defaultItemDownloadType);

            List<Download_Types> downloadTypes = DataBaseConnection.dataBaseEntities.Download_Types.ToList();

            foreach (Download_Types type in downloadTypes)
            {
                ComboBoxItem typeAdd = new ComboBoxItem()
                {
                    Content = $"{type.Name_Download_Type}",
                    Uid = $"{type.Code_Download_Type}"
                };

                downloadTypeBox.Items.Add(typeAdd);
            }
            downloadTypeBox.SelectedIndex = 0;

            //
            // Заполнение CB с доп. параметрами
            //
            ComboBoxItem defaultItemParameter = new ComboBoxItem()
            {
                Content = "Не выбрано",
                Uid = "0"
            };

            additionalParameterBox.Items.Add(defaultItemParameter);

            List<Additionally_Parameters> additionallyParameters = DataBaseConnection.dataBaseEntities.Additionally_Parameters.ToList();

            foreach (Additionally_Parameters parameter in additionallyParameters)
            {
                ComboBoxItem parameterAdd = new ComboBoxItem()
                {
                    Content = $"{parameter.Name_Additionally_Parameter}",
                    Uid = $"{parameter.Code_Additionally_Parameter}"
                };
                additionalParameterBox.Items.Add(parameterAdd);
            }
            additionalParameterBox.SelectedIndex = 0;
        }

        private Vehicles _vehicle;

        private void SetVehicleInformationForUpdate(Vehicles vehicle) // Метод для заполнения полей, если выбрано изменение автомобиля
        {

            header.Text = "ИЗМЕНЕНИЕ АВТОМОБИЛЯ"; // Заголовок страницы

            _vehicle = vehicle;


            //Проверка на значения null. Если значение != null, то устанавливается имеющееся значение в БД, а если == null, то устанавливается пустая строка
            if (_vehicle.Brand_Vehicle != null)
            {
                nameBrandInput.Text = _vehicle.Brand_Vehicle;
            }
            else
            {
                nameBrandInput.Text = "";
            }

            if (_vehicle.Model_Vehicle != null)
            {
                nameModelInput.Text = _vehicle.Model_Vehicle;
            }
            else
            {
                nameModelInput.Text = "";
            }

            if (_vehicle.Number_Vehicle != null)
            {
                //firstPartNumberInput.Text = Convert.ToString(_vehicle.Number_Vehicle[0] + _vehicle.Number_Vehicle[1] + _vehicle.Number_Vehicle[2] + _vehicle.Number_Vehicle[3] + _vehicle.Number_Vehicle[4] + _vehicle.Number_Vehicle[5]);

                string firstPartNumber = "";

                for (int i = 0; i < 6; i++)
                {
                    firstPartNumber += _vehicle.Number_Vehicle[i];
                }

                firstPartNumberInput.Text = firstPartNumber;

                string region = "";

                for (int i = 6; i < _vehicle.Number_Vehicle.Length; i++)
                {
                    region += _vehicle.Number_Vehicle[i];
                }

                regionNumberInput.Text = region;
            }
            else
            {
                firstPartNumberInput.Text = "";
                regionNumberInput.Text = "";
            }

            // Если значение null, то устанавливается нулевой элемент combobox, который является "Не выбрано", иначе элементы combobox конвертируются в ComboBoxItem и ищется запись в базе данных по Uid (Uid = ID записи в бд)

            //
            // Исполнители
            //
            foreach (ComboBoxItem executor in executorBox.Items)
            {
                if (Convert.ToInt32(executor.Uid) == _vehicle.Code_Executor)
                {
                    executorBox.SelectedValue = executor;
                }
            }

            //
            // Категории прицепов
            //
            foreach (ComboBoxItem category in categoryOfTrailerBox.Items)
            {
                if (Convert.ToInt32(category.Uid) == _vehicle.Code_Category)
                {
                    categoryOfTrailerBox.SelectedValue = category;
                }
            }

            //
            // Типы загрузки
            //
            foreach (ComboBoxItem downloadType in downloadTypeBox.Items)
            {
                if (Convert.ToInt32(downloadType.Uid) == _vehicle.Code_Download_Type)
                {
                    downloadTypeBox.SelectedValue = downloadType;
                }
            }

            //
            // Доп. параметры
            //
            foreach (ComboBoxItem addtionalParameter in additionalParameterBox.Items)
            {
                if (Convert.ToInt32(addtionalParameter.Uid) == _vehicle.Code_Additionally_Parameter)
                {
                    additionalParameterBox.SelectedValue = addtionalParameter;
                }
            }

            // Стандартное значение всегда равно 0. Если в БД значение == 0, то остается стандартное, иначе устанавливается то, которое прописано в БД
            if (_vehicle.Tonnage_Vehicle != null)
            {
                tonnageInput.Text = _vehicle.Tonnage_Vehicle.ToString();
            }

            if (_vehicle.Volume_Vehicle != null)
            {
                volumeInput.Text = _vehicle.Volume_Vehicle.ToString();
            }

            if (_vehicle.Width_Vehicle != null)
            {
                widthInput.Text = _vehicle.Width_Vehicle.ToString();
            }

            if (_vehicle.Length_Vehicle != null)
            {
                lengthInput.Text = _vehicle.Length_Vehicle.ToString();
            }

            if (_vehicle.Height_Vehicle != null)
            {
                heightInput.Text = _vehicle.Height_Vehicle.ToString();
            }
        }

        private void addVehicle_Click(object sender, RoutedEventArgs e) // При нажатии кнопки "Добавить"/"Изменить"
        {
            try
            {
                int? indexExecutor, indexTrailer, indexDownloadType, indexAdditionalParameter;

                // Если != 0, то устанавливается ID записи через UID, иначе = NULL
                if (executorBox.SelectedIndex != 0)
                {
                    ComboBoxItem executor = (ComboBoxItem)executorBox.SelectedItem;
                    indexExecutor = Convert.ToInt32(executor.Uid);
                }
                else
                {
                    indexExecutor = null;
                }

                if (categoryOfTrailerBox.SelectedIndex != 0)
                {
                    ComboBoxItem categoryTrailer = (ComboBoxItem)categoryOfTrailerBox.SelectedItem;
                    indexTrailer = Convert.ToInt32(categoryTrailer.Uid);
                }
                else
                {
                    indexTrailer = null;
                }

                if (downloadTypeBox.SelectedIndex != 0)
                {
                    ComboBoxItem downloadType = (ComboBoxItem)downloadTypeBox.SelectedItem;
                    indexDownloadType = Convert.ToInt32(downloadType.Uid);
                }
                else
                {
                    indexDownloadType = null;
                }

                if (additionalParameterBox.SelectedIndex != 0)
                {
                    ComboBoxItem addtitionalParameter = (ComboBoxItem)additionalParameterBox.SelectedItem;
                    indexAdditionalParameter = Convert.ToInt32(addtitionalParameter.Uid);
                }
                else
                {
                    indexAdditionalParameter = null;
                }

                string number = null;

                if (firstPartNumberInput.IsMaskCompleted && regionNumberInput.Text != "")
                {
                    number = firstPartNumberInput.Text.ToLower() + regionNumberInput.Text.ToLower();
                    number = number.Replace("_", "");
                }

                if (_vehicle == null) // Если объект ДОБАВЛЯЕТСЯ
                {
                    Vehicles newVehicle = new Vehicles()
                    {
                        Brand_Vehicle = nameBrandInput.Text,
                        Model_Vehicle = nameModelInput.Text,
                        Code_Executor = indexExecutor,
                        Code_Category = indexTrailer,
                        Code_Download_Type = indexDownloadType,
                        Code_Additionally_Parameter = indexAdditionalParameter,
                        Length_Vehicle = Convert.ToSingle(lengthInput.Text),
                        Width_Vehicle = Convert.ToSingle(widthInput.Text),
                        Height_Vehicle = Convert.ToSingle(heightInput.Text),
                        Tonnage_Vehicle = Convert.ToSingle(tonnageInput.Text),
                        Volume_Vehicle = Convert.ToSingle(volumeInput.Text),
                        Number_Vehicle = number
                    };

                    if (_vehicleListFromExecutors != null) // Если добавление автомобиля происходит из окна с добавлением исполнителя, то автомобиль добавляется в ListView окна добавления исполнителя
                    {
                        _vehicleListFromExecutors.Items.Add(newVehicle);
                    }
                    else // Иначе добавляем сразу в бд
                    {
                        DataBaseConnection.dataBaseEntities.Vehicles.Add(newVehicle);
                    }
                 
                }
                else // Если происходит изменение автомобиля
                {
                    _vehicle.Brand_Vehicle = nameBrandInput.Text;
                    _vehicle.Model_Vehicle = nameModelInput.Text;
                    _vehicle.Code_Executor = indexExecutor;
                    _vehicle.Code_Category = indexTrailer;
                    _vehicle.Code_Download_Type = indexDownloadType;
                    _vehicle.Code_Additionally_Parameter = indexAdditionalParameter;
                    _vehicle.Length_Vehicle = Convert.ToSingle(lengthInput.Text);
                    _vehicle.Width_Vehicle = Convert.ToSingle(widthInput.Text);
                    _vehicle.Height_Vehicle = Convert.ToSingle(heightInput.Text);
                    _vehicle.Tonnage_Vehicle = Convert.ToSingle(tonnageInput.Text);
                    _vehicle.Volume_Vehicle = Convert.ToSingle(volumeInput.Text);
                    _vehicle.Number_Vehicle = number;
                }

                if (_vehicleListFromExecutors == null) // Если изменение происходит со страницы с АВТОМОБИЛЯМИ, то сохраняем изменения, иначе значения меняем и сохраняем уже из окна добавления исполнителя при его окончательном добавлении в БД
                {
                    DataBaseConnection.dataBaseEntities.SaveChanges();
                    MessageBox.Show("Операция прошла успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private Regex _checkDigitRegex = new Regex(@"[\d,]"); // Регулярное выражение на запрет использования всех символов кроме цифр и запятой

        private void tonnageInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) // Использование регулярного выражения для всех полей с габаритами
        {
            e.Handled = !_checkDigitRegex.IsMatch(e.Text);
        }

        private void deleteVehicle_Click(object sender, RoutedEventArgs e) // Удаление автомобиля
        {
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow(); // Окно с подтверждением удаления
            confirmDelete.ShowDialog();
            if (confirmDelete.GetReturnCode() == 1) // Если возвращаемое значение == 1 (т.е удаление было подтверждено), то запись удаляется 
            {
                try
                {
                    DataBaseConnection.dataBaseEntities.Vehicles.Remove(_vehicle);

                    if (_vehicleListFromExecutors != null)
                    {
                        _vehicleListFromExecutors.Items.Remove(_vehicle);
                    }
                    else
                    {
                        DataBaseConnection.dataBaseEntities.SaveChanges();
                        MessageBox.Show("Удаление прошло успешно!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка во время удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
