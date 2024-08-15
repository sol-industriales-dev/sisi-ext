using Core.DTO.Maquinaria.Captura.aceites;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IMaquinariaAceitesLubricantesDAO
    {
        tblM_MaquinariaAceitesLubricantes GuardarMaqAceiteLubricante(tblM_MaquinariaAceitesLubricantes obj);
        List<MaquinariaAceitesLubricantesDTO> GetLstMaqAceiteLubricante(string cc, string consumo, int turno, DateTime fecha, int tipo);
        List<tblM_MaquinariaAceitesLubricantes> GetRepMaqAceiteLubricante(string cc, int turno, DateTime inicio, DateTime fin, string economico);

       

    }
}
