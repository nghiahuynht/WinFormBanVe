using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace GM_DAL
{
    public class SQLAdoContext
    {
        private string connecString = string.Empty;
        public SQLAdoContext()
        {
            connecString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        /// <summary>
        /// Trả về chuỗi kết nối (Connection String) đã được cấu hình.
        /// </summary>
        /// <returns>Chuỗi kết nối SQL Server.</returns>



        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connecString);
        }
    }
}
