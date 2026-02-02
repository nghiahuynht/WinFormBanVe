using GM_DAL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GM_DAL.Models;
using GM_DAL.Models.User;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;


namespace GM_DAL.Services
{
    public class UserInfoService: BaseService, IUserInfoService
    {
        private SQLAdoContext adoContext;
        public UserInfoService(SQLAdoContext adoContext)
        {
            this.adoContext = adoContext;
        }


        public async Task<APIResultObject<AuthenSuccessModel>> Login(string userName, string pass)
        {
            var res = new APIResultObject<AuthenSuccessModel>();
            try
            {


                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserName", CommonHelper.CheckStringNull(userName));
                parameters.Add("@Pass", CommonHelper.CheckStringNull(pass));
                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<AuthenSuccessModel>("sp_Login", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.FirstOrDefault();
                }



            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }

            return res;
        }
        //public async Task<APIResultObject<ResCommon>> ChangePass(ChangePassModel model)
        //{
        //    var res = new APIResultObject<ResCommon>();
        //    try
        //    {


        //        DynamicParameters parameters = new DynamicParameters();
        //        parameters.Add("@UserName", CommonHelper.CheckStringNull(model.userName));
        //        parameters.Add("@CurentPass", CommonHelper.CheckStringNull(model.currentPass));
        //        parameters.Add("@NewPass", CommonHelper.CheckStringNull(model.newPass));
        //        using (var connection = adoContext.CreateConnection())
        //        {
        //            var resultExcute = await connection.QueryAsync<ResCommon>("sp_ChangePass", parameters, commandType: CommandType.StoredProcedure);
        //            res.data = resultExcute.FirstOrDefault();
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        res.code = Enum.ResultCode.ErrorException;
        //        res.message.exMessage = ex.Message;
        //    }

        //    return res;
        //}


        //public async Task<APIResultObject<List<MenuModel>>> GetMenuByRole(string role)
        //{
        //    var res = new APIResultObject<List<MenuModel>>();
        //    try
        //    {


        //        DynamicParameters parameters = new DynamicParameters();
        //        parameters.Add("@RoleCode", CommonHelper.CheckStringNull(role));
        //        using (var connection = adoContext.CreateConnection())
        //        {
        //            var resultExcute = await connection.QueryAsync<MenuModel>("sp_GetMenuByRole", parameters, commandType: CommandType.StoredProcedure);
        //            res.data = resultExcute.ToList();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        res.message.exMessage = ex.Message;
        //    }

        //    return res;
        //}



        public async Task<APIResultObject<ResCommon>> SaveUserInfo(UserInfoModel model)
        {
            var res = new APIResultObject<ResCommon>();
            try
            {


                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserName", CommonHelper.CheckStringNull(model.loginName));
                parameters.Add("@FullName", CommonHelper.CheckStringNull(model.fullName));
                parameters.Add("@Phone", CommonHelper.CheckStringNull(model.phone));
                parameters.Add("@Email", CommonHelper.CheckStringNull(model.email));
                parameters.Add("@Role", CommonHelper.CheckStringNull(model.roleCode));
                parameters.Add("@Title", CommonHelper.CheckStringNull(model.title));
                parameters.Add("@Address", CommonHelper.CheckStringNull(model.address));
                parameters.Add("@Status", CommonHelper.CheckStringNull(model.status));
                parameters.Add("@WorKBeginDate", CommonHelper.CheckDateNull(model.workBeginDate));
                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>("sp_UserSaveInfo", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }


            return res;
        }





        public async Task<APIResultObject<ResCommon>> change_pass(string userName, string currentPass, string newPass)
        {
            var res = new APIResultObject<ResCommon>();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TenTaiKhoan", CommonHelper.CheckStringNull(userName));
                parameters.Add("@MatKhauCu", CommonHelper.CheckStringNull(currentPass));
                parameters.Add("@MatKhauMoi", CommonHelper.CheckStringNull(newPass));

                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>(
                        "sp_UserInfo_changepass",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    res.data = resultExcute.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }

            return res;
        }








        public async Task<APIResultObject<ResCommon>> DeleteUser(int userIdDelete,string userName)
        {
            var res = new APIResultObject<ResCommon>();
            try
            {


                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserIdDelete", CommonHelper.CheckIntNull(userIdDelete));
                parameters.Add("@UserAction", CommonHelper.CheckStringNull(userName));
                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>("sp_UserDelete", parameters, commandType: CommandType.StoredProcedure);
                    res.data = resultExcute.FirstOrDefault();
                }



            }
            catch (Exception ex)
            {
                res.message.exMessage = ex.Message;
            }


            return res;
        }

        public async Task<DataTableResultModel<UserInfoModel>> SearchUserAccount(SearchUserFilterModel filter)
        {
            var res = new DataTableResultModel<UserInfoModel>();
            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@SearchType", CommonHelper.CheckStringNull(filter.searchType));
                parameters.Add("@RoleCode", CommonHelper.CheckStringNull(filter.roleCode));
                parameters.Add("@Keyword", CommonHelper.CheckStringNull(filter.keyword));
                parameters.Add("@Start", CommonHelper.CheckStringNull(filter.start));
                parameters.Add("@Length", CommonHelper.CheckStringNull(filter.length));
                parameters.Add(name: "@TotalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var connection = adoContext.CreateConnection())
                {

                    var resultExcute = await connection.QueryAsync<UserInfoModel>("sp_SearchUserInfo", parameters, commandType: CommandType.StoredProcedure);
                    res.recordsTotal = parameters.Get<int>("TotalRow");
                    res.data = resultExcute.ToList();

                }

            }
            catch (Exception ex)
            {
                res.data = new List<UserInfoModel>();
            }


            return res;
        }
        public async Task<APIResultObject<ResCommon>> reset_pass(int userId, string newPass)
        {
            var res = new APIResultObject<ResCommon>();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", CommonHelper.CheckIntNull(userId));
                parameters.Add("@NewPass", CommonHelper.CheckStringNull(newPass));

                using (var connection = adoContext.CreateConnection())
                {
                    var resultExcute = await connection.QueryAsync<ResCommon>(
                        "sp_UserInfo_resetpass",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    res.data = resultExcute.FirstOrDefault();
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
