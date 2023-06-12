using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace LogisticCompanyLO
{
    internal class SameDrivers
    {
        public static int CheckSameDriversCreate(string numbersPassport, int? idAti, string telephoneNumber, string driverLicense)
        {

            if (numbersPassport != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Passport_Numbers == numbersPassport) != null)
                {
                    return 1;
                }
            }

            if (idAti != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Id_Ati == idAti) != null)
                {
                    return 2;
                }
            }

            if (telephoneNumber != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Telephone_Number == telephoneNumber) != null)
                {
                    return 3;
                }
            }

            if (driverLicense != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Driver_License_Numbers == driverLicense) != null)
                {
                    return 4;
                }
            }

            return 0;
        }

        public static int CheckSameDriversUpdate(Drivers driver, string numbersPassport, int? idAti, string telephoneNumber, string driverLicense)
        {
            if (numbersPassport != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Passport_Numbers == numbersPassport && x.Code_Personal_Data != driver.Code_Personal_Data) != null)
                {
                    return 1;
                }
            }

            if (idAti != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Id_Ati == idAti && x.Code_Personal_Data != driver.Code_Personal_Data) != null)
                {
                    return 2;
                }
            }

            if (telephoneNumber != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Telephone_Number == telephoneNumber && x.Code_Personal_Data != driver.Code_Personal_Data) != null)
                {
                    return 3;
                }
            }

            if (driverLicense != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Driver_License_Numbers == driverLicense && x.Code_Personal_Data != driver.Code_Personal_Data) != null)
                {
                    return 4;
                }
            }

            return 0;
        }
    }
}
