using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GM_DAL.Enum;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GM_DAL.Models
{
    public class APIResultObject<T>
    {
       
        public APIResultObject()
        {
            this.code = ResultCode.Ok;
            this.message = new APIMessageResponse(this.code);
        }

        public ResultCode code { get; set; }
        public T? data { get; set; }
        public APIMessageResponse message { get; set; }

    }
}
