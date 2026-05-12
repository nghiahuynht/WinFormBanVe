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
        private const int MaxRetries = 3; // Thử lại tối đa 3 lần

        public SQLAdoContext()
        {
            connecString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connecString);
        }

       

        // Kiểm tra xem lỗi có phải do mạng tạm thời hay không
        private bool IsTransientError(SqlException ex)
        {
            // Các mã lỗi SQL phổ biến do mạng/server tạm thời không phản hồi
            // 4060, 40197, 40501, 40613, 49918, 49919, 49920, 11001, 233
            int[] transientErrors = { 11001, 233, 10053, 10054, 10060, 40613 };
            return transientErrors.Contains(ex.Number);
        }
    }
}
