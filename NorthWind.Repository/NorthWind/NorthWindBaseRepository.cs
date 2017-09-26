using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Repository.NorthWind
{
    public class NorthWindBaseRepository : baseRespository
    {
        public NorthWindBaseRepository()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["NorthWind"].ConnectionString;
            baseCon = new SqlConnection(connectionString);
        }
    }
}
