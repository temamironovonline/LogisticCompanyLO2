using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticCompanyLO
{
    partial class Orders
    {
        public char? GetFirstLetterNumber
        {
            get
            {
                Vehicles vehicle = DataBaseConnection.dataBaseEntities.Vehicles.FirstOrDefault(x => x.Code_Vehicle == Code_Vehicle);
                
                if (vehicle != null)
                {
                    if (vehicle.Number_Vehicle != null)
                    {
                        return vehicle.Number_Vehicle[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
               
            }
        }

        public string GetDigitsNumber
        {
            get
            {
                Vehicles vehicle = DataBaseConnection.dataBaseEntities.Vehicles.FirstOrDefault(x => x.Code_Vehicle == Code_Vehicle);

                if (vehicle != null)
                {
                    if (vehicle.Number_Vehicle != null)
                    {
                        return $"{vehicle.Number_Vehicle[1]}{vehicle.Number_Vehicle[2]}{vehicle.Number_Vehicle[3]}";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }  
            }
        }

        public string GetLastLettersNumber
        {
            get
            {
                Vehicles vehicle = DataBaseConnection.dataBaseEntities.Vehicles.FirstOrDefault(x => x.Code_Vehicle == Code_Vehicle);

                if (vehicle != null)
                {
                    if (vehicle.Number_Vehicle != null)
                    {
                        return $"{vehicle.Number_Vehicle[4]}{vehicle.Number_Vehicle[5]}";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetRegionNumber
        {
            get
            {
                Vehicles vehicle = DataBaseConnection.dataBaseEntities.Vehicles.FirstOrDefault(x => x.Code_Vehicle == Code_Vehicle);

                if (vehicle != null)
                {
                    if (vehicle.Number_Vehicle != null)
                    {
                        string regionNumber = "";
                        for (int i = 6; i < vehicle.Number_Vehicle.Length; i++)
                        {
                            regionNumber += vehicle.Number_Vehicle[i];
                        }
                        return regionNumber;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
