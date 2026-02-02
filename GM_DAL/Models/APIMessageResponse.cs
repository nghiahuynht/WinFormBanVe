using GM_DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models
{
    public class APIMessageResponse
    {
        public APIMessageResponse(ResultCode resultCode = ResultCode.Ok)
        {
            message = resultCode.GetDescription();
        }
        public string message { set; get; }
        public string exMessage { get; set; }
    }
}
