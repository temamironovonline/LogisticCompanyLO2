using System.Linq;

namespace LogisticCompanyLO
{
    internal class SameExecutors
    {
        public static int CheckSameExecutorsCreate(string passportNumbers, int? idAti, string telephoneNumber, string driverLicense, string tinNumber) // Проверка всех значений, которые не могут повторяться
        {
            if (passportNumbers != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Passport_Numbers == passportNumbers) != null) // Если в бд есть запись с таким номером-серией паспорта
                {
                    return 1;
                }
            }
            
            if (idAti != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Id_Ati == idAti) != null) // Если в бд есть запись с таким кодом АТИ паспорта
                {
                    return 2;
                }
            }
          
            if (telephoneNumber != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Telephone_Number == telephoneNumber) != null) // Если в бд есть запись с таким номером телефона паспорта
                {
                    return 3;
                }
            }

            if (driverLicense != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Driver_License_Numbers == driverLicense) != null) // Если в бд есть запись с таким номером телефона паспорта
                {
                    return 4;
                }
            }

            if (tinNumber != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Tin_Number == tinNumber) != null) // Если в бд есть запись с таким номером телефона паспорта
                {
                    return 5;
                }
            }

            return 0;
        }

        public static int CheckSameExecutorsUpdate(Executors selectedExecutor, string passportNumbers, int? idAti, string telephoneNumber, string driverLicense, string tinNumber)
        {
            if (passportNumbers != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Passport_Numbers == passportNumbers && x.Code_Personal_Data != selectedExecutor.Code_Personal_Data) != null)
                {
                    return 1;
                }
            }

            if (idAti != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Id_Ati == idAti && x.Code_Personal_Data != selectedExecutor.Code_Personal_Data) != null)
                {
                    return 2;
                }
            }

            if (telephoneNumber != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Telephone_Number == telephoneNumber && x.Code_Personal_Data != selectedExecutor.Code_Personal_Data) != null)
                {
                    return 3;
                }
            }

            if (driverLicense != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Driver_License_Numbers == driverLicense && x.Code_Personal_Data != selectedExecutor.Code_Personal_Data) != null)
                {
                    return 4;
                }
            }

            if (tinNumber != null)
            {
                if (DataBaseConnection.dataBaseEntities.Personal_Data.FirstOrDefault(x => x.Tin_Number == tinNumber && x.Code_Personal_Data != selectedExecutor.Code_Personal_Data) != null)
                {
                    return 5;
                }
            }

            return 0;
        }
    }
}
