using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Services
{
    public class BaseService
    {
        public void ValidNullValue(SqlParameter[] paramList)
        {
            foreach (SqlParameter p in paramList)
            {
                if (p.Value == null)
                    p.Value = DBNull.Value;
            }
        }
    }
}
