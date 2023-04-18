using System.Windows;

namespace LogisticCompanyLO
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataBaseConnection.dataBaseEntities = new DataBaseEntities();
            FrameClass.frame = mainFrame;
            FrameClass.frame.Navigate(new VehiclesList());
            headerPage.Text = "СПИСОК АВТОМОБИЛЕЙ";
        }

        private void vehiclesPage_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new VehiclesList());
            headerPage.Text = "СПИСОК АВТОМОБИЛЕЙ";
        }

        private void executorsPage_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ExecutorsListPage());
            headerPage.Text = "СПИСОК ИСПОЛНИТЕЛЕЙ";
        }
    }
}
