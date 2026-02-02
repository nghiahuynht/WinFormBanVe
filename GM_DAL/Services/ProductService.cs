using Dapper;
using GM_DAL.IServices;
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

       


    }
}
