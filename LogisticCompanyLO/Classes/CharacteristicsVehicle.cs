using System.Linq;

namespace LogisticCompanyLO
{
    partial class Vehicles
    {
        public string GetCategoryTrailer
        {
            get
            {
                if (Code_Category != null)
                {
                    Category_Trailer category = DataBaseConnection.dataBaseEntities.Category_Trailer.FirstOrDefault(x => x.Code_Trailer == Code_Category);
                    return category.Name_Trailer;
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetDownloadType
        {
            get
            {
                if (Code_Download_Type != null)
                {
                    Download_Types type = DataBaseConnection.dataBaseEntities.Download_Types.FirstOrDefault(x => x.Code_Download_Type == Code_Download_Type);
                    return type.Name_Download_Type;
                }
                else
                {
                    return null;
                }
                
            }
        }

        public string GetAdditionalParameter
        {
            get
            {
                if (Code_Additionally_Parameter != null)
                {
                    Additionally_Parameters parameter = DataBaseConnection.dataBaseEntities.Additionally_Parameters.FirstOrDefault(x => x.Code_Additionally_Parameter == Code_Additionally_Parameter);
                    return parameter.Name_Additionally_Parameter;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
