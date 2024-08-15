using Core.DAO.SAAP;
using Core.Service.SAAP;
using Data.DAO.SAAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.SAAP
{
    public class SAAPFactoryService
    {
        public ISAAPDAO getSAAPService()
        {
            return new SAAPService(new SAAPDAO());
        }
    }
}
