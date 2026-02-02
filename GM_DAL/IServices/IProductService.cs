using GM_DAL.Models.ItemsModel;
using GM_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.IServices
{
    public interface IProductService
    {
        Task<APIResultObject<ResCommon>> SaveProduct(ProductItemModel model, string userName);
        Task<APIResultObject<ResCommon>> DeleteProduct(int productId, string userName);
        DataTableResultModel<ProductItemModel> SearchProduct(ProductFilterModel filter);
        APIResultObject<List<ComboboxModel>> GetAllCategory();
    }
}
