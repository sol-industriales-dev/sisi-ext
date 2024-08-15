using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface IAdministracionDAO
    {
        tblPro_Administracion GetJsonData(FiltrosGeneralDTO filtro);
        void GuardarActualizarAdministracion(FiltrosGeneralDTO objFiltro, AdministracionDTO obj);
    }
}
