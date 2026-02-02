using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GM_DAL.Enum
{
   // [JsonConverter(typeof(StringEnumConverter))]
    public enum ResultCode
    {
        [Description("Thành công.")]
        Ok = 200,
        /// <summary>
        /// success
        /// </summary>
        [Description("Thành công.")]
        OkPass = 201,

        // system error
        /// <summary>
        /// system error: lỗi chứng thực  
        /// </summary>
        [Description("Thất bại.")]
        ErrorAuthenticationFail = -120,

        /// <summary>
        /// system error: lỗi input
        /// </summary>
        [Description("Thất bại.")]
        ErrorInputInvalid = -121,

        /// <summary>
        /// system error: checksum fail
        /// </summary>
        [Description("Thất bại.")]
        ErrorChecksumFail = -122,

        /// <summary>
        /// system error: access-token hết hạn
        /// </summary>
        [Description("Thất bại.")]
        ErrorTokenExpired = -123,

        /// <summary>
        /// system error: lỗi phân quyền
        /// </summary>
        [Description("Thất bại.")]
        ErrorAuthorizationFail = 401,

        /// <summary>
        /// system error: bảo trì
        /// </summary>
        [Description("Thất bại.")]
        ErrorMaintenance = -199,

        // error
        /// <summary>
        /// lỗi chung business
        /// </summary>
        ErrorFail = -110,

        /// <summary>
        /// lỗi exception
        /// </summary>
        [Description("Thất bại.")]
        ErrorException = -130,

        /// <summary>
        /// lỗi không tìm thấy dữ liệu
        /// </summary>
        [Description("Thất bại.")]
        ErrorNotFound = -140,

        /// <summary>
        /// lỗi không có dữ liệu
        /// </summary>
        [Description("Thất bại.")]
        ErrorNoContent = -141,

        /// <summary>
        /// lỗi dữ liệu đã tồn tại
        /// </summary>
        [Description("Thất bại.")]
        ErrorConflict = -142,

        // warning
        /// <summary>
        /// cảnh báo chung
        /// </summary>
        [Description("Thất bại.")]
        Warning = -301,

        /// <summary>
        /// không có data
        /// </summary>
        [Description("Thất bại.")]
        WarningNoContent = -302,

        /// <summary>
        /// không có quyền truy cập
        /// </summary>
        [Description("Thất bại.")]
        WarningForbidden = 403,
    }
}
