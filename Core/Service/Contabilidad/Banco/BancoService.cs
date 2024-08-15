using Core.DAO.Contabilidad.Banco;
using Core.DTO.Enkontrol.Tablas.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Banco
{
    public class BancoService : IBancoDAO
    {
        private IBancoDAO _bancoDAO;

        private IBancoDAO BancoDAO
        {
            get { return _bancoDAO; }
            set { _bancoDAO = value; }
        }

        public BancoService(IBancoDAO banco)
        {
            this.BancoDAO = banco;
        }

        public sb_cuentaDTO GetBanco(int cta, int scta, int sscta)
        {
            return this.BancoDAO.GetBanco(cta, scta, sscta);
        }
    }
}
