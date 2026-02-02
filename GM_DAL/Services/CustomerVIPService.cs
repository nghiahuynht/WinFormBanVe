using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models;
using GM_DAL.Models.CustomerVIP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GM_DAL.Services
{
    public class CustomerVIPService : BaseService, ICustomerVIPService
    {
        private SQLAdoContext adoContext;

        public CustomerVIPService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }

        // ================== TÌM KIẾM ==================
        public APIResultObject<List<CustomerVIPModel>> TimKiem(
            string keyword,
            int vipLevel,
            int start,
            int length,
            out int totalRow)
        {
            totalRow = 0;
            var res = new APIResultObject<List<CustomerVIPModel>>();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Keyword", keyword);
                parameters.Add("@VipLevel", vipLevel);
                parameters.Add("@Start", start);
                parameters.Add("@Length", length);
                parameters.Add("@TotalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<CustomerVIPModel>(
                        "sp_CustomerVIP_timkiem",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    res.data = result.ToList();
                    totalRow = parameters.Get<int>("@TotalRow");
                }
            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }

            return res;
        }

        // ================== THÊM ==================
        public APIResultObject<ResCommon> Them(CustomerVIPModel model)
        {
            var res = new APIResultObject<ResCommon>();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CustomerCode", model.CustomerCode);
                parameters.Add("@CustomerName", model.CustomerName);
                parameters.Add("@Phone", model.Phone);
                parameters.Add("@Email", model.Email);
                parameters.Add("@Address", model.Address);
                parameters.Add("@Note", model.Note);
                parameters.Add("@VipLevel", model.VipLevel);
                parameters.Add("@DiscountPercent", model.DiscountPercent);
                parameters.Add("@CreatedBy", model.CreatedBy);

                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<ResCommon>(
                        "sp_CustomerVIP_them",
                        parameters,
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

        // ================== SỬA ==================
        public APIResultObject<ResCommon> Sua(CustomerVIPModel model)
        {
            var res = new APIResultObject<ResCommon>();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@CustomerCode", model.CustomerCode);
                parameters.Add("@CustomerName", model.CustomerName);
                parameters.Add("@Phone", model.Phone);
                parameters.Add("@Email", model.Email);
                parameters.Add("@Address", model.Address);
                parameters.Add("@Note", model.Note);
                parameters.Add("@VipLevel", model.VipLevel);
                parameters.Add("@DiscountPercent", model.DiscountPercent);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<ResCommon>(
                        "sp_CustomerVIP_sua",
                        parameters,
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

        // ================== XÓA (SOFT DELETE) ==================
        public APIResultObject<ResCommon> Xoa(int id, string updatedBy)
        {
            var res = new APIResultObject<ResCommon>();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@UpdatedBy", updatedBy);

                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<ResCommon>(
                        "sp_CustomerVIP_xoa",
                        parameters,
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


        public APIResultObject<CustomerVIPModel> FinCustomerByCustomerCode(string customerCode)
        {
            var res = new APIResultObject<CustomerVIPModel>();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CustomerCode", customerCode);

                using (var connection = adoContext.CreateConnection())
                {
                    var result = connection.Query<CustomerVIPModel>(
                        "sp_CustomerFindInfoByCustomerCode",
                        parameters,
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






    }
}
