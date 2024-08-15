using Core.DTO.Enkontrol.Tablas.CC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Enkontrol.General.CC
{
    public interface ICCDAO
    {
        List<ccDTO> GetCCs();
        List<ccDTO> GetCCs(List<string> ccs);
        ccDTO GetCC(string cc);

        #region NOMINAS
        ccDTO GetCCNomina(string cc);
        List<ccDTO> GetCCsNomina(bool? activos);
        List<ccDTO> GetCCsNominaFiltrados(List<string> ccs);
        List<ccDTO> GetCCsNominaInactivos();
        List<ccDTO> GetCCsNominaInactivos(List<string> ccs);
        #endregion
    }
}
