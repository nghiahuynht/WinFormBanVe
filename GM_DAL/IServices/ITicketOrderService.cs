using GM_DAL.Models.TicketOrder;
using GM_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.IServices
{
    public interface ITicketOrderService
    {
        Task<APIResultObject<ResCommon>> SaveOrderInfo(PostOrderSaveModel model, string userName);
        APIResultObject<List<PrintModel>> ListSubCodeForPrint(long orderId);
        Task<APIResultObject<TicketOrderHeaderModel>> GetHeaderOrderById(long orderId);
    }
}
