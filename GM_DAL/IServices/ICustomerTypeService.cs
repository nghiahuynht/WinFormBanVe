using GM_DAL.Models;
using GM_DAL.Models.CustomerType;
using GM_DAL.Models.CustomerType.GM_DAL.Models.CustomerType;
using System.Collections.Generic;

namespace GM_DAL.IServices
{
    public interface ICustomerTypeService
    {
        APIResultObject<List<CustomerTypeModel>> toanbo();
    }
}
