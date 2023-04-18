using System.Windows;

namespace LogisticCompanyLO
{
    public partial class ConfirmDeleteWindow : Window
    {
        public ConfirmDeleteWindow()
        {
            InitializeComponent();
        }

        private int _returnCode; // Возвращаемое значение
        
        private void confirmButton_Click(object sender, RoutedEventArgs e) // Подтверждение удаления
        {
            _returnCode = 1; 
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e) // Отмена удаления
        {
            _returnCode = 0;
            this.Close();
        }

        public int GetReturnCode()
        {
            return _returnCode;
        }
    }
}
