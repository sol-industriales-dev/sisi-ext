
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario.ControlCalidad
{
    public interface IControlCalidadDAO
    {
        List<tblM_CatControlCalidad> getListControlCalidad();
        tblM_CatControlCalidad getControlCalidadById(int idAsignacion, int TipoControl);
        //raguiar
        tblM_CatControlCalidad getControlCalidadById(int idAsignacion, int TipoControl, int TipoFiltro);
        tblM_CatControlCalidad saveControlCalidad(tblM_CatControlCalidad obj);
        tblM_CatControlCalidad getByIDAsignacion(int id);
        tblM_CatControlCalidad getByIDAsignacionTipo(int id, int tipoControl);

        Dictionary<string, object> guardarControlMovimientoMaquinaria(tblM_CatControlCalidad objCalidad,
                                                 List<tblM_RelPreguntaControlCalidad> lstRespuestas,
                                                 tblM_ControlMovimientoMaquinaria objControl,
                                                 string areaCuentaDestino, string areaCuentaOrigen,
                                                 int tipoControl, int envioEspecial);

    }
}
