using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models.User;
using GM_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GM_DAL.Models.TicketOrder;

namespace GM_DAL.Services
{
    public class TicketOrderService: BaseService,ITicketOrderService
    {
        private SQLAdoContext adoContext;

        public TicketOrderService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }


        public async Task<APIResultObject<ResCommon>> SaveUserInfo(PostOrderSaveModel model)
        {
            var res = new APIResultObject<ResCommon>();
            try
            {


                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id",0);
                parameters.Add("@CustomerCode", CommonHelper.CheckStringNull(model.CustomerCode));
                parameters.Add("@CustomerName", CommonHelper.CheckStringNull(model.CustomerName));
                parameters.Add("@CustomerType", CommonHelper.CheckStringNull(model.CustomerType));
                parameters.Add("@TicketCode", CommonHelper.CheckStringNull(model.TicketCode));
                parameters.Add("@Quanti", CommonHelper.CheckIntNull(model.Quanti));
                parameters.Add("@BienSoXe", CommonHelper.CheckStringNull(model.BienSoXe));
                parameters.Add("@IsCopy", false);
                parameters.Add("@GateName", CommonHelper.CheckStringNull(model.GateName));
                parameters.Add("@Objtype", CommonHelper.CheckStringNull(model.ObjType));
                parameters.Add("@IsFree", CommonHelper.CheckBooleanNull(model.IsFree));
                parameters.Add("@PrintType", CommonHelper.CheckStringNull(model.PrintType));
                parameters.Add("@DiscountPercent", CommonHelper.CheckIntNull(model.DiscountPercent));
                parameters.Add("@DiscountValue", CommonHelper.CheckDecimalNull(model.DiscountValue));
                parameters.Add("@TienKhachDua", CommonHelper.CheckDecimalNull(model.TienKhachDua));
                parameters.Add("@PaymentType", CommonHelper.CheckStringNull(model.PaymentType));
                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>("sp_SaveOrderTicketWinForm", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }


            return res;
        }


        public APIResultObject<List<PrintModel>> ListSubCodeForPrint(long orderId)
        {
            var res = new APIResultObject<List<PrintModel>>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderId", CommonHelper.CheckLongNull(orderId));
                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = connection.Query<PrintModel>("sp_GetListSubTicketOrderByOrderId", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.ToList();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                res.data = new List<PrintModel>();
                res.message.exMessage = ex.Message;
            }

            return res;
        }


        public async Task<APIResultObject<TicketOrderHeaderModel>> GetHeaderOrderById(long orderId)
        {
            var res = new APIResultObject<TicketOrderHeaderModel>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderId", CommonHelper.CheckLongNull(orderId));
                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<TicketOrderHeaderModel>("sp_TicketOrderHeaderById", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                res.data = new TicketOrderHeaderModel();
                res.message.exMessage = ex.Message;
            }

            return res;
        }

    }
}
