using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace LogisticCompanyLO
{  
    public partial class VehiclesList : Page
    {
        public VehiclesList()
        {
            InitializeComponent();

            vehiclesList.ItemsSource = DataBaseConnection.dataBaseEntities.Vehicles.ToList();

            typeTrailerBox.ItemsSource = DataBaseConnection.dataBaseEntities.Category_Trailer.ToList();
            typeTrailerBox.Text = "Не выбрано";

            executorBox.Items.Add("Не выбрано");

            List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();

            foreach (Executors executor in executors)
            {
                ComboBoxItem executorName = new ComboBoxItem() // Добавляем элементы в combobox через comboboxitem, чтобы обращаться к записям в бд через Uid, в котором хранится ID записи в БД
                {
                    Content = $"{executor.Surname_Executor} {executor.Name_Executor} {executor.Midname_Executor}",
                    Uid = $"{executor.Code_Executor}"
                };
                executorBox.Items.Add(executorName);
            }
            executorBox.SelectedIndex = 0;
        }

        private List<Vehicles> _vehiclesFiltrationList;

        private void GetFiltrationVehicles() // Фильтрация данных
        {
            _vehiclesFiltrationList = DataBaseConnection.dataBaseEntities.Vehicles.ToList();

            // Фильтрация по грузоподъемности (от и до)

            if (startRangeLoadCapacityInput.Text != "") 
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Tonnage_Vehicle >= Convert.ToDouble(startRangeLoadCapacityInput.Text)).ToList();
            }

            if (endRangeLoadCapacityInput.Text != "")
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Tonnage_Vehicle <= Convert.ToDouble(endRangeLoadCapacityInput.Text)).ToList();
            }

            // Фильтрация по объему кузова (от и до)

            if (startRangeVolumeBodyInput.Text != "")
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Volume_Vehicle >= Convert.ToDouble(startRangeVolumeBodyInput.Text)).ToList();
            }

            if (endRangeVolumeBodyInput.Text != "")
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Volume_Vehicle <= Convert.ToDouble(endRangeVolumeBodyInput.Text)).ToList();
            }

            // Фильтрация по длине кузова (от и до)

            if (startRangeLengthBodyInput.Text != "") 
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Length_Vehicle >= Convert.ToDouble(startRangeLengthBodyInput.Text)).ToList();
            }

            if (endRangeLengthBodyInput.Text != "")
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Length_Vehicle <= Convert.ToDouble(endRangeLengthBodyInput.Text)).ToList();
            }

            // Фильтрация по ширине кузова (от и до)

            if (startRangeWidthBodyInput.Text != "") 
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Width_Vehicle >= Convert.ToDouble(startRangeWidthBodyInput.Text)).ToList();
            }

            if (endRangeWidthBodyInput.Text != "")
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Width_Vehicle <= Convert.ToDouble(endRangeWidthBodyInput.Text)).ToList();
            }

            // Фильтрация по высоте (от и до)

            if (startRangeHeightBodyInput.Text != "") 
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Height_Vehicle >= Convert.ToDouble(startRangeHeightBodyInput.Text)).ToList();
            }

            if (endRangeHeightBodyInput.Text != "")
            {
                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Height_Vehicle <= Convert.ToDouble(endRangeHeightBodyInput.Text)).ToList();
            }

            if (typeTrailerBox.SelectedItems.Count > 0) // Срабатывает, если выбран хотя бы один ТИП КУЗОВА есть хотя бы одно выделенное значение
            {
                List<Vehicles> listVehicle = new List<Vehicles>(); // Новый список автомобилей для фильтрации по типу кузова

                foreach (Category_Trailer category in typeTrailerBox.SelectedItems) // Цикл по выбранным элементам в CheckComboBox
                {
                    listVehicle = listVehicle.Concat(_vehiclesFiltrationList.Where(x => x.Code_Category == category.Code_Trailer)).ToList(); // Добавляет в новый список все выделенные типы кузова
                }

                _vehiclesFiltrationList = listVehicle;
            }

            if (executorBox.SelectedIndex > 0) // Если выбран исполнитель
            {
                ComboBoxItem executor = (ComboBoxItem)executorBox.SelectedItem; // Конвертируем элемента комбо бокса в текст блок и берем Uid значение, в котором вписан ID исполнителя из БД

                _vehiclesFiltrationList = _vehiclesFiltrationList.Where(x => x.Code_Executor == Convert.ToInt32(executor.Uid)).ToList();
            }
            
            vehiclesList.ItemsSource = _vehiclesFiltrationList;
        }

        private void startRangeLoadCapacityInput_TextChanged(object sender, TextChangedEventArgs e) // На все поля для ввода с габаритами установлено это событие, которое вызывает метод фильтрации
        {
            GetFiltrationVehicles();
        }

        Regex dimensionsTrailerRegex = new Regex(@"[0-9,]"); // Регулярное выражение для ввода только цифр и запятой

        private void startRangeLoadCapacityInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) // Событие для проверки вводимых символов на все поля, где требуется ввод только цифр
        {
            e.Handled = !dimensionsTrailerRegex.IsMatch(e.Text);
        }

        private void typeTrailerBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e) // При выборе значения из combobox'a с типами кузовов
        {
            if (typeTrailerBox.SelectedItems.Count == 0) // Если не выбран ни один тип кузова
            {
                typeTrailerBox.Text = "Выберите тип кузова";
            }
           


            GetFiltrationVehicles();
        }

        private void executorBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // При выборе значения из combobox'a с исполнителями
        {
            GetFiltrationVehicles();
        }

        private void addVehicle_Click(object sender, RoutedEventArgs e) // При добавлении автомобиля
        {
            CreateVehicleWindow createVehicleWindow = new CreateVehicleWindow();
            createVehicleWindow.ShowDialog();
            FrameClass.frame.Navigate(new VehiclesList());
        }

        private void vehiclesList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) // При изменении автомобиля
        {
            Vehicles selectedCar = (Vehicles)vehiclesList.SelectedItem;
            Vehicles vehicle = DataBaseConnection.dataBaseEntities.Vehicles.FirstOrDefault(x => x.Code_Vehicle == selectedCar.Code_Vehicle);

            CreateVehicleWindow updateVehicle = new CreateVehicleWindow(vehicle);
            updateVehicle.Title = "Fregate Logistic - Изменение автомобиля";
            updateVehicle.ShowDialog();
            FrameClass.frame.Navigate(new VehiclesList());
        }

        private void deleteVehicle_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow(); // Окно с подтверждением удаления

            confirmDelete.ShowDialog();

            if (confirmDelete.GetReturnCode() == 1) // Если возвращаемое значение == 1 (т.е удаление было подтверждено), то запись удаляется 
            {
                try
                {
                    Vehicles vehicle = (Vehicles)vehiclesList.SelectedItem;
                    DataBaseConnection.dataBaseEntities.Vehicles.Remove(vehicle);
                    MessageBox.Show("Удаление прошло успешно!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataBaseConnection.dataBaseEntities.SaveChanges();
                    FrameClass.frame.Navigate(new VehiclesList());
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка во время удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void clearFiltration_Click(object sender, RoutedEventArgs e)
        {
            startRangeLoadCapacityInput.Clear();
            endRangeLoadCapacityInput.Clear();
            startRangeVolumeBodyInput.Clear();
            endRangeVolumeBodyInput.Clear();
            startRangeLengthBodyInput.Clear();
            endRangeLengthBodyInput.Clear();
            startRangeWidthBodyInput.Clear();
            endRangeWidthBodyInput.Clear();
            startRangeHeightBodyInput.Clear();
            endRangeHeightBodyInput.Clear();
            typeTrailerBox.SelectAll();
            executorBox.SelectedIndex = 0;
        }

        private void startRangeLoadCapacityInput_GotFocus(object sender, RoutedEventArgs e)
        {
            loadCapacityHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            startRangeLoadCapacityText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void startRangeLoadCapacityInput_LostFocus(object sender, RoutedEventArgs e)
        {
            loadCapacityHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            startRangeLoadCapacityText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void endRangeLoadCapacityInput_GotFocus(object sender, RoutedEventArgs e)
        {
            loadCapacityHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            endRangeLoadCapacityText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void endRangeLoadCapacityInput_LostFocus(object sender, RoutedEventArgs e)
        {
            loadCapacityHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            endRangeLoadCapacityText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void startRangeVolumeBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            volumeBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            startRangeVolumeBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void startRangeVolumeBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            volumeBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            startRangeVolumeBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void endRangeVolumeBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            volumeBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            endRangeVolumeBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void endRangeVolumeBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            volumeBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            endRangeVolumeBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void startRangeLengthBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            lengthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            startRangeLengthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void startRangeLengthBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            lengthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            startRangeLengthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void endRangeLengthBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            lengthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            endRangeLengthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void endRangeLengthBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            lengthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            endRangeLengthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void startRangeWidthBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            widthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            startRangeWidthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void startRangeWidthBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            widthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            startRangeWidthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void endRangeWidthBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            widthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            endRangeWidthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void endRangeWidthBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            widthBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            endRangeWidthBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void startRangeHeightBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            heightBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            startRangeHeightBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void startRangeHeightBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            heightBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            startRangeHeightBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void endRangeHeightBodyInput_GotFocus(object sender, RoutedEventArgs e)
        {
            heightBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            endRangeHeightBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void endRangeHeightBodyInput_LostFocus(object sender, RoutedEventArgs e)
        {
            heightBodyHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
            endRangeHeightBodyText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }

        private void executorBox_DropDownOpened(object sender, EventArgs e)
        {
            executorSelectHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
        }

        private void executorBox_DropDownClosed(object sender, EventArgs e)
        {
            executorSelectHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC818181"));
        }
    }
}
