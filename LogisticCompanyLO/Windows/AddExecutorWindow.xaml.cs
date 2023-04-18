using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LogisticCompanyLO
{
    public partial class AddExecutorWindow : Window
    {
        public AddExecutorWindow() // Конструктор при добавлении исполнителя
        {
            InitializeComponent();
        }

        private Executors _currentExecutor;

        public AddExecutorWindow(Executors executor) // Конструктор при изменении исполнителя
        {
            InitializeComponent();

            this.Closing += AddExecutorWindow_Closing;

            _currentExecutor = executor;

            executorSurnameInput.Text = _currentExecutor.Surname_Executor;
            executorNameInput.Text = _currentExecutor.Name_Executor;
            executorMidnameInput.Text = _currentExecutor.Midname_Executor;
            executorIdAti.Text = _currentExecutor.Id_Ati.ToString();
            executorTelephoneNumberInput.Text = _currentExecutor.Telephone_Executor;

            // Подгружаем все автомобили выбранного исполнителя и добавляем в ListView
            List<Vehicles> vehiclesCurrentExecutor = DataBaseConnection.dataBaseEntities.Vehicles.Where(x => x.Code_Executor == _currentExecutor.Code_Executor).ToList();

            foreach(Vehicles vehicleCurrentUser in vehiclesCurrentExecutor)
            {
                vehiclesList.Items.Add(vehicleCurrentUser);
            }

            title.Text = "ИЗМЕНЕНИЕ ИСПОЛНИТЕЛЯ"; // Изменение заголовка

            createExecutor.Content = "Изменить"; // Изменение контента кнопки

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
            string telephoneExecutor; 

            // Если обязательные значения маски не заполнены, то присваивается значение null
            if (executorTelephoneNumberInput.IsMaskCompleted)
            {
               telephoneExecutor = executorTelephoneNumberInput.Text;
            }
            else
            {
                telephoneExecutor = null;
            }

            int? idAti;
            // Если значение idAti не заполнено, то присваивается значение null
            if (executorIdAti.Text != "")
            {
                idAti = Convert.ToInt32(executorIdAti.Text);
            }
            else
            {
                idAti = null;
            }

            if (_currentExecutor == null) // Если исполнитель еще не был добавлен в базу данных (т.е. создание исполнителя)
            {
                Executors executor = new Executors()
                {
                    Surname_Executor = executorSurnameInput.Text,
                    Name_Executor = executorNameInput.Text,
                    Midname_Executor = executorMidnameInput.Text,
                    Id_Ati = idAti,
                    Telephone_Executor = telephoneExecutor
                };
                DataBaseConnection.dataBaseEntities.Executors.Add(executor);
                DataBaseConnection.dataBaseEntities.SaveChanges();
            }
            else // Если исполнитель уже есть в бд (т.е. изменение)
            {
                _currentExecutor.Surname_Executor = executorSurnameInput.Text;
                _currentExecutor.Name_Executor = executorNameInput.Text;
                _currentExecutor.Midname_Executor = executorMidnameInput.Text;
                _currentExecutor.Id_Ati = idAti;
                _currentExecutor.Telephone_Executor = telephoneExecutor;
            }

            if (vehiclesList.Items.Count != 0) // Если в ListView есть автомобили
            {
                if (_currentExecutor == null) // Если исполнитель только добавляется в бд, то находим его и присваиваем автомобилям этого исполнителя
                {
                    List<Executors> executors = DataBaseConnection.dataBaseEntities.Executors.ToList();
                    executors.Reverse();

                    Executors addedExecutor = executors.FirstOrDefault();

                    foreach (Vehicles vehicle in vehiclesList.Items)
                    {
                        vehicle.Code_Executor = addedExecutor.Code_Executor;
                        DataBaseConnection.dataBaseEntities.Vehicles.Add(vehicle);
                    }
                    
                }
                else // Если происходит изменение исполнителя, то игнорируются уже добавленные в БД автомобили, а добавляются только новые
                {
                    List<Vehicles> vehicles = DataBaseConnection.dataBaseEntities.Vehicles.ToList();

                    foreach(Vehicles vehicle in vehiclesList.Items)
                    {
                        if (vehicles.FirstOrDefault(x => x.Code_Vehicle == vehicle.Code_Vehicle) == null)
                        {
                            vehicle.Code_Executor = _currentExecutor.Code_Executor;
                            DataBaseConnection.dataBaseEntities.Vehicles.Add(vehicle);
                        }
                    }
                }
                DataBaseConnection.dataBaseEntities.SaveChanges();
            }

            MessageBox.Show("Операция выполнена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                DataBaseConnection.dataBaseEntities.Executors.Remove(_currentExecutor);
                DataBaseConnection.dataBaseEntities.SaveChanges();
                MessageBox.Show("Удаление прошло успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Question);
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
                    DataBaseConnection.dataBaseEntities.Vehicles.Remove(vehicleDelete);
                    vehiclesList.Items.Remove(vehicleDelete);
                    DataBaseConnection.dataBaseEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка во время удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
