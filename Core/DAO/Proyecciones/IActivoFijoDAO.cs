using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface IActivoFijoDAO
    {
        tblPro_ActivoFijo GetJsonData(FiltrosGeneralDTO objFiltro);
        void GuardarActualizarActivoFijo(tblPro_ActivoFijo obj);
        int getUltimoMesCapturado(int Mes);
    }
}
