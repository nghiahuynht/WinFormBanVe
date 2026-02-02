using Dapper;
using GM_DAL.IServices;
using GM_DAL.Models;
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
       

    }
}
