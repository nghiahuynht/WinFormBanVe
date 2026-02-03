using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models;
using GM_DAL.Models.TicketGroup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GM_DAL.Services
{
    public class TicketGroupService : BaseService, ITicketGroupService
    {
        private readonly SQLAdoContext adoContext;

        public TicketGroupService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }

        public APIResultObject<List<TicketGroupModel>> toanbo()
        {
            var res = new APIResultObject<List<TicketGroupModel>>();

            try
            {
                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<TicketGroupModel>(
                        "sp_GetTicketGroupDDL",
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
