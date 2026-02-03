using GM_DAL.Models;
using GM_DAL.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.IServices
{
    public interface ICustomerService
    {
        // customerTypeKey = Code của CustomerType (vd: "KHALE", "VIP"... tuỳ bạn lưu)
        APIResultObject<List<CustomerModel>> TimKiemTheoCusType(string customerTypeKey);
    }
}
