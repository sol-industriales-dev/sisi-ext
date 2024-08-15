using Core.DAO.Contabilidad.Banco;
using Core.Service.Contabilidad.Banco;
using Data.DAO.Enkontrol.General.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Banco
{
    public class BancoFactoryService
    {
        public IBancoDAO GetBancoEkService()
        {
            return new BancoService(new BancoEkDAO());
        }
    }
}
