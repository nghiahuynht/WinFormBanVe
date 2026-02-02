using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models.ItemsModel;
using GM_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Services
{
    public class ProductService:BaseService, IProductService
    {
        private SQLAdoContext adoContext;
        public ProductService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }

        /*
        public async Task<APIResultObject<ResCommon>> SaveProduct(ProductItemModel model, string userName)
        {
            var res = new APIResultObject<ResCommon>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@Id", model.id);
                parameters.Add("@Name", CommonHelper.CheckStringNull(model.name));
                parameters.Add("@Unit", CommonHelper.CheckStringNull(model.unit));
                parameters.Add("@Price", model.price);
                parameters.Add("@CategoryCode", CommonHelper.CheckStringNull(model.categoryCode));
                parameters.Add("@ImgThunail", CommonHelper.CheckStringNull(model.imgThunail));
                parameters.Add("@UserName", CommonHelper.CheckStringNull(userName));

                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>("sp_ProductSave", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                res.message.exMessage = ex.Message;
            }

            return res;
        }

        */

        public async Task<APIResultObject<ResCommon>> SaveProduct(ProductItemModel model, string userName)
        {
            var res = new APIResultObject<ResCommon>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@Id", model.id);
                parameters.Add("@Name", CommonHelper.CheckStringNull(model.name));
                parameters.Add("@Unit", CommonHelper.CheckStringNull(model.unit));

                parameters.Add("@Price", model.price);

                // ✅ NEW: lưu đơn giá nhập
                parameters.Add("@DonGiaNhap", model.dongianhap);

                parameters.Add("@CategoryCode", CommonHelper.CheckStringNull(model.categoryCode));
                parameters.Add("@ImgThunail", CommonHelper.CheckStringNull(model.imgThunail));
                parameters.Add("@UserName", CommonHelper.CheckStringNull(userName));

                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>(
                        "sp_ProductSave",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    res.data = resultExcute.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }

            return res;
        }


        public async Task<APIResultObject<ResCommon>> DeleteProduct(int productId, string userName)
        {
            var res = new APIResultObject<ResCommon>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductId", productId);
                parameters.Add("@UserName", CommonHelper.CheckStringNull(userName));

                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>("sp_ProductSoftDelete", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                res.message.exMessage = ex.Message;
            }

            return res;
        }




        public DataTableResultModel<ProductItemModel> SearchProduct(ProductFilterModel filter)
        {
            var resSearch = new DataTableResultModel<ProductItemModel>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Category", CommonHelper.CheckStringNull(filter.category));
                parameters.Add("@TrangThaiTon", CommonHelper.CheckStringNull(filter.trangThaiTon));
                parameters.Add("@Keyword", CommonHelper.CheckStringNull(filter.keyword));
                parameters.Add("@Start", CommonHelper.CheckIntNull(filter.start));
                parameters.Add("@Length", CommonHelper.CheckIntNull(filter.length));
                parameters.Add(name: "@TotalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);


                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = connection.Query<ProductItemModel>("sp_ProductSearchPaging", parameters, commandType: CommandType.StoredProcedure);
                    resSearch.recordsTotal = parameters.Get<int>("TotalRow");
                    resSearch.data = resultExcute.ToList();
                }
            }
            catch (Exception ex)
            {
                resSearch.data = new List<ProductItemModel>();
            }
            return resSearch;
        }





        public APIResultObject<List<ComboboxModel>> GetAllCategory()
        {
            var res = new APIResultObject<List<ComboboxModel>>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = connection.Query<ComboboxModel>("sp_CategoryGetAll", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.ToList();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                res.message.exMessage = ex.Message;
            }

            return res;
        }


    }
}
