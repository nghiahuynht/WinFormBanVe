using GM_DAL.Models;          // ✅ cái này để nhận ra APIResultObject
using GM_DAL.Models.Ticket;   // ✅ TicketModel
using System.Collections.Generic;

namespace GM_DAL.IServices
{
    public interface ITicketService
    {
        APIResultObject<List<TicketModel>> toanbo();
        APIResultObject<TicketModel> timkiemvetheoten(string code);
    }
}
