using Core.DTO.Administracion.Seguridad.AgrupacionCC;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.Seguridad.AgrupacionCC
{
    public interface IAgrupacionCCDAO
    {

        List<ComboDTO> getCC();
        List<ComboDTO> getCCTodos(int idAgrupacionCC);
        List<ComboDTO> ObtnerAgrupacion();
        List<AgrupacionCCDTO> GetDetalleAgrupacion(int idAgrupacionCC);
        AgrupacionCCDTO CrearAgrupacion(AgrupacionCCDTO objAgrupaciones, List<tblS_IncidentesAgrupacionCCDet> lstAgrupaciones);
        bool EditarAgrupacion(int id, string NuevoNombre, string[] lstAgrupacion);
        bool EliminarAgrupacion(int id, int esActivo);
        List<ComboDTO> obtenerAgrupacionCombo();

        #region 

        #endregion

    }
}
