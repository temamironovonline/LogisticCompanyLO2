using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticCompanyLO
{
    partial class Vehicles
    {
        public char? GetFirstLetterNumber
        {
            get
            {
                if (Number_Vehicle != null)
                {
                    return Number_Vehicle[0];
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
                if (Number_Vehicle != null)
                {
                    return $"{Number_Vehicle[1]}{Number_Vehicle[2]}{Number_Vehicle[3]}";
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
                if (Number_Vehicle != null)
                {
                    return $"{Number_Vehicle[4]}{Number_Vehicle[5]}";
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
                if (Number_Vehicle != null)
                {
                    string regionNumber = "";
                    for (int i = 6; i < Number_Vehicle.Length; i++)
                    {
                        regionNumber += Number_Vehicle[i];
                    }
                    return regionNumber;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
