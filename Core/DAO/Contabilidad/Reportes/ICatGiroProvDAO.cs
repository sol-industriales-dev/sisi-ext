using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface ICatGiroProvDAO
    {
        bool saveGiro(tblC_CatGiro giro);
        List<tblC_CatGiro> getAllGiro();
        List<tblC_CatGiro> getLstGiro();
        List<ComboDTO> getCboGiro();
    }
}
