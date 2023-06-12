using Microsoft.Office.Interop.Word;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Window = System.Windows.Window;

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
            headerPage.Text = "Автомобили";

            vehiclesPage.Focus();
            vehiclesNavigation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            vehiclesEnable.Opacity = 1.0;
            vehiclesDisable.Opacity = 0.0;
        }

        private void disableVehiclesAnimation()
        {
            DoubleAnimation visibilityAnimation = new DoubleAnimation();
            visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
            visibilityAnimation.To = 1;

            vehiclesDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

            visibilityAnimation.To = 0;

            vehiclesEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

            ColorAnimation foregroundColor = new ColorAnimation();
            ColorConverter converter = new ColorConverter();
            Color newColor = (Color)converter.ConvertFrom("#FFC5C5C5");
            foregroundColor.To = newColor;
            foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
            vehiclesNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
        }

        private void disableExecutorsAnimation()
        {
            DoubleAnimation visibilityAnimation = new DoubleAnimation();
            visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
            visibilityAnimation.To = 1;

            executorsDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

            visibilityAnimation.To = 0;

            executorsEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

            ColorAnimation foregroundColor = new ColorAnimation();
            ColorConverter converter = new ColorConverter();
            Color newColor = (Color)converter.ConvertFrom("#FFC5C5C5");
            foregroundColor.To = newColor;
            foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
            executorsNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
        }

        private void disableDriversAnimation()
        {
            DoubleAnimation visibilityAnimation = new DoubleAnimation();
            visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
            visibilityAnimation.To = 1;

            driversDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

            visibilityAnimation.To = 0;

            driversEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

            ColorAnimation foregroundColor = new ColorAnimation();
            ColorConverter converter = new ColorConverter();
            Color newColor = (Color)converter.ConvertFrom("#FFC5C5C5");
            foregroundColor.To = newColor;
            foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
            driversNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
        }

        private void disableOrdersAnimation()
        {
            DoubleAnimation visibilityAnimation = new DoubleAnimation();
            visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
            visibilityAnimation.To = 1;

            ordersDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

            visibilityAnimation.To = 0;

            ordersEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

            ColorAnimation foregroundColor = new ColorAnimation();
            ColorConverter converter = new ColorConverter();
            Color newColor = (Color)converter.ConvertFrom("#FFC5C5C5");
            foregroundColor.To = newColor;
            foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
            ordersNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
        }

        private void vehiclesPage_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new VehiclesList());
            headerPage.Text = "Автомобили";

            vehiclesNavigation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
   
            vehiclesEnable.Opacity = 1;
            vehiclesDisable.Opacity = 0;

            if (executorsEnable.Opacity == 1)
            {
                disableExecutorsAnimation();
            }

            if (driversEnable.Opacity == 1)
            {
                disableDriversAnimation();
            }

            if (ordersEnable.Opacity == 1)
            {
                disableOrdersAnimation();
            }

        }

        private void executorsPage_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ExecutorsListPage());
            headerPage.Text = "Исполнители";

            executorsNavigation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            executorsEnable.Opacity = 1.0;
            executorsDisable.Opacity = 0.0;

            if (vehiclesEnable.Opacity == 1)
            {
                disableVehiclesAnimation();
            }

            if (driversEnable.Opacity == 1)
            {
                disableDriversAnimation();
            }

            if (ordersEnable.Opacity == 1)
            {
                disableOrdersAnimation();
            }
        }

        private void driversPage_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new DriversListPage());
            headerPage.Text = "Водители";

            driversNavigation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            driversEnable.Opacity = 1;
            driversDisable.Opacity = 0;

            if (executorsEnable.Opacity == 1)
            {
                disableExecutorsAnimation();
            }

            if (vehiclesEnable.Opacity == 1)
            {
                disableVehiclesAnimation();
            }

            if (ordersEnable.Opacity == 1)
            {
                disableOrdersAnimation();
            }
        }

        private void ordersPage_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new OrdersListPage());
            headerPage.Text = "Заявки";

            ordersNavigation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D8DF0"));
            ordersEnable.Opacity = 1;
            ordersDisable.Opacity = 0;

            if (executorsEnable.Opacity == 1)
            {
                disableExecutorsAnimation();
            }

            if (vehiclesEnable.Opacity == 1)
            {
                disableVehiclesAnimation();
            }

            if (driversEnable.Opacity == 1)
            {
                disableDriversAnimation();
            }
        }

        private void vehiclesPage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Автомобили")
            {

                DoubleAnimation visibilityAnimation = new DoubleAnimation();
                visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                visibilityAnimation.To = 0;

                vehiclesDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

                visibilityAnimation.To = 1;
                
                vehiclesEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

                ColorAnimation foregroundColor = new ColorAnimation();
                ColorConverter converter = new ColorConverter();
                Color newColor = (Color)converter.ConvertFrom("#FF7D8DF0");
                foregroundColor.To = newColor;
                foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
                vehiclesNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
                
            }
        }

        private void vehiclesPage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Автомобили")
            {
                disableVehiclesAnimation();
            }
        }

        private void executorsPage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Исполнители")
            {
                DoubleAnimation visibilityAnimation = new DoubleAnimation();
                visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                visibilityAnimation.To = 0;

                executorsDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

                visibilityAnimation.To = 1;

                executorsEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

                ColorAnimation foregroundColor = new ColorAnimation();
                ColorConverter converter = new ColorConverter();
                Color newColor = (Color)converter.ConvertFrom("#FF7D8DF0");
                foregroundColor.To = newColor;
                foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
                executorsNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
            }
        }

        private void executorsPage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Исполнители")
            {
                disableExecutorsAnimation();
            }
        }

        private void driversPage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Водители")
            {
                DoubleAnimation visibilityAnimation = new DoubleAnimation();
                visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                visibilityAnimation.To = 0;

                driversDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

                visibilityAnimation.To = 1;

                driversEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

                ColorAnimation foregroundColor = new ColorAnimation();
                ColorConverter converter = new ColorConverter();
                Color newColor = (Color)converter.ConvertFrom("#FF7D8DF0");
                foregroundColor.To = newColor;
                foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
                driversNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
            }
        }

        private void driversPage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Водители")
            {
                disableDriversAnimation();
            }
        }

        private void ordersPage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Заявки")
            {
                DoubleAnimation visibilityAnimation = new DoubleAnimation();
                visibilityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                visibilityAnimation.To = 0;

                ordersDisable.BeginAnimation(OpacityProperty, visibilityAnimation);

                visibilityAnimation.To = 1;

                ordersEnable.BeginAnimation(OpacityProperty, visibilityAnimation);

                ColorAnimation foregroundColor = new ColorAnimation();
                ColorConverter converter = new ColorConverter();
                Color newColor = (Color)converter.ConvertFrom("#FF7D8DF0");
                foregroundColor.To = newColor;
                foregroundColor.Duration = TimeSpan.FromSeconds(0.2);
                ordersNavigation.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColor);
            }
        }

        private void ordersPage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (headerPage.Text != "Заявки")
            {
                disableOrdersAnimation();
            }
        }
    }
}
