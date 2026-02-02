using GM_DAL.Models.User;
using GM_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GM_DAL.IServices
{
    public interface IUserInfoService
    {
        Task<APIResultObject<AuthenSuccessModel>> Login(string userName, string pass);
        //Task<APIResultObject<List<MenuModel>>> GetMenuByRole(string role);
        Task<APIResultObject<ResCommon>> SaveUserInfo(UserInfoModel model);
        //Task<APIResultObject<UserInfoModel>> GetUserById(int id);
        Task<DataTableResultModel<UserInfoModel>> SearchUserAccount(SearchUserFilterModel filter);
        //Task<APIResultObject<ResCommon>> ChangePass(ChangePassModel model);
        Task<APIResultObject<ResCommon>> change_pass(string userName, string currentPass, string newPass);
        Task<APIResultObject<ResCommon>> reset_pass(int userId, string newPass);
        
        Task<APIResultObject<ResCommon>> DeleteUser(int userIdDelete, string userName);
    }
}
