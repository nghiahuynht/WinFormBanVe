using GM_DAL.Models;
using GM_DAL.Models.CustomerVIP;
using System.Collections.Generic;

namespace GM_DAL.IServices
{
    public interface ICustomerVIPService
    {
        APIResultObject<List<CustomerVIPModel>> TimKiem(
            string keyword,
            int vipLevel,
            int start,
            int length,
            out int totalRow
        );

        APIResultObject<ResCommon> Them(CustomerVIPModel model);
        APIResultObject<ResCommon> Sua(CustomerVIPModel model);
        APIResultObject<ResCommon> Xoa(int id, string updatedBy);
        APIResultObject<CustomerVIPModel> FinCustomerByCustomerCode(string customerCode);
    }
}