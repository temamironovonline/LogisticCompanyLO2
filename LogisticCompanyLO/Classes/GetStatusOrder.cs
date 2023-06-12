
namespace LogisticCompanyLO
{
    partial class Orders
    {
        public string SetStatusOrder
        {
            get
            {
                switch (Status_Order)
                {
                    case true:
                        return "Закрыта";


                    case false:
                        return "Открыта";

                    default:
                        return null;

                }
            }
        }
    }
}
