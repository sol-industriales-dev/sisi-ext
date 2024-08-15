using Core.DAO.Enkontrol.General.CC;
using Core.Service.Enkontrol.General.CC;
using Data.DAO.Enkontrol.General.CC;
using Data.DAO.Principal.CC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Enkontrol.General.CC
{
    public class CCFactoryService
    {
        public ICCDAO getCCService()
        {
            return new CCService(new CCDAO());
        }

        public ICCDAO getCCServiceSP()
        {
            return new CCService(new CCDAO_SP());
        }
    }
}
