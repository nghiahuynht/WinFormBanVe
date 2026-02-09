using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models;
using GM_DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GM_DAL.Services
{
    public class TicketService : BaseService, ITicketService
    {
        private readonly SQLAdoContext adoContext;

        public TicketService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }

        public APIResultObject<List<TicketModel>> toanbo()
        {
            var res = new APIResultObject<List<TicketModel>>();

            try
            {
                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<TicketModel>(
                        "sp_ListAllTicket",
                        commandType: CommandType.StoredProcedure);

                    res.data = result.ToList();
                }
            }
            catch (Exception ex)
            {
                res.data = new List<TicketModel>();
                res.message.exMessage = ex.Message;
            }

            return res;
        }

        // ✅ TÌM KIẾM VÉ THEO TÊN (thực tế là theo Code)
        public APIResultObject<TicketModel> timkiemvetheoten(string code)
        {
            var res = new APIResultObject<TicketModel>();

            try
            {
                var p = new DynamicParameters();
                p.Add("@Code", code);

                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<TicketModel>(
                        "sp_Search_Ticket_byCode",
                        p,
                        commandType: CommandType.StoredProcedure);

                    res.data = result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }

            return res;
        }



        public APIResultObject<List<TicketPricePolicyModel>> GetListPricePolicy()
        {
            var res = new APIResultObject<List<TicketPricePolicyModel>>();

            try
            {
                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<TicketPricePolicyModel>(
                        "sp_GetAllPricePolicy",
                        commandType: CommandType.StoredProcedure);

                    res.data = result.ToList();
                }
            }
            catch (Exception ex)
            {
                res.data = new List<TicketPricePolicyModel>();
                res.message.exMessage = ex.Message;
            }

            return res;
        }





    }
}
