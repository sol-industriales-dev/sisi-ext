using Core.DAO.Enkontrol.General.CC;
using Core.DTO.Enkontrol.Tablas.CC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Enkontrol.General.CC
{
    public class CCService : ICCDAO
    {
        private ICCDAO _ccDAO;

        public ICCDAO CCDAO
        {
            get { return _ccDAO; }
            set { _ccDAO = value; }
        }

        public CCService(ICCDAO cc)
        {
            this.CCDAO = cc;
        }

        public List<ccDTO> GetCCs()
        {
            return this.CCDAO.GetCCs();
        }

        public List<ccDTO> GetCCs(List<string> ccs)
        {
            return this.CCDAO.GetCCs(ccs);
        }

        public ccDTO GetCC(string cc)
        {
            return this.CCDAO.GetCC(cc);
        }

        #region NOMINAS
        public ccDTO GetCCNomina(string cc)
        {
            return this.CCDAO.GetCCNomina(cc);
        }

        public List<ccDTO> GetCCsNomina(bool? activos)
        {
            return this.CCDAO.GetCCsNomina(activos);
        }

        public List<ccDTO> GetCCsNominaFiltrados(List<string> ccs)
        {
            return this.CCDAO.GetCCsNominaFiltrados(ccs);
        }

        public List<ccDTO> GetCCsNominaInactivos()
        {
            return this.CCDAO.GetCCsNominaInactivos();
        }

        public List<ccDTO> GetCCsNominaInactivos(List<string> ccs)
        {
            return this.CCDAO.GetCCsNominaInactivos(ccs);
        }
        #endregion
    }
}
