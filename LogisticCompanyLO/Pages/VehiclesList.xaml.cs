using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
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
            typeTrailerBox.Text = "Выберите тип кузова";

            executorBox.Items.Add("Не выбран");

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

        private void GetFiltrationData() // Фильтрация данных
        {
            List<Vehicles> vehicles = DataBaseConnection.dataBaseEntities.Vehicles.ToList();

            if (loadCapacityInput.Text != "") // Если поле для ввода грузоподъемности (тоннаж) не пустое
            {
                double tonnage, tonnageRange, tonnageMinimum, tonnageMaximum;

                tonnage = Convert.ToDouble(loadCapacityInput.Text);

                if (loadCapacityRangeInput.Text != "") // Если поле для ввода погрешности не пустое
                {
                    tonnageRange = Convert.ToDouble(loadCapacityRangeInput.Text);
                    tonnageMinimum = tonnage - tonnageRange; // Находим минимальное значение при введенном диапазоне, т.е введенная грузоподъемность - введенная погрешность
                    tonnageMaximum = tonnage + tonnageRange; // Находим максимальное значение при введенном диапазоне, т.е введенная грузоподъемность + введенная погрешность
                }
                else // Если поле для ввода пустое, то присваиваем минимальному и максимальному значениям диапазона значение введенное в поле с грузоподъемностью. Т.е поиск автомобилей будет осуществляться только по первому значению без диапазона
                {
                    tonnageMinimum = tonnage;
                    tonnageMaximum = tonnage;
                }

                vehicles = vehicles.Where(x => x.Tonnage_Vehicle >= tonnageMinimum && x.Tonnage_Vehicle <= tonnageMaximum).ToList(); // Фильтрация автомобилей с грузоподъемностью от минимального введенного до максимального
            }

            if (volumeBodyInput.Text != "") // Если поле для ввода объема прицепа не пустое
            {
                double volume, volumeRange, volumeMinimum, volumeMaximum;

                volume = Convert.ToDouble(volumeBodyInput.Text);

                if (volumeBodyRangeInput.Text != "") // Если погрешность не пустая
                {
                    volumeRange = Convert.ToDouble(volumeBodyRangeInput.Text);
                    volumeMinimum = volume - volumeRange; // Находим минимальное значение при введенном диапазоне, т.е введенный объем - введенная погрешность
                    volumeMaximum = volume + volumeRange; // Находим максимальное значение при введенном диапазоне, т.е введенный объем + введенная погрешность
                }
                else // Иначе, если погрешность пустая, то минимальному и максимальному значениям диапазона присваиваем введенное значение объема. Т.е поиск автомобилей будет осуществляться только по первому значению без диапазона
                {
                    volumeMinimum = volume;
                    volumeMaximum = volume;
                }

                vehicles = vehicles.Where(x => x.Volume_Vehicle >= volumeMinimum && x.Volume_Vehicle <= volumeMaximum).ToList(); // Фильтрация автомобилей с объемом прицепа от минимального введенного до максимального
            }

            // С длиной прицепа аналогичная логика фильтрации информации
            if (lengthBodyInput.Text != "")
            {
                double length, lengthRange, lengthMinimum, lengthMaximum;

                length = Convert.ToDouble(lengthBodyInput.Text);

                if (lengthBodyRangeInput.Text != "")
                {
                    lengthRange = Convert.ToDouble(lengthBodyRangeInput.Text);
                    lengthMinimum = length - lengthRange;
                    lengthMaximum = length + lengthRange;
                }
                else
                {
                    lengthMinimum = length;
                    lengthMaximum = length;
                }

                vehicles = vehicles.Where(x => x.Length_Vehicle >= lengthMinimum && x.Length_Vehicle <= lengthMaximum).ToList();
            }

            // С шириной прицепа аналогичная логика фильтрации информации
            if (widthBodyInput.Text != "")
            {
                double width, widthRange, widthMinimum, widthMaximum;

                width = Convert.ToDouble(widthBodyInput.Text);

                if (widthBodyRangeInput.Text != "")
                {
                    widthRange = Convert.ToDouble(widthBodyRangeInput.Text);
                    widthMinimum = width - widthRange;
                    widthMaximum = width + widthRange;
                }
                else
                {
                    widthMinimum = width;
                    widthMaximum = width;
                }

                vehicles = vehicles.Where(x => x.Width_Vehicle >= widthMinimum && x.Width_Vehicle <= widthMaximum).ToList();
            }

            // С высотой прицепа аналогичная логика фильтрации информации
            if (heightBodyInput.Text != "")
            {
                double height, heightRange, heightMinimum, heightMaximum;

                height = Convert.ToDouble(heightBodyInput.Text);

                if (heightBodyRangeInput.Text != "")
                {
                    heightRange = Convert.ToDouble(heightBodyRangeInput.Text);
                    heightMinimum = height - heightRange;
                    heightMaximum = height + heightRange;
                }
                else
                {
                    heightMinimum = height;
                    heightMaximum = height;
                }

                vehicles = vehicles.Where(x => x.Height_Vehicle >= heightMinimum && x.Height_Vehicle <= heightMaximum).ToList();
            }

            if (typeTrailerBox.SelectedItems.Count > 0) // Срабатывает, если выбран хотя бы один ТИП КУЗОВА есть хотя бы одно выделенное значение
            {
                List<Vehicles> listVehicle = new List<Vehicles>(); // Новый список автомобилей для фильтрации по типу кузова

                foreach (Category_Trailer category in typeTrailerBox.SelectedItems) // Цикл по выбранным элементам в CheckComboBox
                {
                    listVehicle = listVehicle.Concat(vehicles.Where(x => x.Code_Category == category.Code_Trailer)).ToList(); // Добавляет в новый список все выделенные типы кузова
                }

                vehicles = listVehicle;
            }

            if (executorBox.SelectedIndex > 0) // Если выбран исполнитель
            {
                ComboBoxItem executor = (ComboBoxItem)executorBox.SelectedItem; // Конвертируем элемента комбо бокса в текст блок и берем Uid значение, в котором вписан ID исполнителя из БД

                vehicles = vehicles.Where(x => x.Code_Executor == Convert.ToInt32(executor.Uid)).ToList();
            }
            
            vehiclesList.ItemsSource = vehicles;
        }

        private void loadCapacityInput_TextChanged(object sender, TextChangedEventArgs e) // На все поля для ввода с габаритами установлено это событие, которое вызывает метод фильтрации
        {
            GetFiltrationData();
        }

        Regex dimensionsTrailerRegex = new Regex(@"[0-9,]"); // Регулярное выражение для ввода только цифр и запятой

        private void loadCapacityInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) // Событие для проверки вводимых символов на все поля, где требуется ввод только цифр
        {
            e.Handled = !dimensionsTrailerRegex.IsMatch(e.Text);
        }

        private void typeTrailerBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e) // При выборе значения из combobox'a с типами кузовов
        {
            if (typeTrailerBox.SelectedItems == null) // Если не выбран ни один тип кузова
            {
                typeTrailerBox.Text = "Выберите тип кузова";
            }
            GetFiltrationData();
        }

        private void executorBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // При выборе значения из combobox'a с исполнителями
        {
            GetFiltrationData();
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
            updateVehicle.Title = "CARAVELLA - Изменение автомобиля";
            updateVehicle.ShowDialog();
            FrameClass.frame.Navigate(new VehiclesList());
        }

        private void deleteVehicle_Click(object sender, RoutedEventArgs e)
        {
            Vehicles vehicle = (Vehicles)vehiclesList.SelectedItem;

            ConfirmDeleteWindow confirmDelete = new ConfirmDeleteWindow(); // Окно с подтверждением удаления
            confirmDelete.ShowDialog();
            if (confirmDelete.GetReturnCode() == 1) // Если возвращаемое значение == 1 (т.е удаление было подтверждено), то запись удаляется 
            {
                try
                {
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
    }
}
