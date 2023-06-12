using System.Windows.Media;

namespace LogisticCompanyLO
{
    partial class Orders
    {
        public SolidColorBrush SetColor
        {
            get
            {
                switch (Status_Order)
                {
                    case true:
                        return Brushes.Red;


                    case false:
                        return Brushes.Green;

                    default:
                        return null;
                }
            }
        }
    }
}
