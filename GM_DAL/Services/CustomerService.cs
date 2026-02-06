using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models;
using GM_DAL.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly SQLAdoContext adoContext;

        public CustomerService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }

        public APIResultObject<List<CustomerModel>> TimKiemTheoCusType(string customerTypeKey)
        {
            var res = new APIResultObject<List<CustomerModel>>();

            try
            {
                var p = new DynamicParameters();
                p.Add("@CustomerTypeKey", customerTypeKey);

                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<CustomerModel>(
                        "sp_Customer_searchCusType",
                        p,
                        commandType: CommandType.StoredProcedure);

                    res.data = result.ToList();
                }
            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }

            return res;
        }
    }
}
