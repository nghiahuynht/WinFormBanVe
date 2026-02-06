using GM_DAL.Models;
using GM_DAL.Models.TicketGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.IServices
{
    public interface ITicketGroupService
    {
        APIResultObject<List<TicketGroupModel>> toanbo();
    }
}
