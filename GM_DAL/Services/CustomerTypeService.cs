using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models;
using GM_DAL.Models.CustomerType;
using GM_DAL.Models.CustomerType.GM_DAL.Models.CustomerType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GM_DAL.Services
{
    public class CustomerTypeService : BaseService, ICustomerTypeService
    {
        private readonly SQLAdoContext adoContext;

        public CustomerTypeService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }

        public APIResultObject<List<CustomerTypeModel>> toanbo()
        {
            var res = new APIResultObject<List<CustomerTypeModel>>();

            try
            {
                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<CustomerTypeModel>(
                        "sp_CustomerType_tatca",
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
