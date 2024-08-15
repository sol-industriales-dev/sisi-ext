using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ICobrosDiversosDAO
    {
        tblPro_CobrosDiversos GetJsonData(FiltrosGeneralDTO filtro);
        void GuardarActualizarCobrosDiversos(FiltrosGeneralDTO objFiltro, CobrosDivDTO obj);
    }
}
